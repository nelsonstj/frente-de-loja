"use strict";
var buscaAreaNegocio = '/Orcamento/ObterAreaNegocio';
var buscaVendedor = '/Orcamento/ObterVendedor';
//var buscaLojaDestino = '/Orcamento/ObterLojaDestino';
var buscaTransportadora = '/Orcamento/ObterTransportadora';
var areaNegocio;
var loja;

function iniciarOrcamentoSubmit() {
    if (validaEquipeVendas()) {
        setVisibilidadeLoading(true);
    } else {
        return false;
    }
}

var requestTermoEquipeVendas = function (term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        termoBusca: term
    };
};

function processaDescricao(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            text: obj.text || obj.Descricao,
            id: obj.id || obj.Id
        };
    });
    return processa(data, result.count, page);
};

function processaNomeId(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.IdConsultor,
            text: obj.text || obj.Nome
        };
    });
    return processa(data, result.count, page);
};

/*function processaLojaDestino(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.CampoCodigo,
            text: obj.text || obj.Descricao
        };
    });
    return processa(data, result.count, page);
};
*/
/*function processaCampoCodigo(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.Id,
            text: obj.text || obj.CampoCodigo
        };
    });
    return processa(data, result.count, page);
};
*/
function validaEquipeVendas() {
    var menssagem = '';
    var mandatorios = [
        { id: "Vendedor", text: "Vendedor" },
        { id: "AreaNegocio", text: "Área de negócio" },
        { id: "DataValidade", text: "Data de validade" }
    ]

    mandatorios.forEach(function (item) {
        if (!$("#" + item.id).val()) {
            menssagem += "O campo " + item.text + " é obrigatório. <br />";
        }
    });
/*
    if ($("#TipoOrcamento").val() === "1") { //telemarketing
        if (!$("#LojaDestino").val()) {
          menssagem += "O campo Loja destino é obrigatório."
        }
    }
*/
    if (menssagem) {
        criaAlertaMensagem(menssagem);
        return false;
    } else {
        return true;
    }
}

function initEquipeVendas() {
    var selects = [];
    if ($("#StatusSomenteLeitura").val() == "True") {
        setFormDisable("#equipe-vendas");
    }

    selects.push(criaSelect2(select2configFactory("#AreaNegocio", buscaAreaNegocio, requestTermoEquipeVendas, processaDescricao)));
    selects.push(criaSelect2(select2configFactory("#Vendedor", buscaVendedor, requestTermoController, processaNomeId)));
    //selects.push(criaSelect2(select2configFactory("#LojaDestino", buscaLojaDestino, requestTermoEquipeVendas, processaLojaDestino)));
    selects.push(criaSelect2(select2configFactory("#Transportadora", buscaTransportadora, requestTermoEquipeVendas, processaDescricao)));

    selectDefaultEvents(selects);

    $('#DataValidade').daterangepicker({
        "singleDatePicker": true,
        "minDate": new Date(),
        "drops": "up"
    });

    if ($("#AreaNegocioDescricao").val()) {
        $("#AreaNegocio").select2("data", { id: $("#AreaNegocio").val(), text: $("#AreaNegocioDescricao").val() });
    }

    if ($("#VendedorDescricao").val()) {
        $("#Vendedor").select2("data", { id: $("#Vendedor").val(), text: $("#VendedorDescricao").val() });
    }
/*
    if ($("#LojaDestinoDescricao").val()) {
        loja = { id: $("#LojaDestino").val(), text: $("#LojaDestinoDescricao").val() };
        $("#LojaDestino").select2("data", { id: $("#LojaDestino").val(), text: $("#LojaDestinoDescricao").val() });
    }
*/
    if ($("#TransportadoraDescricao").val()) {
        $("#Transportadora").select2("data", { id: $("#Transportadora").val(), text: $("#TransportadoraDescricao").val() });
    }
}