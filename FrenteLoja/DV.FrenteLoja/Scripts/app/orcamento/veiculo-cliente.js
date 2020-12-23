'use strict';

var buscaLojaDestinoPorId = '/Orcamento/BuscaLojaPorId';
var buscaLojaDestino = '/Orcamento/ObterLojaDestino';
var buscaConvenio = '/Orcamento/ObterConvenios';
var buscaConvenioPorId = '/Orcamento/BuscaConvenioPorId';
var buscaTabelaPreco = '/Orcamento/ObterTabelaPreco';
var buscaMarca = '/Orcamento/ObterMarcasVeiculos';
var buscaMarcaModelo = '/Orcamento/ObterModelosVeiculos';
var buscaMarcaModeloVersao = '/Orcamento/ObterVersaoVeiculos';
var buscaVersaoMotor = '/Orcamento/ObterMotorVeiculos';
var buscaAnos = '/Orcamento/ObterAnos';
var buscaNomeCliente = '/Orcamento/ObterNomeCliente';
var ObterVeiculoPorPlaca = '/Orcamento/ObterVeiculoPorPlaca';
var ObterClientePorTipo = '/Orcamento/ObterClientePorTipo';
var VerificaConveniosPorCliente = '/Orcamento/VerificaConveniosPorCliente';
var convenio;
var convenioCerto = true;
var tabPreco;
var cliente = { text: "" };
var loja = { text: "" };
var marca = { text: "" };
var modelo = { text: "" };
var versao = { text: "" };
var motor = { text: "" };
var ano = { text: "" };
var tipoConsulta;
var fetch;
var placaAnterior = "";
var clienteAnterior = "";

function requestTermoConvenio(term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        termoBusca: term,
        cliente: cliente.id || cliente.text,
        loja: loja.id || loja.text
    };
};

function requestTermoModelo(term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        marca: marca.id || marca.text,
        versao: versao.id || versao.text,
        motor: motor.id || motor.text,
        ano: ano.id || ano.text,
        buscaModelo: term
    };
};

function requestTermoVersao(term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        marca: marca.id || marca.text,
        modelo: modelo.id || modelo.text,
        motor: motor.id || motor.text,
        ano: ano.id || ano.text,
        buscaVersao: term,
    };
};

function requestTermoMotor(term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        marca: marca.id || marca.text,
        modelo: modelo.id || modelo.text,
        versao: versao.id || versao.text,
        ano: ano.id || ano.text,
        buscaMotor: term
    };
};

function requestTermoAnos(term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        marca: marca.id || marca.text,
        modelo: modelo.id || modelo.text,
        versao: versao.id || versao.text,
        motor: motor.id || motor.text,
        buscaAno: term
    };
};

function processa(data, count, page) {
    var more = page * pageSize < count;
    return { results: data, more: more };
};

function processaResult(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            text: obj.text || obj.Descricao,
            id: obj.text || obj.Descricao
        };
    });
    return processa(data, result.count, page);
};

function processaLojaDestino(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.CampoCodigo,
            text: obj.text || obj.Descricao
        };
    });
    return processa(data, result.count, page);
};

function processaDescricaoConvenio(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            text: obj.text || obj.IdConvenio + '-' + obj.Descricao,
            id: obj.id || obj.IdConvenio
        };
    });
    return processa(data, result.count, page);
};

function processaNomeCliente(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.IdCliente,
            text: obj.text || obj.Nome
        };
    });
    return processa(data, result.count, page);
}

function processaCampoCodigo(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.Id,
            text: obj.text || obj.CampoCodigo
        };
    });
    return processa(data, result.count, page);
};

function setVisibilidadeCampos(tipoSelecionado) {
    tipoConsulta = tipoSelecionado;
    switch (tipoSelecionado) {
        case "nome":
            setVisibilidade(['nome', 'CodigoConsulta'], false);
            setVisibilidade(['SelectNome', 'CpfCnpj', 'codigo'], true);
            $("#consulta-nome").select2("val", "");
            limparCamposCliente();
            break;
        case "cpf":
            setVisibilidade(['CpfCnpj', 'SelectNome'], false);
            setVisibilidade(['nome', 'codigo', 'CodigoConsulta'], true);
            $("#codigo-consulta").mask('000.000.000-000');
            $("#consulta-nome").select2("val", "");
            limparCamposCliente();
            break;
        case "cnpj":
            setVisibilidade(['CpfCnpj', 'SelectNome'], false);
            setVisibilidade(['nome', 'codigo', 'CodigoConsulta'], true);
            $("#codigo-consulta").mask('00.000.000/0000-00');
            $("#consulta-nome").select2("val", "");
            limparCamposCliente();            
            break;
        case "codigo":
            setVisibilidade(['SelectNome', 'codigo'], false);
            setVisibilidade(['nome', 'CodigoConsulta', 'CpfCnpj'], true);
            $("#codigo-consulta").mask('AAAAAA-00');
            $("#consulta-nome").select2("val", "");
            limparCamposCliente();            
            break;
    }
}

function setVisibilidade(campos, visibilidade) {
    campos.map(function (campo) {
        if (visibilidade) {
            $("#" + campo).show();
        } else {
            $("#" + campo).hide();
        }
    });
}

function preencheCamposVeiculo(informacoes) {
    marca = { id: informacoes.Marca, text: informacoes.Marca };
    modelo = { id: informacoes.Modelo, text: informacoes.Modelo };
    versao = { id: informacoes.Versao, text: informacoes.Versao };
    motor = { id: informacoes.Motor, text: informacoes.Motor };
    ano = { id: informacoes.Ano, text: informacoes.Ano };

    $("#PlacaVeiculo").val(informacoes.Placa ? informacoes.Placa : $("#PlacaVeiculo").val());

    if (informacoes.Origem) {
        $("#DadosVeiculoOrigem").text(informacoes.Origem)
        setVisibilidade(['DadosVeiculoOrigem'], true);
    } else {
        $("#DadosVeiculoOrigem").text("")
        setVisibilidade(['DadosVeiculoOrigem'], false);
    }
    if (informacoes.Marca) {
        $("#MarcaVeiculo").select2("data", { id: informacoes.Marca, text: informacoes.Marca })
        $("#SinespMarcaVeiculo").text("")
        setVisibilidade(['SinespMarcaVeiculo'], false);
    } else {
        $("#MarcaVeiculo").select2("val", "")
        $("#SinespMarcaVeiculo").text(informacoes.SinespMarca)
        setVisibilidade(['SinespMarcaVeiculo'], true);
    }
    $("#MarcaVeiculoDescricao").val(informacoes.Marca ? informacoes.Marca : $("#MarcaVeiculo").val());

    if (informacoes.Modelo) {
        $("#ModeloVeiculo").select2("data", { id: informacoes.Modelo, text: informacoes.Modelo })
        $("#SinespModeloVeiculo").text("")
        setVisibilidade(['SinespModeloVeiculo'], false);
    } else {
        $("#ModeloVeiculo").select2("val", "")
        $("#SinespModeloVeiculo").text(informacoes.SinespModelo)
        setVisibilidade(['SinespModeloVeiculo'], true);
    }
    $("#ModeloVeiculoDescricao").val(informacoes.Modelo ? informacoes.Modelo : $("#ModeloVeiculo").val());

    if (informacoes.Versao) {
        $("#VersaoVeiculo").select2("data", { id: informacoes.Versao, text: informacoes.Versao })
        $("#SinespVersaoVeiculo").text("")
        setVisibilidade(['SinespVersaoVeiculo'], false);
    } else {
        $("#VersaoVeiculo").select2("val", "")
        $("#SinespVersaoVeiculo").text(informacoes.SinespVersao)
        setVisibilidade(['SinespVersaoVeiculo'], true);
    }
    $("#VersaoVeiculoDescricao").val(informacoes.Versao ? informacoes.Versao : $("#VersaoVeiculo").val());

    if (informacoes.Motor) {
        $("#VersaoMotor").select2("data", { id: informacoes.Motor, text: informacoes.Motor })
        $("#SinespMotorVeiculo").text("")
        setVisibilidade(['SinespMotorVeiculo'], false);
    } else {
        $("#VersaoMotor").select2("val", "");
        $("#SinespMotorVeiculo").text(informacoes.SinespMotor)
        setVisibilidade(['SinespMotorVeiculo'], true);
    }
    $("#VersaoMotorDescricao").val(informacoes.VersaoMotor ? informacoes.VersaoMotor : $("#VersaoMotor").val());

    if (informacoes.Ano) {
        $("#AnoVeiculo").select2("data", { id: informacoes.Ano, text: informacoes.Ano })
        $("#SinespAnoModeloVeiculo").text("")
        setVisibilidade(['SinespAnoModeloVeiculo'], false);
    } else {
        $("#AnoVeiculo").select2("val", "")
        $("#SinespAnoModeloVeiculo").text(informacoes.SinespAnoModelo)
        setVisibilidade(['SinespAnoModeloVeiculo'], true);
    }
    $("#AnoDescricao").val(informacoes.Ano ? informacoes.Ano : $("#AnoVeiculo").val());
}

function limparCamposCliente() {
    $("#IdCliente").val("");
    cliente = { id: $("#IdCliente").val(), text: $("#IdCliente").val() };
    $('#codigo-consulta').val("");
    $('#LojaCliente').val("")
    loja = { id: $("#LojaCliente").val(), text: $("#LojaCliente").val() };
    $("#CodigoCliente").val("")
    $("#CPFCNPJCliente").val("")
    $("#NomeCliente").val("")
    $("#ClassificacaoCliente").val("")
    $("#ScoreCliente").val("")
    $("#EmailCliente").val("")
    $("#CelularCliente").val("")
    $("#TelefoneComercialCliente").val("")
    $("#TelefoneCliente").val("")
}

function preencheCamposCliente(informacoes) {
    $("#tipo-consulta-cliente").val("codigo").trigger('change.select2');
    $("#consulta-nome").select2("val", "");
    $("#IdCliente").val(informacoes.IdCliente);
    $('#codigo-consulta').val(informacoes.CampoCodigo + informacoes.Loja).trigger('keyup');
    $('#LojaCliente').val(informacoes.Loja)
    $("#CodigoCliente").val(informacoes.CampoCodigo);
    if (informacoes.CNPJCPF.length > 11) {
        $("#CPFCNPJCliente").mask('00.000.000/0000-00');
    } else {
        $("#CPFCNPJCliente").mask('000.000.000-00');
    }
    $("#CPFCNPJCliente").val(informacoes.CNPJCPF).trigger('keyup');
    $("#NomeCliente").val(informacoes.Nome);
    $("#ClassificacaoCliente").val(informacoes.ClassificacaoCliente);
    $("#ScoreCliente").val(informacoes.Score);
    $("#EmailCliente").val(informacoes.Email);
    $("#CelularCliente").val(informacoes.TelefoneCelular).trigger('keyup');
    $("#TelefoneComercialCliente").val(informacoes.TelefoneComercial).trigger('keyup');
    $("#TelefoneCliente").val(informacoes.Telefone).trigger('keyup');
    cliente = { id: $("#IdCliente").val(), text: $("#IdCliente").val() };
    loja = { id: $("#LojaCliente").val(), text: $("#LojaCliente").val() };
}

function consultaVeiculoPorPlaca(placa, abreModalCLiente) {
    setVisibilidadeLoading(true);
    enableCamposVeiculo();
    $.ajax({
        type: "POST",
        url: ObterVeiculoPorPlaca,
        data: { placa: placa }
    }).done(function (response) {
        preencheCamposVeiculo(response);
        setVisibilidadeLoading(false);
        var codigoProduto = $("#codigo-consulta").val();
        if (!codigoProduto && abreModalCLiente && response.ClienteId) {
            abrirModalClienteVinculado(response.ClienteId);
        } else {
            $("#QuilometragemVeiculo").focus();
        }
    }).fail(function (err) {
        $("#MarcaVeiculo").select2("val", "");
        $("#MarcaVeiculoDescricao").val("");
        $("#ModeloVeiculo").select2("val", "");
        $("#ModeloVeiculoDescricao").val("");
        $("#VersaoVeiculo").select2("val", "");
        $("#VersaoVeiculoDescricao").val("");
        $("#VersaoMotor").select2("val", "");
        $("#VersaoMotorDescricao").val("");
        $("#AnoVeiculo").select2("val", "");
        $("#AnoDescricao").select2("val", "");
        $("#QuilometragemVeiculo").val("");
        criaAlertaMensagem(err.responseJSON);
        setVisibilidadeLoading(false);
    });
}

function consultaClientePorTipo(tipo, codigo, abreModalVeiculo) {
    setVisibilidadeLoading(true);
    $.ajax({
        type: "POST",
        url: ObterClientePorTipo,
        data: { tipoConsulta: tipo, codigo: codigo }
    }).done(function (response, teste) {
        preencheCamposCliente(response);
        var placa = $("#PlacaVeiculo").val();
        if (!placa && abreModalVeiculo && response.UltimoVeiculoUtilizado) {
            if (!response.UltimoVeiculoUtilizado.Placa) {
                setVisibilidadeLoading(false);
            } else {
                AbrirModalVeiculoVinculado(response.UltimoVeiculoUtilizado.Placa);
            }
        } else {
            $("#QuilometragemVeiculo").focus();
            setVisibilidadeLoading(false);
        }
        if ($("#TipoOrcamento").val() === "2") {
            disableCamposVeiculo();
        } else {
            enableCamposVeiculo();
        }
        var convenio = $("#Convenio").val();
        verificaConvenioPorCliente(convenio, response.IdCliente, response.Loja);
    }).fail(function (err) {
        criaAlertaMensagem(err.responseJSON);
        setVisibilidadeLoading(false);
        limparCamposCliente();
        enableCamposVeiculo();
    });
}

function verificaConvenioPorCliente(convenio, cliente, loja) {
    $.ajax({
        type: "POST",
        url: VerificaConveniosPorCliente,
        data: { convenio: convenio, cliente: cliente, loja: loja }
    }).done(function (response) {
        convenioCerto = true;
        if (response == false) {
            convenioCerto = false;
            criaAlertaMensagem("É necessário trocar o convênio para o cliente selecionado!");
        }
    }).fail(function (err) {
        criaAlertaMensagem(err.responseJSON);
    });
}

function setMascarasCampos() {
    var optionsTelefone = {
        translation: { 'Z': { pattern: /[0-9]/, optional: true } },
        onKeyPress: function (tel, ev, el, op) {
           // console.log(tel.length)
            var masks = ['(00) 0000-0000Z', '(00) 00000-0000'],
                mask = (tel.length > 15) ? masks[1] : masks[0];
            el.mask(mask, op);
        }
    }

    $('#PlacaVeiculo').mask('AAA 9A99');
    $('#PlacaVeiculo').keyup(function (e) {
        var str = $(this).val();
        $("#PlacaVeiculo").val(str.toUpperCase());
        if (e.target.value) {
            //disableCamposVeiculo();
        } else {
            enableCamposVeiculo();
        }
    });
    $('#PlacaVeiculo').focus(function (e) {
        if (e.target.value) {
            //disableCamposVeiculo();
            placaAnterior = $('#PlacaVeiculo').val();
        }
    });

    $('#codigo-consulta').focus(function (e) {
        clienteAnterior = $('#codigo-consulta').val();
    });

    $('#codigo-consulta').keyup(function (e) {
        if (e.target.value) {
            disableCamposVeiculo();
        } else {
            enableCamposVeiculo();
        }
    });


    if ($("#CPFCNPJCliente").val().length > 11) {
        $("#CPFCNPJCliente").mask('00.000.000/0000-00');
    } else {
        $("#CPFCNPJCliente").mask('000.000.000-00');
    }

    $("#codigo-consulta").mask('000.000.000-00');
    $("#CelularCliente").mask('(00) Z0000-0000', optionsTelefone);
    $("#TelefoneComercialCliente").mask('(00) Z0000-0000', optionsTelefone);
    $("#TelefoneCliente").mask('(00) Z0000-0000', optionsTelefone);
}

function abrirModalClienteVinculado(idCLiente) {
    setVisibilidadeLoading(true);
    $("#modal").load("/Orcamento/AbrirModalClienteVinculado/", {
        idCLiente: idCLiente
    }, function (content, status, response) {
        if (status === "success") {
            $("#modal").modal();
        } else {
            criaAlertaMensagem(JSON.parse(content));
        }
        setVisibilidadeLoading(false);
    });
}

function AbrirModalVeiculoVinculado(placaVeiculo) {
    $("#modal").load("/Orcamento/AbrirModalVeiculoVinculado/", {
        placa: placaVeiculo
    }, function (content, status, response) {
        if (status === "success") {
            $("#modal").modal();
        } else {
            criaAlertaMensagem(JSON.parse(content));
        }
        setVisibilidadeLoading(false);
    })
}

function setBlurEvents() {
    $('#PlacaVeiculo').blur(function (event) {
        if (event.target.value) {
            if (event.target.value != placaAnterior) {
                consultaVeiculoPorPlaca(event.target.value, true)
            } else {
                enableCamposVeiculo();
            }
        }
    });

    $('#codigo-consulta').blur(function (event) {
        if (event.target.value) {
            if (event.target.value != clienteAnterior) {
                var tipoConsulta = $("#tipo-consulta-cliente").val();
                var codigo = $("#codigo-consulta").val();
                consultaClientePorTipo(tipoConsulta, codigo, true);
            } else {
                enableCamposVeiculo();
            }
        } else {
            limparCamposCliente();
        }
    });
};

function setSelectedEvents() {
    $("#LojaDestino").on('select2-selected', function (event) {
        loja = event.choice;
        $("#LojaDestino").val(event.choice.id);
        $("#LojaDestinoDescricao").val(event.choice.text);
        buscaLojaId(event.choice.id);
    });

    $("#Convenio").on('select2-selected', function (event) {
        convenio = event.choice;
        $("#Convenio").val(event.choice.id);
        $("#ConvenioDescricao").val(event.choice.text);
    });

    $("#TabelaPreco").on('select2-selected', function (event) {
        tabPreco = event.choice;
        $("#TabelaPreco").val(event.choice.id);
        $("#TabelaPrecoDescricao").val(event.choice.text);
    });

    $("#MarcaVeiculo").on('focus', '.select2-selection--single', function (e) {
        console.log('focus');
    });

    $("#MarcaVeiculo").on("select2-selected", function (event) {
        marca = event.choice;
        $("#MarcaVeiculoDescricao").val(event.choice.text);
        $("#ModeloVeiculo").select2("val", "");
        $("#VersaoVeiculo").select2("val", "");
        $("#VersaoMotor").select2("val", "");
        setVisibilidade(['SinespMarcaVeiculo'], false);
    });

    $("#MarcaVeiculo").on("select2-clearing", function (event) {
        marca = '';
        $("#MarcaVeiculoDescricao").val("");
        $("#ModeloVeiculo").select2("val", "");
        $("#VersaoVeiculo").select2("val", "");
        $("#VersaoMotor").select2("val", "");
        setVisibilidade(['SinespMarcaVeiculo'], false);
    });

    $("#ModeloVeiculo").on("select2-selected", function (event) {
        modelo = event.choice;
        $("#ModeloVeiculoDescricao").val(event.choice.text);
        setVisibilidade(['SinespModeloVeiculo'], false);
    });

    $("#ModeloVeiculo").on("select2-clearing", function (event) {
        modelo = '';
        $("#ModeloVeiculoDescricao").val("");
        $("#VersaoVeiculo").select2("val", "");
        $("#VersaoMotor").select2("val", "");
        setVisibilidade(['SinespModeloVeiculo'], false);
    });

    $("#VersaoVeiculo").on("select2-selected", function (event) {
        versao = event.choice;
        $("#VersaoVeiculoDescricao").val(event.choice.text);
        setVisibilidade(['SinespVersaoVeiculo'], false);
    });

    $("#VersaoVeiculo").on("select2-clearing", function (event) {
        versao = '';
        $("#VersaoVeiculoDescricao").val("");
        $("#VersaoMotor").select2("val", "");
        setVisibilidade(['SinespVersaoVeiculo'], false);
    });

    $("#VersaoMotor").on("select2-selected", function (event) {
        motor = event.choice;
        $("#VersaoMotorDescricao").val(event.choice.text);
        setVisibilidade(['SinespMotorVeiculo'], false);
    });

    $("#VersaoMotor").on("select2-clearing", function (event) {
        motor = '';
        $("#VersaoMotorDescricao").val("");
        setVisibilidade(['SinespMotorVeiculo'], false);
    });

    $("#AnoVeiculo").on("select2-selected", function (event) {
        ano = event.choice;
        $("#AnoDescricao").val(event.choice.text);
        setVisibilidade(['SinespAnoModeloVeiculo'], false);
    });

    $("#AnoVeiculo").on("select2-clearing", function (event) {
        ano = '';
        $("#AnoDescricao").val("");
        setVisibilidade(['SinespAnoModeloVeiculo'], false);
    });

    $("#consulta-nome").on("select2-selected", function (event) {
        consultaClientePorTipo("id", event.choice.id, true)
    });

    $("#tipo-consulta-cliente").on("change.select2", function (event) {
        $("#codigo-consulta").val('');
        setVisibilidadeCampos($("#tipo-consulta-cliente").val());
    });

    $("#TipoOrcamento").on("change.select2", function (event) {
        if ($("#TipoOrcamento").val() === "2") {
            disableCamposVeiculo();
        } else {
            enableCamposVeiculo();
        }
    });
}

function buscaLojaId(idLoja) {
    setVisibilidadeLoading(true);
    $.ajax({
        type: "POST",
        url: buscaLojaDestinoPorId,
        data: { idLoja: idLoja }
    }).done(function (response) {
        $("#LojaDestino").val(response.lojaDestino);
        $("#LojaDestinoDescricao").val(response.lojaDestinoDescricao);
        loja = { id: $("#LojaDestino").val(), text: $("#LojaDestinoDescricao").val() };
        $("#IdCliente").val(response.convenio.IdCliente);
        cliente = { id: $("#IdCliente").val(), text: $("#IdCliente").val() };
        buscaConvenioId(response.convenio.IdConvenio, response.convenio.IdCliente);
        $("#Convenio").val(response.convenio.IdConvenio);
        $("#ConvenioDescricao").val(response.convenio.IdConvenio + " - " + response.convenio.Descricao);
        $("#Convenio").select2("data", { id: $("#Convenio").val(), text: $("#ConvenioDescricao").val() });
    }).fail(function (err) {
        $("#LojaDestino").select2("val", "");
        $("#LojaDestinoDescricao").val("");
        criaAlertaMensagem(err.responseJSON);
        setVisibilidadeLoading(false);
    });
}

function buscaConvenioId(idConvenio, idCliente) {
    setVisibilidadeLoading(true);
    $.ajax({
        type: "POST",
        url: buscaConvenioPorId,
        data: { idConvenio: idConvenio, idCliente: idCliente }
    }).done(function (response) {
        $("#InformacaoConvenio").val(response.Observacoes);
        setTabelaPrecoEnable(response.TrocaTabelaPreco);
        preencheTabelaPreco(response.TabelaPreco);
        convenioCerto = true;
        setVisibilidadeLoading(false);
    }).fail(function (err) {
        $("#Convenio").select2("val", "");
        $("#ConvenioDescricao").val("");
        $("#InformacaoConvenio").val("");
        criaAlertaMensagem(err.responseJSON);
        setVisibilidadeLoading(false);
    });
}

function setTabelaPrecoEnable(trocaTabelaPreco) {
    if (trocaTabelaPreco) {
        $("#TabelaPreco").select2('enable');
    } else {
        $("#TabelaPreco").select2('disable');
    }
}

function preencheTabelaPreco(tabelaPreco) {
    $("#TabelaPreco").select2("data", { id: tabelaPreco.CampoCodigo, text: tabelaPreco.CampoCodigo });
    $("#TabelaPrecoDescricao").val(tabelaPreco.CampoCodigo);
}

function validaVeiculoCliente() {
    var menssagem = '';
    if ($("#TipoOrcamento").val() === "0" || $("#TipoOrcamento").val() === "1") { // 0 loja, 1 telemarketing
        var mandatorios = [
            { id: "Convenio", text: "Convênio" },
            { id: "TabelaPreco", text: "Tabela de preço" },
            { id: "PlacaVeiculo", text: "Placa" },
            { id: "MarcaVeiculo", text: "Marca" },
            { id: "ModeloVeiculo", text: "Modelo" },
            { id: "VersaoVeiculo", text: "Versao" },
            { id: "VersaoMotor", text: "Motor" },
            { id: "AnoVeiculo", text: "Ano" },
            { id: "QuilometragemVeiculo", text: "KM Rodados" },
            { id: "IdCliente", text: "Cliente" }
        ]
        mandatorios.forEach(function (item) {
            if (!$("#" + item.id).val()) {
                menssagem += "O campo " + item.text + " é obrigatório. <br />";
            } else {
                if (item.id == "Convenio" && convenioCerto == false) {
                    menssagem += "É necessário trocar o convênio para o cliente selecionado! <br />";
                }
                if (item.id == "PlacaVeiculo" && $("#PlacaVeiculo").val().length < 8) {
                    menssagem += "O campo Placa é obrigatório. <br />";
                }
                if (item.id == "QuilometragemVeiculo" && $("#QuilometragemVeiculo").val() <= "0") {
                    menssagem += "O campo KM Rodados precisa ser maior que 0. <br />";
                }
            }
        });
        if ($("#TipoOrcamento").val() === "1") { //telemarketing
            if (!$("#LojaDestino").val()) {
                menssagem += "O campo Loja destino é obrigatório."
            }
        }
    } else {
        if (!$("#IdCliente").val()) {
            menssagem = $("#IdCliente").data('val-required');
        }
    }

    if (menssagem) {
        criaAlertaMensagem(menssagem);
        return false;
    } else {
        document.querySelector("#TabelaPreco").disabled = false;
        return true;
    }
}

function botaoVoltar() {
    $("#Voltar").val("True");
}

function setClearingVeiculo() {
    marca = '';
    modelo = '';
    versao = '';
    motor = '';
    ano = '';
    $("#MarcaVeiculoDescricao").val('');
    $("#ModeloVeiculoDescricao").val('');
    $("#VersaoVeiculoDescricao").val('');
    $("#VersaoMotorDescricao").val('');
    $("#AnoDescricao").val('');
    $("#MarcaVeiculo").select2("val", "");
    $("#ModeloVeiculo").select2("val", "");
    $("#VersaoVeiculo").select2("val", "");
    $("#VersaoMotor").select2("val", "");
    $("#AnoVeiculo").select2("val", "");
}

function disableCamposOpcoes() {
    $("#TipoOrcamento").prop("disabled", "disabled");
    $("#tipo-consulta-cliente").select2("enable", false);
}

function enableCamposOpcoes() {
    $("#TipoOrcamento").removeAttr("disabled");
    $("#tipo-consulta-cliente").select2("enable", true);
}

function disableCamposVeiculo() {
    $("#PlacaVeiculo").prop("disabled", "disabled");
    $("#MarcaVeiculo").select2("enable", false);
    $("#ModeloVeiculo").select2("enable", false);
    $("#VersaoVeiculo").select2("enable", false);
    $("#VersaoMotor").select2("enable", false);
    $("#AnoVeiculo").select2("enable", false);
    $("#QuilometragemVeiculo").prop("disabled", "disabled");
}

function enableCamposVeiculo() {
    $("#PlacaVeiculo").removeAttr("disabled");
    $("#MarcaVeiculo").select2("enable", true);
    $("#ModeloVeiculo").select2("enable", true);
    $("#VersaoVeiculo").select2("enable", true);
    $("#VersaoMotor").select2("enable", true);
    $("#AnoVeiculo").select2("enable", true);
    $("#QuilometragemVeiculo").removeAttr("disabled");
}

function initOrcamento() {
    var selects = [];

    criaSelect2({
        selector: "#tipo-consulta-cliente",
        minimumResultsForSearch: -1,
    });

    selects.push(criaSelect2(select2configFactory("#LojaDestino", buscaLojaDestino, requestTermoController, processaLojaDestino)));
    selects.push(criaSelect2(select2configFactory("#Convenio", buscaConvenio, requestTermoConvenio, processaDescricaoConvenio)));
    selects.push(criaSelect2(select2configFactory("#TabelaPreco", buscaTabelaPreco, requestTermoController, processaCampoCodigo)));
    selects.push(criaSelect2(select2configFactory("#MarcaVeiculo", buscaMarca, requestTermoController, processaResult)));
    selects.push(criaSelect2(select2configFactory("#ModeloVeiculo", buscaMarcaModelo, requestTermoModelo, processaResult)));
    selects.push(criaSelect2(select2configFactory("#VersaoVeiculo", buscaMarcaModeloVersao, requestTermoVersao, processaResult)));
    selects.push(criaSelect2(select2configFactory("#VersaoMotor", buscaVersaoMotor, requestTermoMotor, processaResult)));
    selects.push(criaSelect2(select2configFactory("#AnoVeiculo", buscaAnos, requestTermoAnos, processaResult)));
    criaSelect2(select2configFactory("#consulta-nome", buscaNomeCliente, requestTermoController, processaNomeCliente, 0, 5));

    if ($("#LojaDestinoDescricao").val()) {
        loja = { id: $("#LojaDestino").val(), text: $("#LojaDestinoDescricao").val() };
        $("#LojaDestino").select2("data", { id: $("#LojaDestino").val(), text: $("#LojaDestinoDescricao").val() });
    }

    if ($("#ConvenioDescricao").val()) {
        $("#Convenio").select2("data", { id: $("#Convenio").val(), text: $("#ConvenioDescricao").val() });
        convenio = { id: $("#ConvenioDescricao").val(), text: $("#ConvenioDescricao").val() };
        if ($("#TrocaTabelaPreco").val()) {
            var trocaTabela = $("#TrocaTabelaPreco").val() == "True" ? true : false;
            setTabelaPrecoEnable(trocaTabela);
        }
    }

    if ($("#TabelaPreco").val()) {
        $("#TabelaPreco").select2("data", { id: $("#TabelaPreco").val(), text: $("#TabelaPreco").val() });
        tabPreco = { id: $("#TabelaPreco").val(), text: $("#TabelaPreco").val() };
    }
    if ($("#MarcaVeiculoDescricao").val()) {
        $("#MarcaVeiculo").select2("data", { id: $("#MarcaVeiculoDescricao").val(), text: $("#MarcaVeiculoDescricao").val() });
        marca = { id: $("#MarcaVeiculoDescricao").val(), text: $("#MarcaVeiculoDescricao").val() };
    }
    if ($("#ModeloVeiculoDescricao").val()) {
        $("#ModeloVeiculo").select2("data", { id: $("#ModeloVeiculoDescricao").val(), text: $("#ModeloVeiculoDescricao").val() });
        modelo = { id: $("#ModeloVeiculoDescricao").val(), text: $("#ModeloVeiculoDescricao").val() };
    }
    if ($("#VersaoVeiculoDescricao").val()) {
        $("#VersaoVeiculo").select2("data", { id: $("#VersaoVeiculoDescricao").val(), text: $("#VersaoVeiculoDescricao").val() });
        versao = { id: $("#VersaoVeiculoDescricao").val(), text: $("#VersaoVeiculoDescricao").val() };
    }
    if ($("#VersaoMotorDescricao").val()) {
        $("#VersaoMotor").select2("data", { id: $("#VersaoMotorDescricao").val(), text: $("#VersaoMotorDescricao").val() });
        motor = { id: $("#VersaoMotorDescricao").val(), text: $("#VersaoMotorDescricao").val() };
    }
    if ($("#AnoDescricao").val()) {
        $("#AnoVeiculo").select2("data", { id: $("#AnoDescricao").val(), text: $("#AnoDescricao").val() });
        ano = { id: $("#AnoDescricao").val(), text: $("#AnoDescricao").val() };
    }

    setMascarasCampos();
    setSelectedEvents();
    setBlurEvents();
    selectDefaultEvents(selects);

    $("#Convenio").on("select2-selected", function (event) {
        buscaConvenioId(event.choice.id, $("#IdCliente").val());
    });

    if ($("#CodigoCliente").val()) {
        $("#tipo-consulta-cliente").select2("val", "codigo");
        setVisibilidade(['SelectNome', 'codigo'], false);
        setVisibilidade(['nome', 'CodigoConsulta', 'CpfCnpj'], true);
        $("#codigo-consulta").mask('AAAAAA-00');
        if ($("#LojaCliente").val()) {
            $('#codigo-consulta').val($("#CodigoCliente").val() + "-" + $("#LojaCliente").val()).trigger('keyup');
            loja = { id: $("#LojaCliente").val(), text: $("#LojaCliente").val() };
        } else {
            $('#codigo-consulta').val($("#CodigoCliente").val()).trigger('keyup');
        }
        cliente = { id: $("#IdCliente").val(), text: $("#IdCliente").val() };
        enableCamposVeiculo();
    }

    if ($("#OrcamentoProdutoCount").val() > "0") {
        var prod = $("#OrcamentoProdutoCount").val();
        $("#Convenio").select2("enable", false);
        document.querySelector("#Convenio").disabled = true;
        $("#TabelaPreco").select2("enable", false);
        document.querySelector("#TabelaPreco").disabled = true;
    }

    setVisibilidade(['divLojaDestino'], false);
    setVisibilidade(['divSpaceLojaDestino2'], false);
    setVisibilidade(['divSpaceLojaDestino5'], true);
    if ($("#TipoOrcamento").val() === "2") {
        setClearingVeiculo();
        disableCamposVeiculo();
    } else {
        enableCamposVeiculo();
    }
    if ($("#TipoOrcamento").val() === "1") {
        setVisibilidade(['divLojaDestino'], true);
        setVisibilidade(['divSpaceLojaDestino2'], true);
        setVisibilidade(['divSpaceLojaDestino5'], false);
    }

    if ($("#StatusSomenteLeitura").val() == "True") {
        setFormDisable("#veiculo-cliente");
        disableCamposVeiculo();
        disableCamposOpcoes();
    }
};