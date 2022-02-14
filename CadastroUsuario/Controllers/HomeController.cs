using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CadastroUsuario
{
    public class HomeController : Controller
    {
        protected readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var dados = new Dictionary<int, string>();
            var usuario = dbContext.Usuario.ToList();
            foreach (var item in usuario)
            {
                dados.Add(item.Id, item.Nome);
            }
            return View(dados);
        }

        [HttpGet]
        [Route("/Detalhes/{idUsuario}")]
        public IActionResult Detalhes(int idUsuario)
        {
            var usuario = dbContext.Usuario.Where(x => x.Id == idUsuario).Include(x => x.Enderecos).FirstOrDefault();
            var endereco = usuario.Enderecos.FirstOrDefault();
            var model = new CadastroUsuarioViewModel()
            {
                Nome = usuario.Nome,
                DataNascimento = usuario.DataNascimento.ToString("dd/MM/yyyy"),
                CPF = usuario.CPF,
                Email = usuario.Email,
                CEP = endereco.CEP,
                Logradouro = endereco.Logradouro,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(CadastroUsuarioViewModel formulario)
        {
            var ret = false;

            var usuario = new Usuario()
            {
                Nome = formulario.Nome,
                DataNascimento = DateTime.Parse(formulario.DataNascimento),
                Email = formulario.Email,
                CPF = formulario.CPF,
                Enderecos = new List<Endereco>
                {
                    new Endereco
                    {
                        CEP = formulario.CEP,
                        Logradouro = formulario.Logradouro,
                        Complemento = formulario.Complemento,
                        Bairro = formulario.Bairro,
                        Cidade = formulario.Cidade,
                        Estado = formulario.Estado,
                    }
                }
            };
            dbContext.Usuario.Add(usuario);

            if (await dbContext.SaveChangesAsync() > 0)
            {
                ret = true;
            }

            return Json(ret);
        }

        [HttpDelete]
        public async Task<IActionResult> Excluir(int id)
        {
            var ret = false;
            var usuario = dbContext.Usuario.Where(x => x.Id == id).FirstOrDefault();
            if (usuario != null)
            {
                dbContext.Usuario.Remove(usuario);
                await dbContext.SaveChangesAsync();
                ret = true;
            }

            return Json(ret);
        }
    }
}
