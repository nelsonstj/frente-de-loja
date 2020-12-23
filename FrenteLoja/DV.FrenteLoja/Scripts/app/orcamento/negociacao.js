
var aplicarDescontos = '/Orcamento/AbrirModalDesconto';
var equipeMontagem = '/Orcamento/AbrirModalEquipeMontagem';
var parcelamento = '/Orcamento/AbrirModalParcelamento';
var calcularAjuste = '/Orcamento/CalcularAjuste';
//var calculaDescontoModeloVenda = '/Orcamento/CalculaDescontoModeloVenda';
var buscaProfissional = '/Orcamento/BuscaProfissional';
var adicionarProfissional = '/Orcamento/AdicionarProfissionalMontagem';
var removerProfissional = '/Orcamento/RemoverProfissionalMontagem';
var buscaProfissionalMontagem = '/Orcamento/ObterProfissionalMontagem';
var removerItemCarrinhoRefresh = '/Orcamento/RemoverItemCarrinhoRefresh';
var buscaCondicoesPagamento = '/Orcamento/ObterCondicoesPagamento';
var buscaBandeira = '/Orcamento/ObterBandeira';
var buscaBanco = '/Orcamento/ObterBanco';
var adicionaFormaPagamento = '/Orcamento/AdicionaPagamento';
var removeFormaPagamento = '/Orcamento/RemovePagamento';
var finalizacaoOrcamento = '/Orcamento/Finalizacao';
var obterSaldoPagamento = '/Orcamento/ObterSaldoPagamento';
var montarEquipeMontagem = '/Orcamento/MontarEquipeMontagem';
var formaPagamento;
var percentualAcrescimo;

function requestTermoCondicoesPagamentoController(term, page) {
    var areaNegocio = $("#AreaNegocio").val();
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        termoBusca: term,
        areaNegocio: areaNegocio
    };
}

function requestTermoAdministradoraController(term, page) {
    var idFormaPagto = formaPagamento;
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        termoBusca: term,
        idCondPagto: idFormaPagto
    };
}

function requestTermoIdFilialController(term, page) {
    var idFilial = $("#IdLojaOrcamento").val();
    return {
        tamanhoPagina: pageSize,
        numeroPagina: page,
        termoBusca: term,
        idFilial: idFilial
    };
}

function processaProfissionalNome(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.Id,
            text: obj.text || obj.ProfissionalNome
        };
    });
    return processa(data, result.count, page);
};

function processaDescricaoTipoPagamento(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.Id,
            text: obj.text || obj.Descricao,
            tipoFormaPagamento: obj.TipoFormaPagamento,
            formaPagamento: obj.FormaPagamento,
            percentualAcrescimo: obj.PercentualAcrescimo
        };
    });
    return processa(data, result.count, page);
};

//#region Desconto
function abrirModalDesconto(idOrcamentoItem, idOrcamento, qtdeItem) {
    setVisibilidadeLoading(true);
    $.ajax({
        type: "POST",
        url: aplicarDescontos,
        data: { idOrcamento: idOrcamento, idOrcamentoItem: idOrcamentoItem }
    }).done(function (response) {
        $("#modal-desconto").html(response);
        setNegociacaoBlurEvents();
        //setNegociacaoSelectedEvents(idOrcamentoItem, idOrcamento, qtdeItem);
        if ($("#ObservacaoGeral").val() || $("#ObservacaoItem").val())
            $("#div-alcada").show();
        setVisibilidadeLoading(false);
        $("#modal-desconto").modal('show');
    }).fail(function (response) {
        criaAlertaMensagem(response.responseJSON);
        setVisibilidadeLoading(false);
    });
}

function setNegociacaoBlurEvents() {
    $("#PercentualDesconto").focus(onFocusDesconto);
    $("#ValorDesconto").focus(onFocusDesconto);
    $("#PercentualDesconto").blur(onBlurDesconto);
    $("#ValorDesconto").blur(onBlurDesconto);
}

function onFocusDesconto(event) {
    $("#ControleDesconto").val(event.target.id);
    // $("#botao-aplica").attr("disabled", true);
    if ($("#ControleDesconto").val() == "ValorDesconto") {
        $("#PercentualDesconto").prop('disabled', 'disabled');
        $("#ValorDesconto").removeAttr('disabled');
    } else {
        $("#PercentualDesconto").removeAttr('disabled');
        $("#ValorDesconto").prop('disabled', 'disabled');
    }
}

function onBlurDesconto(event) {
    var desconto = event.target.value;
    var total = $("#ValorOriginal").val();
    var tipo = this.getAttribute('data-target');
    $.ajax({
        type: "POST",
        url: calcularAjuste,
        data: { tipo: tipo, valor: desconto, total: total }
    }).done(function (response) {
        $("#ValorDesconto").removeAttr('disabled');
        $("#ValorDesconto").val(response.valor);
        $("#PercentualDesconto").removeAttr('disabled');
        $("#PercentualDesconto").val(response.percentual);
        //response.tipo == "percentual" ? $("#ValorDesconto").prop('disabled', 'disabled') : $("#PercentualDesconto").prop('disabled', 'disabled');
        $("#ValorTotalComDesconto").val(response.total);
        validaLimiteDesconto(response.percentual, response.valor);
    });
    $("#botao-aplica").attr("disabled", false);
}

/*function calcularDesconto(event) {
    //var desconto = event.target.value;
    var desconto = 0;
    var tipo = "percentual";
    // Pelo título.
    var inputPercentual = document.querySelector('input[id="PercentualDesconto"]');
    var inputValor = document.querySelector('input[id="ValorDesconto"]');

    if (inputPercentual.disabled) {
        desconto = $("#ValorDesconto").val();
        tipo = inputValor.getAttribute('data-target');
    } else {
        desconto = $("#PercentualDesconto").val();
        tipo = inputPercentual.getAttribute('data-target');
    }
    var total = $("#ValorOriginal").val();
    //var tipo = this.getAttribute('data-target');
    $.ajax({
        type: "POST",
        url: calculaValorDesconto,
        data: { total: total, desconto: desconto, tipo: tipo }
    }).done(function (response) {
        $("#ValorDesconto").removeAttr('disabled');
        $("#ValorDesconto").val(response.valorDesconto);
        //$("#ValorDesconto").prop('readonly', 'readonly');
        $("#PercentualDesconto").removeAttr('disabled');
        $("#PercentualDesconto").val(response.percentualDesconto);
        //$("#PercentualDesconto").prop('readonly', 'readonly');
        //response.tipo == "percentual" ? $("#ValorDesconto").prop('readonly', 'readonly') : $("#PercentualDesconto").prop('readonly', 'readonly');
        $("#ValorTotalComDesconto").val(response.totalComDesconto);
        validaLimiteDesconto(response.percentualDesconto, response.valorDesconto);
    });
    $("#botao-aplica").attr("disabled", false);
}
*/

function validaLimiteDesconto(percentualDesconto, valorDesconto) {
    var mensagem = "";
    percentualDesconto = percentualDesconto.replace(',', '.')
    var percentualLimiteDesconto = $("#PercentualLimiteDesconto").val().replace(',', '.');
    var percentualDescontoAlcadaGerente = $("#PercentualDescontoAlcadaGerente").val().replace(',', '.');
    if (parseFloat(percentualDesconto) > parseFloat(percentualLimiteDesconto)) {
        if ($("#perfil-telemarketing").val() == 'true') {
            mensagem = "Percentual desconto acima do permitido, valor máximo " + $("#PercentualLimiteDesconto").val() + "%!";
        } else {
            if (parseFloat(percentualDesconto) > parseFloat(percentualDescontoAlcadaGerente)) {
                mensagem = "Percentual informado excedeu o limite gerencial, valor máximo " + $("#PercentualDescontoAlcadaGerente").val() + "%!";
            } else {
                $("#div-alcada").show();
                return true;
            }
        }
    } else {
        $("#div-alcada").hide();
        return true;
    }
    if (mensagem) {
        criaAlertaMensagem(mensagem, "#modal-warning");
        return false;
    }
}

/*function calcularDescontoModeloVenda(modeloVenda, idItemOrcamento, idOrcamento, qtdeItem) {
    $.ajax({
        type: "POST",
        url: calculaDescontoModeloVenda,
        data: {
            selecionado: modeloVenda, idItemOrcamento: idItemOrcamento,
            orcamentoId: idOrcamento, qtdeItem: qtdeItem
        }
    }).done(function (response) {
        $("#ValorOriginal").val(response.valorOriginal)
        $("#PercentualDesconto").val(response.percentualDesconto)
        $("#ValorDesconto").val(response.valorDesconto);
        $("#ValorTotalComDesconto").val(response.totalComDesconto);
        $("#qtdeModeloVenda").html(response.qtdeModeloVenda);
        $("#PercentualDesconto").prop('readonly', 'readonly');
        $("#ValorDesconto").prop('readonly', 'readonly');
    })
}
*/
/*function setNegociacaoSelectedEvents(idItemOrcamento, idOrcamento, qtdeItem) {
    $("#div-modelo-venda").find("input[type=radio]").click(function () {
        $("#PercentualDesconto").val($("#DescontoModeloVenda" + this.value).val()).keyup();
        calcularDescontoModeloVenda(this.value, idItemOrcamento, idOrcamento, qtdeItem);
    });
}
*/

function onBlurDescontoSubmit() {
    if ($("#ControleDesconto").val()) {
        var nomeCampo = $("#ControleDesconto").val();
        $("#" + nomeCampo).blur(onBlurDesconto);
    }
}

function aplicarDescontoSubmit() {
    if (!validaLimiteDesconto($("#PercentualDesconto").val(), $("#ValorDesconto").val()))
        return false;
    setVisibilidadeLoading(true);
    //onBlurDescontoSubmit();
    $("#modal-desconto").css('z-index', 1030);
}

function aplicarDescontoSuccess(response) {
    location.reload();
}

function aplicarDescontoFail(response) {
    criaAlertaMensagem(response.responseJSON, "#modal-warning");
    setVisibilidadeLoading(false);
    $("#modal-desconto").css('z-index', "");
}

/*function limparModeloDeVendas() {
    $('input[name=DescontoModeloVendaUtilizado]').prop('checked', false);
    $("#PercentualDesconto").val("0,00");
    $("#ValorDesconto").val("0,00");
    $("#ValorOriginal").val($("#ValorSalvo").val());
    $("#ValorTotalComDesconto").val($("#ValorSalvo").val());
    $("#PercentualDesconto").removeAttr('readonly');
    $("#ValorDesconto").removeAttr('readonly');
    $("#PercentualDesconto").removeAttr('disabled');
    $("#ValorDesconto").removeAttr('disabled');
    $("#qtdeModeloVenda").html($("#QuantidadeProduto").val());
}*/
//#endregion Desconto

//#region Equipe Montagem
function abrirModalEquipeMontagem(idOrcamento, idOrcamentoItem) {
    setVisibilidadeLoading(true);
    $.ajax({
        type: "POST",
        url: equipeMontagem,
        data: { idOrcamento: idOrcamento, idOrcamentoItem: idOrcamentoItem }
    }).done(function (response) {
        $("#modal-equipe-montagem").html(response);
        setVisibilidadeLoading(false);
        $("#modal-equipe-montagem").modal('show');
    }).fail(function (response) {
        criaAlertaMensagem(response.responseJSON);
        setVisibilidadeLoading(false);
    });
}

function loadMontagem() {
    $("input[id*='profissional']").each(function (index, element) {
        var select = criaSelect2(select2configFactory(element, buscaProfissionalMontagem, requestTermoIdFilialController, processaProfissionalNome));
        var id = $("input[name='Equipe[" + element.name + "].Id'").val();
        if (id) {
            var nome = $("input[name='Equipe[" + element.name + "].ProfissionalNome'").val();
            select.select2("data", { id: id, text: nome });
            //$("input[name='Equipe[" + element.name + "].ProfissionalNome'").val(val[0]);
        }
        select.on("select2-selected", function (event) {
            $("input[name='Equipe[" + event.target.name + "].Id'").val(event.choice.id);
            $("input[name='Equipe[" + event.target.name + "].ProfissionalNome'").val(event.choice.text);
        });
        select.on("select2-clearing", function (event) {
            $("input[name='Equipe[" + event.target.name + "].Id'").val("");
            $("input[name='Equipe[" + event.target.name + "].ProfissionalNome'").val("");
        });
    });
    $("select[name*='Funcao']").each(function (index, element) {
        criaSelect2(select2configFactory(element, null, null, null, -1));
    });
}

function getEquipeMontagem() {
    var equipe = [];
    $("input[id*='profissional']").each(function (index, element) {
        equipe.push({
            Id: $("input[name='Equipe[" + element.name + "].Id']").val(),
            ProfissionalNome: $("input[name='Equipe[" + element.name + "].ProfissionalNome']").val(),
            Funcao: $("select[name='Equipe[" + element.name + "].Funcao']").val()
        });
    });
    return {
        IdOrcamentoItem: $("#IdOrcamentoItem").val(),
        Equipe: equipe,
        IndexRemover: $("#IndexRemover").val(),
        IdLojaOrcamento: $("#IdLojaOrcamento").val(),
        DescricaoProduto: $("#DescricaoProduto").val()
    };
}

function adicionaProfissional(index) {
    var equipeMontagem = getEquipeMontagem();
    $.ajax({
        type: "POST",
        url: adicionarProfissional,
        data: { equipe: equipeMontagem }
    }).done(function (response) {
        $("#modal-equipe-montagem").html(response);
    }).fail(function (response) {
        criaAlertaMensagem(response.responseJSON, $("#modal-warning"));
    });
}

function removeProfissional(index) {
    $("#IndexRemover").val(index);
    var equipeMontagem = getEquipeMontagem();
    $.ajax({
        type: "POST",
        url: removerProfissional,
        data: { equipe: equipeMontagem }
    }).done(function (response) {
        $("#modal-equipe-montagem").html(response);
    })
}

function equipeMontagemSubmit() {
    setVisibilidadeLoading(true);
    $("#modal-equipe-montagem").css('z-index', 1030);
    var equipeMontagem = getEquipeMontagem();
    $.ajax({
        type: "POST",
        url: montarEquipeMontagem,
        data: { equipe: equipeMontagem }
    }).done(function (response) {
        equipeMontagemSuccess();
    }).fail(function (response) {
        equipeMontagemFail(response);
    });
}

function equipeMontagemSuccess() {
    location.reload();
}

function equipeMontagemFail(response) {
    criaAlertaMensagem(response.responseJSON, $("#modal-warning"));
    setVisibilidadeLoading(false);
    $("#modal-equipe-montagem").css('z-index', "");
}
//#endregion Equipe Montagem

//#region Formas de Pagamento
function calculaAcrescimo(percentualAcrescimo, tipo) {
    var total = $("#valor-forma-pagamento").val();
    $.ajax({
        type: "POST",
        url: calcularAjuste,
        data: { tipo: tipo, valor: percentualAcrescimo, total: total }
    }).done(function (response) {
        $("#condicao-acrescimo").val(response.valor);
        $("#valor-forma-pagamento").val(response.total);
        if (response.valor != "R$ 0,00") {
            $("#div-acrescimo").show();
            $("#valor-forma-pagamento").prop('readonly', 'readonly');
}
    });
    //$("#botao-aplica").attr("disabled", false);
}

function adicionaPagamento() {
    setVisibilidadeLoading(true);
    var forma = {
        Id: $("#condicao-pagamento").val(),
        IdAdministradoraFinanceira: $("#condicao-bandeira").val(),
        IdBanco: $("#condicao-banco").val(),
        Valor: $("#valor-forma-pagamento").maskMoney('unmasked')[0].toString().replace('.', ','),
        TipoFormaPagamento: $("#tipo-condicao").val(),
        IdOrcamento: $("#orcamento-id").val(),
        PercentualAcrescimo: percentualAcrescimo
    }
    $.ajax({
        type: "POST",
        url: adicionaFormaPagamento,
        data: { forma: forma }
    }).done(function (response) {
        location.reload();
    }).fail(function (response) {
        criaAlertaMensagem(response.responseJSON);
        setVisibilidadeLoading(false);
    })
}

function removePagamento(idPagamento) {
    $.ajax({
        type: "POST",
        url: removeFormaPagamento,
        data: { idPagamento: idPagamento }
    }).done(function (response) {
        location.reload();
    })
}

function setVisibilidadeTipoPagamento(tipo) {
    switch (tipo) {
        case 0: //cartao
            $("#div-bandeira").show();
            $("#div-banco").hide();
            break;
        case 1: //banco
            $("#div-bandeira").hide();
            $("#div-banco").show();
            break;
        case 2: // dinheiro
            $("#div-bandeira").hide();
            $("#div-banco").hide();
            break;
    }
}
//#endregion Formas de Pagamento

function abrirModalParcelamento(idOrcamentoFormaPagamento) {
    setVisibilidadeLoading(true);
    $.ajax({
        type: "POST",
        url: parcelamento,
        data: { idPagamento: idOrcamentoFormaPagamento }
    }).done(function (response) {
        $("#modal-parcelamento").html(response);
        $("#modal-parcelamento").modal();
        setVisibilidadeLoading(false);
    }).fail(function (response) {
        criaAlertaMensagem(response.responseJSON);
        setVisibilidadeLoading(false);
    })
}

function removerOrcamentoItem(tipoItem, idOrcamento, idItemOrcamento) {
    var clickSim = function () {
        setVisibilidadeLoading(true);
        $.ajax({
            type: "POST",
            url: removerItemCarrinhoRefresh,
            data: { idOrcamento: idOrcamento, idItemOrcamento: idItemOrcamento }
        }).done(function (response) {
            $("#partial-itens").html(response);
            //  $("#subtotal-negociacao").html(response.subtotal);
            $.ajax({
                type: "POST",
                url: obterSaldoPagamento,
                data: {
                    idOrcamento: idOrcamento
                }
            }).done(function (responseSaldo) {
                location.reload();
            });
        }).fail(function (response) {
            criaAlertaMensagem(response.responseJSON);
            setVisibilidadeLoading(false);
        });
    }

    var clickNao = function () {
        console.log('negado');
    }

    if (tipoItem === "1") {
        criaModalMensagem("Exclusão de produto", "Deseja excluir o produto  e seus serviços correlacionados?", clickSim, clickNao)
    } else {
        criaModalMensagem("Exclusão do serviço", "Deseja excluir o serviço ?", clickSim, clickNao)
    }
}

function setNegociacaoSomenteLeitura() {
    document.querySelector(".negociacao").querySelectorAll("input, select, textarea, button").forEach(function (item) {
        item.disabled = true;
    });
}

function removeDelecaoPagamentos() {
    document.querySelector("#partial-pagamento").querySelectorAll(".close").forEach(function (item) {
        item.remove();
    });
}

function finalizacaoSubmit() {
    var menssagem = '';
    if ($('#OrcamentoProdutoCount').val() == "0" && $("#ValorRestante").val() == "0,00") {
        menssagem = "É necessário incluir um produto e as condições de pagamento."
    }
    if ($("#ValorRestante").val() !== "0,00") {
        menssagem += "É necessário informar todas as condições de pagamento."
    }
    if (menssagem) {
        criaAlertaMensagem(menssagem);
        return false;
    } else {
        $("#ReservaEstoque").val($("#reserva").is(":checked"));
        return true;
    }
}

function initNegociacao() {
    if ($("#ReservaEstoque").val() == "True") {
        $("#reserva")[0].checked = true;;
    }
    if ($("#StatusSomenteLeitura").val() == "True") {
        setNegociacaoSomenteLeitura();
        removeDelecaoPagamentos();
    }

    var condicao = criaSelect2(select2configFactory("#condicao-pagamento", buscaCondicoesPagamento, requestTermoCondicoesPagamentoController, processaDescricaoTipoPagamento));
    criaSelect2(select2configFactory("#condicao-banco", buscaBanco, requestTermoController, processaDescricao));
    criaSelect2(select2configFactory("#condicao-bandeira", buscaBandeira, requestTermoAdministradoraController, processaDescricao));
    $("#ValorDesconto").maskMoney({ prefix: 'R$ ', allowNegative: false, thousands: '.', decimal: ',', affixesStay: true, allowZero: true });
    $("#condicao-acrescimo").maskMoney({ prefix: 'R$ ', allowNegative: false, thousands: '.', decimal: ',', affixesStay: true, allowZero: true });
    $("#valor-forma-pagamento").maskMoney({ prefix: 'R$ ', allowNegative: false, thousands: '.', decimal: ',', affixesStay: true, allowZero: true });
    $("#valor-forma-pagamento").val($("#valor-restante").text());

    condicao.on("select2-selected", function (event) {
        formaPagamento = event.choice.formaPagamento;
        tipoPagamento = event.choice.tipoFormaPagamento;
        percentualAcrescimo = event.choice.percentualAcrescimo;
        var tipo = document.getElementById("condicao-acrescimo").getAttribute('data-target');
        calculaAcrescimo(percentualAcrescimo, tipo);
        setVisibilidadeTipoPagamento(tipoPagamento);
        $("#tipo-condicao").val(tipoPagamento);
    });
    condicao.on("select2-clearing", function () {
        $("#valor-forma-pagamento").prop('readonly', '');
        $("#valor-forma-pagamento").val($("#valor-restante").text());
        $("#div-bandeira").hide();
        $("#div-banco").hide();
        $("#div-acrescimo").hide();
    });

    $("#finalizacao-form").submit(function () {
        $("#ReservaEstoque").val($("#reserva").is(":checked"));
    })
}