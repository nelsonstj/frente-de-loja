(function() {
    var inicio = $("#DataInicio").kendoDatePicker({
        format: "dd/MM/yyyy"
    }).data("kendoDatePicker");

    var fim = $("#DataFim").kendoDatePicker({
        format: "dd/MM/yyyy"
    }).data("kendoDatePicker");

    inicio.max(fim.value());
    fim.min(inicio.value());

    criaSelect2("#TipoTabelaProtheus");
    criaSelect2("#StatusIntegracao");
})();

var kendoGridColumns = [
    { field: "NomeArquivo", title: "Nome do Arquivo" },
    { field: "StatusIntegracao", title: "Status Integração" },
    { field: "DataAtualizacao", title: "Data de atualização", format: "{0: yyyy-MM-dd HH:mm:ss}" },
    { template: $("#log-column").html(), width: 110 }
];

function filtraLogSuccess(res, test, lala) {
    if ($("#grid-integracao").data("kendoGrid")) {
        $("#grid-integracao").data("kendoGrid").destroy();
    }

    console.log('success');
    var processado = res.map(function (item) {
        item.DataAtualizacao = new Date(item.DataAtualizacao);
        return item;
    });

    criaKendoGrid("#grid-integracao", kendoGridColumns, processado);
}

function startChange() {
    var startDate = start.value();
    var endDate = end.value();

    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        end.min(startDate);
    } else if (endDate) {
        start.max(new Date(endDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}

function endChange() {
    var endDate = end.value();
    var startDate = start.value();

    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
        start.max(endDate);
    } else if (startDate) {
        end.min(new Date(startDate));
    } else {
        endDate = new Date();
        start.max(endDate);
        end.min(endDate);
    }
}

function abrirModalLogErro(id) {
    $("#erro").load("/LogCargaCatalogo/ObterTextoErro/", { id: id }, function (resp) {
        $("#erro").css("display", "flex");
    });

}