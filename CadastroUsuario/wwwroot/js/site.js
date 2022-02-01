var validarFormulario = true;

function validarCampoVazio(formulario) {
    var isValid = true;
    $('.error').remove();

    for (var i = 0; i < formulario.length; i++) {
        if (!validarValor($(formulario)[i])) {
            isValid = false;
        }
    }

    return isValid;
}

function validarValor(elemento) {
    var valor = elemento.val();
    var spanErro = `<span class="error" style="color : red"></span>`;
    elemento.after(spanErro);
    var errorElemnt = elemento.next();
    errorElemnt.text('');

    if (!valor || $.trim(valor) == '') {
        errorElemnt.html('Campo Obrigatório <br/>');
        return false;
    }

    return true;
}

function validarDadosFormulario() {
    var form = [$('#nome'),
    $('#data_nascimento'),
    $('#cpf'),
    $('#email'),
    $('#cep'),
    $('#logradouro'),
    $('#complemento'),
    $('#bairro'),
    $('#cidade'),
    $('#estado')
    ];
    var validar = validarCampoVazio(form)
    return validar;
}

function recuperarDadosFormulario() {
    var form = {
        nome: $.trim($('#nome').val()),
        dataNascimento: $.trim($('#data_nascimento').val()),
        cpf: $.trim($('#cpf').val()),
        email: $.trim($('#email').val()),
        cep: $.trim($('#cep').val()),
        logradouro: $.trim($('#logradouro').val()),
        complemento: $.trim($('#complemento').val()),
        bairro: $.trim($('#bairro').val()),
        cidade: $.trim($('#cidade').val()),
        estado: $.trim($('#estado').val())
    };

    return form;
}

function preencherEndereco(valor) {
    var cep = $(valor).val().replace(".", "").replace("-", "")
    var url = `https://viacep.com.br/ws/${cep}/json/ `;
    var request = new XMLHttpRequest()
    request.open("GET", url, false)
    request.send()
    if (JSON.parse(request.responseText).erro) {
        $('#cep').after('<span class="erroCep" style="color : red">CEP não encontrado...</span>')
    } else {
        var dados = JSON.parse(request.responseText);
        $('#logradouro').val(dados.logradouro);
        $('#complemento').val(dados.complemento);
        $('#bairro').val(dados.bairro);
        $('#cidade').val(dados.localidade);
        $('#estado').val(dados.uf);
    }
}

function validarEmail(email) {
    var texto = /\S+@\S+\.\S+/;
    return texto.test(email);
}

function cadastrarUsuario() {
    $('.erroEmail').remove();
    var validarFormulario = validarDadosFormulario();
    var formulario = recuperarDadosFormulario();
    if ($.trim($('#email').val()) != "") {
        var validado = validarEmail($.trim($('#email').val()))
        if (!validado) {
            $('#email').after('<span class="erroEmail" style="color : red">Email inválido</span>');
            validarFormulario = false;
            return false;
        }
    }
    if (validarFormulario) {
        $.ajax({
            type: "POST",
            url: UrlRegistro.CadastrarUsuario,
            data: { formulario: formulario },
            success: function (response) {
                window.location.href = UrlRegistro.RetornarHome;
            },
            error: function (erro) {

            }
        });
    }
}

function excluirUsuario(id) {
    var check = "Deseja excluir este usuário";
    if (confirm(check)) {
        $.ajax({
            type: "DELETE",
            url: UrlHome.ExcluirUsuario,
            data: { id: id },
            success: function (response) {
                window.location.reload();
            },
            error: function (erro) {

            }
        });
    }
}

function mascaraCEP(c, f) {
    setTimeout(function () {
        var valor = validarCEP($(c).val());
        if (valor != $(c).val()) {
            $(c).val(valor);
        }
    }, 1);
}

function validarCEP(valor) {
    var ret = valor.replace(/\D/g, "");
    if (ret.length > 4) {
        ret = ret.replace(/^(\d{2})(\d{3})/, "$1.$2-");
    }
    else if (ret.length > 1) {
        ret = ret.replace(/^(\d{2})/, "$1.");
    }

    return ret;
}

function mascaraCNPJ(c, f) {
    setTimeout(function () {
        var valor = validarCpf($(c).val());
        if (valor != $(c).val()) {
            $(c).val(valor);
        }
    }, 1);
}

function validarCpf(valor) {
    var ret = valor.replace(/\D/g, "");
    if (ret.length > 8) {
        ret = ret.replace(/^(\d{3})(\d{3})(\d{3})/, "$1.$2.$3-");
    }
    else if (ret.length > 5) {
        ret = ret.replace(/^(\d{3})(\d{3})/, "$1.$2.");
    }
    else if (ret.length > 2) {
        ret = ret.replace(/^(\d{3})/, "$1.");
    }

    return ret;
}


$(document)
    .on('click', '#btn_submit', function () {
        cadastrarUsuario();
    }).on('blur', '#cep', function () {
        $('.erroCep').remove();
        if ($(this).val().length < 10) {
            $('#cep').after('<span class="erroCep" style="color : red">CEP inválido</span><br/>')
        } else {
            preencherEndereco($(this));
        }
    }).on('blur', '#email', function () {
        $('.erroEmail').remove();
        if ($.trim($('#email').val()) != "") {
            var validado = validarEmail($.trim($('#email').val()))
            if (!validado) {
                $('#email').after('<span class="erroEmail" style="color : red">Email inválido</span><br/>');
            }
        }
    }).on('input', '#cep', function () {
        mascaraCEP($(this));
    }).on('input', '#cpf', function () {
        mascaraCNPJ($(this));
    }).on('click', '#excluir', function () {
        excluirUsuario($(this).attr('data-id'));
    });