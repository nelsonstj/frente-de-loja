var buscaOrcamentos = '/Orcamento/ObterOrcamentos';
var buscaOrcamentosVencidos = '/Orcamento/ObterOrcamentosVencidosHoje';
var buscaOrcamentosProtheus = '/Orcamento/ObterOrcamentosProtheus';
var sincronizaOrcamentoProtheus = '/Orcamento/SincronizaOrcamentoProtheus';

function filtrarOrcamentos() {
    var tipo = $("#tipo-filtro").val();
    var termo = $("#termo-busca").val();
    var statusOrcamento = $("#status").val();
    visibilidadeGifCarregandoGrid($("#grid-orcamento"), true);
    visibilidadeGifCarregandoGrid($("#grid-orcamento-protheus"), true);

    $.ajax({
        type: "POST",
        url: buscaOrcamentos,
        data: { tipo: tipo, termo: termo, statusOrcamento: statusOrcamento }
    }).done(function (response) {
        $("#grid-orcamento").data("kendoGrid").dataSource.data(response);
        visibilidadeGifCarregandoGrid($("#grid-orcamento"), false);
    }).fail(function (response) {
        $("#grid-orcamento").data("kendoGrid").dataSource.data([]);
        criaAlertaMensagem(response.responseJSON);
        visibilidadeGifCarregandoGrid($("#grid-orcamento"), false);
    });

    $.ajax({
        type: "POST",
        url: buscaOrcamentosProtheus,
        data: { tipo: tipo, termo: termo, statusOrcamento: statusOrcamento }
    }).done(function (response) {
        $("#grid-orcamento-protheus").data("kendoGrid").dataSource.data(response);
        visibilidadeGifCarregandoGrid($("#grid-orcamento-protheus"), false);
    }).fail(function (response) {
        $("#grid-orcamento-protheus").data("kendoGrid").dataSource.data([]);
        criaAlertaMensagem(response.responseJSON);
        visibilidadeGifCarregandoGrid($("#grid-orcamento-protheus"), false);
    });
}

function visibilidadeGifCarregandoGrid(element, visibilidade) {
    kendo.ui.progress(element, visibilidade);
}

function formataDataProtheus(data) {
    if (data) {
        return data.substr(6, 2) + "/" + data.substr(4, 2) + "/" + data.substr(0, 4) + data.substr(8);
    } else {
        return "";
    }
}

function orcamentoVencido(status) {
    return status.indexOf("Em aberto vencido") > -1 ? "orcamento-vencido" : "";
}



function setVisibilidadeLoadingSyncProtheus(visibilidade) {
    if (visibilidade) {
        $("#aviso-sync").modal('show');
    } else {
        $("#aviso-sync").modal('hide');
    }
}

function sincronizaOrcamento(campoCodigo, idOrcamento) {
    if (campoCodigo) {
        setVisibilidadeLoadingSyncProtheus(true);
        $.ajax({
            type: "POST",
            url: sincronizaOrcamentoProtheus,
            data: { id: idOrcamento }
        }).done(function (response) {
            location.href = '/Orcamento/Negociacao/' + idOrcamento;
        }).fail(function (response) {
            criaAlertaMensagem(response.responseJSON);
            setVisibilidadeLoadingSyncProtheus(false);
        });
    } else {
        location.href = '/Orcamento/Negociacao/' + idOrcamento;
    }
}

function onFiltroTipoFiltroSelected(event) {
    switch (event.choice.id) {
        case "0":       
            $("#termo-busca").mask('0#', { maxlength: false });
            break;
        case "1":
            $("#termo-busca").mask('000000-00');
            break;
        case "2":
            $("#termo-busca").mask('AAA-9999');
            break;
        case "3":
            $("#termo-busca").mask('000.000.000-00');
            break;
        case "4":
            $("#termo-busca").mask('00.000.000/0000-00');
            break;
        case "5":
            $("#termo-busca").mask('AAAAAA-AA');
            break;      
        case "6":
        case "7":
            $("#termo-busca").unmask();
            break;
    }
}

function init() {
    criaSelect2({
        selector: "#status",
        minimumResultsForSearch: -1,
    });
    criaSelect2({
        selector: "#tipo-filtro",
        minimumResultsForSearch: -1,
    });

    $("#tipo-filtro").on("select2-selected", onFiltroTipoFiltroSelected);
    var busca = (location.search.slice(1).indexOf("vencidos") > -1) ? buscaOrcamentosVencidos : buscaOrcamentos;

    $('#termo-busca').keypress(function (e) {
        if (e.which == 13) {
            filtrarOrcamentos();
        }
    });

    $("#grid-orcamento").kendoGrid({
        dataSource: {
            transport: {
                read: busca
            },
            pageSize: 10
        },
        height: 475,
        sortable: true,
        pageable: {
            buttonCount: 5
        },
        columns: [{
            field: "Id",
            title: "ID",
            width: 50
        }, {
            field: "CampoCodigo",
            title: "Nº Protheus",
            width: 100
        }, {
            field: "NomeCliente",
            title: "Nome do Cliente"
        }, {
            field: "LojaDestino",
            title: "Loja destino"
        }, {
            field: "Vendedor",
            title: "Vendedor"
        }, {
            field: "DataCriacao",
            title: "Emissão",
            width: 80
        }, {
            field: "DataValidade",
            title: "Vencto.",
            template: kendo.template($("#template-data").html()),
            width: 80
        }, {
            field: "Status",
            title: "Status",
            template: kendo.template($("#template-status").html()),
            width: 120
        }, {
            title: "Opções",
            template: kendo.template($("#template-detalhes").html()),
            width: 163
        }]
    });

    $("#grid-orcamento-protheus").kendoGrid({
        dataSource: {
            transport: {
                read: buscaOrcamentosProtheus
            },
            pageSize: 10
        },
        height: 475,
        sortable: true,
        pageable: {
            buttonCount: 5
        },
        columns: [{
            field: "Id",
            title: "ID",
            width: 50
        }, {
            field: "CampoCodigo",
            title: "Nº Protheus",
            width: 100
        }, {
            field: "NomeCliente",
            title: "Nome do Cliente"
        }, {
            field: "LojaDestino",
            title: "Loja destino"
        }, {
            field: "Vendedor",
            title: "Vendedor"
        }, {
            field: "DataCriacao",
            title: "Emissão",
            template: kendo.template($("#template-data-emissao").html()),
            width: 80
        }, {
            field: "DataValidade",
            title: "Vencto.",
            template: kendo.template($("#template-data-validade-protheus").html()),
            width: 80
        }, {
            field: "Status",
            title: "Status",
            template: kendo.template($("#template-status").html()),
            width: 120
        }, {
            title: "Opções",
            template: kendo.template($("#template-detalhes-hist").html()),
            width: 163
        }]
    });
    var gridHead = $("#grid").getKendoGrid().thead;
    var cells = gridHead.find("th");
    cells.css("background-color", "#90EE90");
}