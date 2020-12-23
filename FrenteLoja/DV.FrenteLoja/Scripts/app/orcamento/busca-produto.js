'use strict';

var pesquisarProdutos = '/Orcamento/PesquisarProdutos';
var obterFabricantePeca = '/Orcamento/ObterFabricantePeca';
var obterFilialDellavia = '/Orcamento/ObterFilialDellavia';
var removerItemCarrinho = '/Orcamento/RemoverItemCarrinho';
var editarOrcamentoItem = '/Orcamento/AbrirEditarOrcamentoItem';
var atualizarOrcamentoItem = '/Orcamento/AtualizarOrcamentoItem';
var buscaEstoqueProdutos = '/Orcamento/BuscaEstoqueProdutos';
var buscaValoresProdutos = '/Orcamento/BuscaValoresProdutos';

//================================================================

function processaLoja(result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            id: obj.id || obj.CampoCodigo,
            text: obj.text || obj.Descricao
        };
    });
    return processa(data, result.count, page);
};

function buscaProdutoPorGrupoSql(evt, grupoProduto) {
    var novoGrupoClass = grupoProduto; // ? grupoProduto.toLowerCase() : grupoProduto;
    var oldGrupoClass = $("#GrupoAplicacao").val().toLowerCase();
    $("#botoes-grupo").find('.btn-black').removeClass("btn-black " + oldGrupoClass + '-white').addClass(oldGrupoClass);
    $(evt.target).removeClass(novoGrupoClass);
    $(evt.target).addClass("btn-black " + novoGrupoClass + "-white");
    $("#GrupoAplicacao").val(grupoProduto);

    buscaProdutosSql(grupoProduto);
}

function buscaProdutosSql(grupoProduto) {
    setVisibilidadeLoading(true);
    window.store = {
        produtos: [],
        renderizados: [],
        filtro: {},
        indexRender: 20,
        indexEstoque: 40
    }
    renderizaProdutosProd();
    $("#App").unbind('scroll');
    var filtro = parseParams($("#busca-produto").serialize());
    var result = criaSearch(filtro, $("#switchToggle").is(':checked'));

    //alert(JSON.stringify(result));
    $.ajax({
        type: "POST",
        url: pesquisarProdutos,
        data: { "search": JSON.stringify(result) },
        dataType: "json",
    }).done(trataRetornoSearch)
        .fail(response => {
            console.log(response.responseText);
            setVisibilidadeLoading(false);
            criaAlertaMensagem(response.responseText)
    });

    setRenderizadosProd();
    renderizaProdutosProd();
    $("#App").scroll(onAppScrollProd);
}

function renderizaProdutosProd() {
    var numeroOrcamento = $("#Id").val();
    var props = {
        itens: window.store.renderizados,
        numeroOrcamento: numeroOrcamento
    }
    ReactDOM.render(window.Main(props, numeroOrcamento),
        document.getElementById('App')
    );
}

function setRenderizadosProd() {
    window.store.renderizados = window.store.produtos.slice(0, window.store.indexRender);
}

function criaSearch(filtro, consideraVeiculo) {
    var result = {};
    if (consideraVeiculo) {
        if (filtro.VeiculoIdFraga) {
            result.VeiculoIdFraga = filtro.VeiculoIdFraga;
        } else {
            result.VeiculoAnoInicial = filtro.AnoVeiculo;
            result.VeiculoAnoFinal = filtro.AnoVeiculo;
            result.VeiculoMarca = filtro.MarcaVeiculo;
            result.VeiculoModelo = filtro.ModeloVeiculo;
            //Ini
            //A base do cliente não esta igual ao sinesp, para isso, fiz esse ajuste para remover um campo
            //quando a base estiver igual descomente a linha 83 e remova esse codigo do ini ao fim.
            if (filtro.VersaoVeiculo) {
                var splitted = filtro.VersaoVeiculo.split(" ");
                var i;
                var VersaoVeiculoAux = "";
                for (i = 0; i <= (splitted.length - 1); i++) {
                    if (splitted[i] != "") {
                        if (VersaoVeiculoAux == "") {
                            var tamanhoString = splitted[i].length;
                            VersaoVeiculoAux = Left(splitted[i], tamanhoString);
                        } else {
                            VersaoVeiculoAux = VersaoVeiculoAux + " " + splitted[i];
                        }
                    }
                }
                result.VeiculoVersao = VersaoVeiculoAux;
            }
            //Fim
            result.VersaoMotor = filtro.VersaoMotor;
        }
    }
    if (filtro.FabricantePeca) {
        result.ProdutoFabricantePeca = filtro.FabricantePeca;
    }
    if (filtro.GrupoAplicacao) {
        result.CodigoGrupo = filtro.GrupoAplicacao;
    }
    if (filtro.Descricao) {
        result.ProdutoDescricao = filtro.Descricao.replace(/\\/, " ");
    }
    if ($("#TabelaPreco").val()) {
        result.TabelaPreco = $("#TabelaPreco").val()
    }
    if (filtro.FilialUsuario) {
        result.IdLojaDestino = filtro.FilialUsuario;
    }

    return result;
}

function trataRetornoSearch(response, error) {
    if (response) {
        $("#produto-nao-encontrado").hide();
        response = response.filter(item => { return item.ProdutoCodFabricante !== "" })
        window.store.produtos = response.map(item => {
            item.estoque = "busca";
            return item;
        });
        window.store.produtos.map(function (item, index) {
            item.Id = index;
            if (item.ProdutoCodDellavia) {
                item.ImgProduto = dellaviaUrl + item.ProdutoCodDellavia.trim() + '.jpg';
            } else {
                item.ImgProduto = dellaviaUrl + item.ProdutoCodFabricante.trim().toUpperCase() + '.jpg';
            }
            item.ImgGrupoProduto = dellaviaUrl + item.CodigoSubGrupo.trim().toUpperCase() + '.jpg';
            return item;
        });
        window.store.produtos = removeDuplicatesProd(window.store.produtos, 'ProdutoCodDellavia');
        //_.orderBy(window.store.produtos, ['ProdutoCodDellavia'], ['desc'])

        setRenderizadosProd();
        renderizaProdutosProd();
        //buscaEstoqueProd();
        setVisibilidadeLoading(false);
    } else {
        setVisibilidadeLoading(false);
        $("#produto-nao-encontrado").show();
    }
}

function removeDuplicatesProd(myArr, prop) {
    return myArr.filter((obj, pos, arr) => {
        return arr.map(mapObj => mapObj[prop]).indexOf(obj[prop]) === pos;
    });
}

function buscaEstoqueProd() {
    //Arrumar promises da busca de estoque pois se der scroll muito rapido ele manda buscar o mesmo id dos itens
    var promiseEstoque = $.Deferred();
    var promiseValor = $.Deferred();
    var numeroOrcamento = $("#Id").val();

    var requestEstoque = {
        IdFilial: filialUsuario,
        Produtos: window.store.renderizados.reduce(function (anterior, atual) {
            if (atual.estoque == "busca") {
                anterior = anterior.concat(atual.ProdutoCodDellavia)
            }
            return anterior;
        }, [])
    }

    var requestValor = {
        IdOrcamento: numeroOrcamento,
        Produtos: window.store.renderizados.reduce(function (anterior, atual) {
            if (atual.ProdutoCodDellavia) {
                anterior = anterior.concat(atual.ProdutoCodDellavia)
            }
            return anterior;
        }, [])
    }

    $.when(promiseEstoque, promiseValor).done(function (estoque, valores) {
        if (estoque !== "erro") {
            estoque.map(function (item) {
                window.store.produtos.find(function (produto) {
                    if (item.CodigoDellaVia == produto.ProdutoCodDellavia) {
                        produto.estoque = item.SaldoDisponivel;
                    }
                });
            });
        }

        if (valores !== "erro") {
            valores.map(function (item) {
                window.store.produtos.find(function (produto) {
                    if (item.CampoCodigo == produto.ProdutoCodDellavia) {
                        produto.valor = item.Valor;
                    }
                });
            });
        }

        setRenderizadosProd();
        renderizaProdutosProd();
    });

    $.ajax({
        type: "POST",
        url: buscaValoresProdutos,
        data: requestValor
    }).done(function (response, error) {
        promiseValor.resolve(response);
    }).fail(function (erro) {
        window.store.produtos.map(function (produto) {
            produto.valor = 0;
        });
        promiseValor.resolve("erro");
    });

    $.ajax({
        type: "POST",
        url: buscaEstoqueProdutos,
        data: requestEstoque
    }).done(function (response, error) {
        promiseEstoque.resolve(response);
    }).fail(function (erro) {
        window.store.produtos.map(function (produto) {
            produto.estoque = 'erro';
        });
        promiseEstoque.resolve("erro");
    });
}

function onAppScrollProd() {
    if ($("#App").scrollTop() + $("#App").height() >= $("#App")[0].scrollHeight) {
        window.store.indexRender += 20;
        if (window.store.produtos.length > window.store.indexRender) {
            setRenderizadosProd();
            renderizaProdutosProd();
            //buscaEstoqueProd();
        }
    }
}

//=================================================================
function processaSomenteDescricao(result, page) {
  var data = $.map(result.data, function (obj) {
    return {
      id: obj,
      text: obj
    };
  });
  return processa(data, result.count, page);
};

function criaSelectFilialModalEstoque() {
    criaSelect2(select2configFactory("#filial-dellavia", obterFilialDellavia, requestTermoController, processaLoja))
}

function removeItem(event, orcamentoId, orcamentoItemId, tipoItem) {
  event.stopPropagation();

  var clickSim = function () {
    setVisibilidadeLoading(true);
    $("#sacola-compra").load(removerItemCarrinho, { orcamentoId: orcamentoId, orcamentoItemId: orcamentoItemId }, function () {
      setVisibilidadeLoading(false);
    })
  }

  var clickNao = function () {
    console.log('negado');
  }

  if (tipoItem === "1") {
    criaModalMensagem("Exclusão de produto", "Deseja excluir o produto e seus serviços correlacionados?", clickSim, clickNao)
  } else {
    criaModalMensagem("Exclusão do serviço", "Deseja excluir o serviço?", clickSim, clickNao)
  }
}

function editaOrcamentoItem(idOrcamento, idOrcamentoItem) {
  setVisibilidadeLoading(true);
  $.ajax({
    type: "POST",
    url: editarOrcamentoItem,
    data: { idOrcamento: idOrcamento, idOrcamentoItem: idOrcamentoItem }
  }).done(function (response) {
    $("#modal-produto-editar").html(response);
    $("#modal-produto-editar").modal();
    setVisibilidadeLoading(false);
    criaMultiplicadores();
  }).fail(function (response) {
    criaAlertaMensagem(response.responseJSON);
    setVisibilidadeLoading(false);
  });
}

function criaSacolaSubmit() {
  setVisibilidadeLoading(true);
  $("#relacionado-grid").css('z-index', 1030);
}

function criaSacolaSuccess(response) {
  $("#sacola-compra").html(response);
  setVisibilidadeLoading(false);
  $("#relacionado-grid").css('z-index', "");
  $("#relacionado-grid").modal('hide');
}

function criaSacolaFail(response) {
  criaAlertaMensagem(response.responseJSON, $("#modal-warning"));
  setVisibilidadeLoading(false);
  $("#relacionado-grid").css('z-index', "");
}

function editaSacolaSubmit() {
  setVisibilidadeLoading(true);
  $("#modal-produto-editar").css('z-index', 1030);
}

function editaSacolaSuccess(response) {
  $("#sacola-compra").html(response);
  setVisibilidadeLoading(false);
  $("#modal-produto-editar").css('z-index', "");
  $("#modal-produto-editar").modal('hide');
}

function editaSacolaFail(response) {
  criaAlertaMensagem(response.responseJSON, $("#modal-warning"));
  setVisibilidadeLoading(false);
  $("#modal-produto-editar").css('z-index', "");
}

function negociacaoSubmit() {
    var menssagem = '';
    if ($("#produtosCount").val() === "0") {
        menssagem = "É necessário incluir um produto."
    }
    if (menssagem) {
        criaAlertaMensagem(menssagem);
        return false;
    } else {
        return true;
    }
}

function initBuscaProdutos() {
    $('#clearSearch').click(function () {
        $('#search input').val('');
        $('#search input').focus();
    });
    criaSelect2(select2configFactory("#FabricantePeca", obterFabricantePeca, requestTermoController, processaSomenteDescricao));
    if ($("#TipoOrcamento").val() === "2" || $("#TipoOrcamento").val() === "Retira") {
        if ($("#switchToggle").is(':checked')) {
            $("#switchToggle").click();
            $("#switchToggle").prop("disabled", "disabled");
        }
    }
};
