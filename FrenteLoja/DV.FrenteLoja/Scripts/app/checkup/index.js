var ENDPOINT = {
    VENDEDOR: '/Checkup/ObterVendedor',
    TECNICO: '/Checkup/ObterTecnico',
    BUSCA: '/Checkup/ObterListaCheckupsPorTipo',
    SEARCH: '/Checkup/Search'
};

var selectedMap = new Map();

function search() {
    var submit = document.getElementById('submit');
    submit.click();
}

function buildFakeRadioButtons() {
    var checkList = document.querySelectorAll('.checkList');


    checkList.forEach(item => {
        var spanIcon = item.querySelector('span');
        var radio = item.querySelector('input[type=radio]');

        item.radioBtt = radio;
        spanIcon.radioBtt = radio;
        spanIcon.label = item;

        if (item.getAttribute('selected') == "true") {
            selectedMap.set(item.radioBtt.name, item);
            item.radioBtt.checked = true;
        }


        spanIcon.onclick = clickEvent;
    });
}

function buildFakeCheckBox() {

    var checkMark = document.querySelectorAll('.checkMark');
    checkMark.forEach(item => {
        var spanIcon = item.querySelector('span');
        var checkBox = item.querySelector('input[type=hidden]');

        item.checkBox = checkBox;
        spanIcon.checkBox = checkBox;
        spanIcon.label = item;
        spanIcon.onclick = clickEventCheckBox;
    });
}

function clickEventCheckBox(event) {
    var element = event.target;

    if (element.label.getAttribute('selected') == "true") {
        element.label.setAttribute("selected", false);
        element.checkBox.value = false;
    }
    else {
        element.label.setAttribute("selected", true);
        element.checkBox.value = true;
    }

}

function clickEvent(event) {
    var element = event.target;
    var groupName = element.radioBtt.name;

    resetLast(groupName);

    element.label.setAttribute("selected", true);
    element.radioBtt.click();
    lastSelect = element;


    selectedMap.set(groupName, element.label);
}

function resetLast(name) {
    var lastFromGroup = selectedMap.get(name);
    if (lastFromGroup)
        lastFromGroup.setAttribute("selected", false);
}

$('#oleoType').select2({
    minimumInputLength: 0,
});

$('#searchType').select2({
    minimumResultsForSearch: -1,
});

function processa(data, count, page) {
    var more = page * pageSize < count;
    return { results: data, more: more };
};

function processVendedorResult(result, page) {
    var data = result.data.map(function (item) {
        return {
            id: item.Id,
            text: item.Nome,
        }
    });

    return processa(data, result.count, page);
}

function processTecnicoResult(result, page) {
    var data = result.data.map(function (item) {
        return {
            id: item.Id,
            text: item.Nome,
        }
    });

    return processa(data, result.count, page);
}

function requestController(term, page) {
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        termoBusca: term
    }
}


criaSelect2(select2configFactory('#vendedor', ENDPOINT.VENDEDOR, requestController, processVendedorResult));
criaSelect2(select2configFactory('#tecnico', ENDPOINT.TECNICO, requestController, processTecnicoResult));

//hack
if ($('#vendedor').val()) {
    var container1 = document.querySelector('#s2id_vendedor');
    var select2 = container1.querySelector('.select2-chosen');
    select2.textContent = $('#vendedor').val() + ' - ' + $('#vendedorName').val();
}

if ($('#tecnico').val()) {
    var container2 = document.querySelector('#s2id_tecnico');
    var select2 = container2.querySelector('.select2-chosen');
    select2.textContent = $('#tecnico').val() + ' - ' + $('#tecnicoName').val();
}

var especificacaoData = [
    { id: '5W30', text: '5W30' },
    { id: '5W40', text: '5W40' },
    { id: '10W40', text: '10W40' },
    { id: '15W40', text: '15W40' },
    { id: '20W50', text: '20W50' }]

$("#especificacao").select2({
    minimumInputLength: 0,
    query: function (query) {
        var data = { results: [] };
        data.results.push({ id: query.term, text: query.term });
        data.results = data.results.concat(especificacaoData);
        query.callback(data);
    }
});

if ($('#especificacao').val()) {
    var container = document.querySelector('#s2id_especificacao');
    var select2 = container.querySelector('.select2-chosen');
    select2.textContent = $('#especificacao').val();
}

if ($('#oleoType').val()) {
    var container = document.querySelector('#s2id_oleoType');
    var select2 = container.querySelector('.select2-chosen');
    select2.textContent = $('#oleoType').val();
}

function dataChange(event) {
    var input = event.target;
    input.setAttribute("value", input.value);
}

function abrirModalOrcamentos(event) {
    setVisibilidadeLoading(true);
    var searchType = document.querySelector('#searchType');
    if (searchType.value == "codigo-orcamento")
        return true;
    else {
        var idPesquisa = document.querySelector('#id');
        var tipoCheckup = document.querySelector('#IsCheckupCar');
        $.ajax({
            type: "POST",
            url: ENDPOINT.SEARCH,
            data: {
                id: idPesquisa.value,
                tipoPesquisa: searchType.value,
                tipoCheckup: tipoCheckup.value == "True" ? "varejo" : "truck"
            }
        }).done(function (response) {
            if (response.id) {
                var endpoint = ($("#IsCheckupCar").val() == "True" ? "/Checkup/CheckupVarejo/" : "/Checkup/CheckupTruck/") + response.id;
                window.location.href = endpoint;
            } else {
                setVisibilidadeLoading(false);
                $("#modalOrcamentos").html(response);
                $("#modalOrcamentos").modal();
            }
        }).fail(function (response) {
            setVisibilidadeLoading(false);
        });
        return false;
    }
}

$('#typeCheckup').select2({
    minimumInputLength: 0,
});

$('#typeCheckup').on('change', function (props) {
    if (props.val == "truck" || props.val == "car")
        document.querySelector('#pesquisa').setAttribute('disabled', true);
    else
        document.querySelector('#pesquisa').removeAttribute('disabled');
});

function visibilidadeGifCarregandoGrid(element, visibilidade) {
    kendo.ui.progress(element, visibilidade);
}

function filtrarCheckup() {
    visibilidadeGifCarregandoGrid($("#gridBusca"), true);
    var pesquisa = $('#pesquisa').val();
    var typeSearch = $('#typeCheckup').val();

    $.ajax({
        type: "POST",
        url: ENDPOINT.BUSCA,
        data: { tipoConsulta: typeSearch, codigo: pesquisa }
    }).done(function (response) {
        visibilidadeGifCarregandoGrid($("#gridBusca"), false);
        $("#gridBusca").data("kendoGrid").dataSource.data(response);      
    });
}

function gridConsultasCheckup() {
    $('#gridBusca').kendoGrid({
        dataSource: {
            pageSize: 10
        },
        height: 350,
        sortable: true,
        pageable: {
            buttonCount: 5
        },
        columns: [{
            title: "Tipo",
            field: "tipo",
            template: renderTipo.bind(this),
            width: 20
        }, {
            field: "Id",
            title: "ID",
            width: 20
        },
        {
            field: "Cliente",
            title: "Cliente",
            width: 100
        },
        {
            field: "CPFCNPJ",
            title: "CPF\CNPJ",
            template: kendo.template($("#template-cpf-cnpj").html()),
            width: 40
        },
        {
            field: "License",
            title: "Placa",
            width: 30
        },
        {
            field: "Car",
            title: "Veículo",
            width: 40
        },
        {
            title: "Opções",
            template: kendo.template($("#template-detalhes").html()),
            width: 40

        }]
    });
}

function renderTipo(a) {
    if (a.IsCheckupCar)
        return '<span class="icon-icon-car icon-grid"></span>';
    else
        return '<span class="icon-icon-truck icon-grid"></span>';

}

function excluirCheckupModal(event) {
    var container = event.currentTarget.closest('div');
    var input = container.querySelector('input');

    var inputModal = document.querySelector('#exluirCheckupModal').querySelector('input');
    inputModal.replaceWith(input);

    $("#exluirCheckupModal").modal();
}

function onFiltroTipoFiltroSelected(id, campo) {
    switch (id) {
        case "placa":
            $(campo).mask('AAA-9999');
            break;
        case "cpf":
            $(campo).mask('000.000.000-00');
            break;
        case "cnpj":
            $(campo).mask('00.000.000/0000-00');
            break;
        case "codigo-cliente":
            $(campo).mask('AAAAAA-AA');
            break;
        case "":
        case "nomecliente":
        case "veiculo":
        case "nomecliente":
            $(campo).unmask();
            break;
    }
}

function initConsulta() {
    $("#searchType").on("select2-selected", function (event) {
        onFiltroTipoFiltroSelected(event.choice.id, "#id")
    });

}

function init() {
    $("#typeCheckup").on("select2-selected", function (event) {
        onFiltroTipoFiltroSelected(event.choice.id, "#pesquisa")
    });

    $('#pesquisa').keypress(function (e) {
        if (e.which == 13) {
            filtrarCheckup();
        }
    });

    $.ajax({
        type: "POST",
        url: ENDPOINT.BUSCA,
        data: { tipoConsulta: 'usuario', codigo: '' }
    }).done(function (response) {
        $("#gridBusca").data("kendoGrid").dataSource.data(response);
    })
}

function EnviarEmail() {
    setVisibilidadeLoading(true);
    $("#EnviarPorEmail").submit();
    $('#emailModal').modal('hide');
}

function submitForm(event) {
    event.currentTarget.closest('form').submit();
}

function formataCpfCnpj(valor) {
    return valor.length > 11 ? valor.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "\$1.\$2.\$3\/\$4\-\$5") : valor.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
}