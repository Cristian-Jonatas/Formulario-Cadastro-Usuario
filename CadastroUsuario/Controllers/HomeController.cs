using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

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
            var usuario = dbContext.Usuario.Where(x => x.Id == idUsuario).FirstOrDefault();
            var endereco = dbContext.Endereco.Where(x => x.IdUsuario == idUsuario).FirstOrDefault();
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
            try
            {
                var usuario = new Usuario()
                {
                    Nome = formulario.Nome,
                    DataNascimento = DateTime.Parse(formulario.DataNascimento),
                    Email = formulario.Email,
                    CPF = formulario.CPF
                };
                await dbContext.Usuario.AddAsync(usuario);
                await dbContext.SaveChangesAsync();

                var endereco = new Endereco()
                {
                    CEP = formulario.CEP,
                    Logradouro = formulario.Logradouro,
                    Complemento = formulario.Complemento,
                    Bairro = formulario.Bairro,
                    Cidade = formulario.Cidade,
                    Estado = formulario.Estado,
                    IdUsuario = usuario.Id
                };

                await dbContext.Endereco.AddAsync(endereco);
                await dbContext.SaveChangesAsync();
                ret = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(ret);
        }

        [HttpDelete]
        public async Task<IActionResult> Excluir(int id)
        {
            var ret = false;
            try
            {
                var usuario = dbContext.Usuario.Where(x => x.Id == id).FirstOrDefault();
                if (usuario != null)
                {
                    dbContext.Usuario.Remove(usuario);
                    await dbContext.SaveChangesAsync();
                }
                ret = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(ret);
        }

    }
}
