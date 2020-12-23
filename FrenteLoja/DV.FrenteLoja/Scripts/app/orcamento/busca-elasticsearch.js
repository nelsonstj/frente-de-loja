'use strict';

var buscaEstoqueProdutos = '/Orcamento/BuscaEstoqueProdutos';
var buscaValoresProdutos = '/Orcamento/BuscaValoresProdutos';
var filialUsuario = $("#FilialUsuario").val();
var orcamentoId = $("Orcamento")

function buscaProdutoPorGrupo(evt, grupoProduto) {
  var novoGrupoClass = grupoProduto ? grupoProduto.toLowerCase() : grupoProduto;
  var oldGrupoClass = $("#ProdutoCodGrupo").val().toLowerCase();
  $("#botoes-grupo").find('.btn-black').removeClass("btn-black " + oldGrupoClass + '-white').addClass(oldGrupoClass);
  $(evt.target).removeClass(novoGrupoClass);
  $(evt.target).addClass("btn-black " + novoGrupoClass + "-white");
  $("#ProdutoCodGrupo").val(grupoProduto);

  //buscaProdutos();
}

function buscaProdutosElastic() {
  setVisibilidadeLoading(true);
  window.store = {
    produtos: [],
    renderizados: [],
    filtro: {},
    indexRender: 20,
    indexEstoque: 40
  }

  renderizaProdutos();
  $("#App").unbind('scroll');
  var filtro = parseParams($("#busca-produto").serialize());
  var result = criaElasticSerachQuery(filtro, $("#switchToggle").is(':checked'));

  $.ajax({
    type: "POST",
    url: elasticSearchUrl,
    data: JSON.stringify(result),
    contentType: "application/json",
    dataType: "json",
  }).done(trataRetornoElasticSearch)
    .fail(function (response) {
      console.log(response.responseText);
      setVisibilidadeLoading(false);
      criaAlertaMensagem("Não foi possível realizar a busca.")
    });

  $("#App").scroll(onAppScroll);
}

function criaElasticSerachQuery(filtro, ignoraVeiculo) {
    var result = {
        //funciona mas tem que habilitar no elastic search (pode aumentar bastante a memoria)
        //sort: [{
        //  Descricao: {
        //    order: "asc", unmapped_type: "string"
        //  }
        //}],
        size: 1000,
        query: {
            bool: {},
        },
        sort: [
            { "ProdutoCodDellavia.keyword": { "order": "desc" } }, 
            { "PrioridadeOrdenacao.keyword": { "order": "asc" } },
            { "_score": { "order": "desc" } }

        ]
    };
    
    if (ignoraVeiculo) {
        must = [{
            range: {
                VeiculoAnoInicial: {
                    "lte": filtro.AnoVeiculo
                }
            }
        },
        {
            range: {
                VeiculoAnoFinal: {
                    "gte": filtro.AnoVeiculo
                }
            }
        }]

        must.push({ match: { VeiculoMarca: filtro.MarcaVeiculo } });
        must.push({ match: { VeiculoModelo: filtro.ModeloVeiculo } });
        //must.push({ match: { VeiculoVersao: filtro.VersaoVeiculo } });

        //Ini
        //A base do cliente não esta igual ao finesp, para isso, fiz esse ajuste para remover um campo
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
            must.push({ match: { VeiculoVersao: VersaoVeiculoAux } });
        }
        //Fim

        must.push({ match: { VersaoMotor: filtro.VersaoMotor } });
           
    } else {
        var must = [];
    }

  if (filtro.FabricantePeca) {
    must.push({ match: { ProdutoFabricantePeca: filtro.FabricantePeca } });
  }

  if (filtro.ProdutoCodGrupo) {
    must.push({ match: { ProdutoCodGrupo: filtro.ProdutoCodGrupo } });
  }


  if (filtro.Descricao) {
    must.push({
      query_string: {
        minimum_should_match: "100%",
        query: filtro.Descricao.replace(/\\/, " ").replace("/", " "),
        fields: ["ProdutoCodFabricante", "ProdutoDescricao", "ProdutoCodDellavia"]
      }
    })
  }

  //result.query.bool.must = range;
  //result.query.bool.should = should;
  if (must) {
      result.query.bool.must = must;
  }


  return result;
}

function Left(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str
    else
        return String(str).substring(0, n);
}


function trataRetornoElasticSearch(response, error) {
  if (response.hits.total) {
    $("#produto-nao-encontrado").hide();
    //verificar url elastic trazendo o primeiro ou o ultimo item como fielddata   
    response.hits.hits = response.hits.hits.filter(function (item) { return item._id !== "_search" })


    window.store.produtos = response.hits.hits.map(function (item, index) {

      item._source.estoque = "busca";

      return item._source;
    });

    window.store.produtos.map(function (item, index) {
      item.Id = index;
        if (item.ProdutoCodDellavia) {
            item.ImgProduto = dellaviaUrl + item.ProdutoCodDellavia + '.jpg';
        } else {
            item.ImgProduto = dellaviaUrl + item.ProdutoCodFabricante.trim().toUpperCase() + '.jpg';
        }
      return item;
      });
  
      //removeDuplicates(window.store.produtos, 'ProdutoCodDellavia').map(item => item.ProdutoCodDellavia);
      window.store.produtos = removeDuplicates(window.store.produtos, 'ProdutoCodFabricante');   
      _.orderBy(window.store.produtos, ['ProdutoCodDellavia'], ['desc'])

    setRenderizados();
    renderizaProdutos();
    buscaEstoque();
    setVisibilidadeLoading(false);
  } else {
    setVisibilidadeLoading(false);
    $("#produto-nao-encontrado").show();
  }
}


function removeDuplicates(myArr, prop) {
    return myArr.filter((obj, pos, arr) => {
        return arr.map(mapObj => mapObj[prop]).indexOf(obj[prop]) === pos;
    });
}

function setRenderizados() {
  window.store.renderizados = window.store.produtos.slice(0, window.store.indexRender);
}

function renderizaProdutos() {
  var numeroOrcamento = $("#Id").val();
  var props = {
    itens: window.store.renderizados,
    numeroOrcamento: numeroOrcamento
  }
  ReactDOM.render(window.Main(props, numeroOrcamento),
    document.getElementById('App')
  );
}


function buscaEstoque() {
  //Arrumar promises da busca de estoque pois se o cara der scroll muito rapido ele manda buscar o mesmo id dos itens

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

    setRenderizados();
    renderizaProdutos();
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

function onAppScroll() {
  if ($("#App").scrollTop() + $("#App").height() >= $("#App")[0].scrollHeight) {

    window.store.indexRender += 20;
    if (window.store.produtos.length > window.store.indexRender) {
      setRenderizados();
      renderizaProdutos();
      buscaEstoque();
    }
  }
}