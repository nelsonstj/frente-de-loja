//var buscaCatalogo = '/CargaCatalogo/ObterCatalogos';
var pageSize = 20;
//$("#catalogo").select2({
//    placeholder: 'Catalogos',
//    minimumInputLength: 0,
//    allowClear: true,
//    ajax: {
//        //How long the user has to pause their typing before sending the next request
//        quietMillis: 500,
//        //The url of the json service
//        url: buscaCatalogo,
//        dataType: 'json',
//        //Our search term and what page we are on
//        data: (term, page) => {
//            return {
//                tamanhoPagina: pageSize,
//                numeroPagina: page,
//                termoBusca: term
//            };
//        },
//        cache: true,
//        results: (reponse, page) => {
//            //Used to determine whether or not there are more results available,
//            //and if requests for more data should be sent in the infinite scrolling
//            var more = page * pageSize < reponse.count;
//            return { results: reponse.data, more: more };
//        }
//    }
//});

//$("#catalogo").on("select2-selected", (e) => {
//    $('#valor').text(`${e.choice.id} ${e.choice.text}`);
//});


////UTIL

function parseParams(str) {
  return str.split('&').reduce(function (params, param) {
    var paramSplit = param.split('=').map(function (value) {
      return decodeURIComponent(value.replace('+', ' '));
    });
    params[paramSplit[0]] = paramSplit[1];
    return params;
  }, {});
}

$(function () {



  //var options = {
  //    onKeyPress: function (cpf, ev, el, op) {
  //        var masks = ['000.000.000-000', '00.000.000/0000-00'],
  //            mask = (cpf.length > 14) ? masks[1] : masks[0];
  //        el.mask(mask, op);
  //    }
  //}
  //$("#cpfcnpj").mask('000.000.000-000', options);

  var optionsTelefone = {
    translation: { 'Z': { pattern: /[0-9]/, optional: true } },
    onKeyPress: function (tel, ev, el, op) {
      console.log(tel.length)
      var masks = ['(00) 0000-0000Z', '(00) 00000-0000'],
        mask = (tel.length > 14) ? masks[1] : masks[0];
      el.mask(mask, op);
    }
  }
  $("#telefone").mask('(00) Z0000-0000', optionsTelefone);

});

function formatObj(item) {
  if (item.name.split('.').length > 1) {
    var nomeField = item.name.split('.')[item.name.split('.').length - 1]
    if (!itemLista[nomeField]) {
      itemLista[nomeField] = item.value;
    } else {
      var nomeProperty = item.name.split('.')[0]
      if (obj[nomeProperty]) {
        obj[nomeProperty].push(itemLista)
        itemLista = {}
      } else {
        obj[nomeProperty] = []
        obj[nomeProperty].push(itemLista)
        itemLista = {}
      }
    }
  } else {
    obj[item.name] = item.value;
  }
}

function setVisibilidadeLoading(visibilidade) {
  if (visibilidade) {
    $("#loading-modal").modal('show')
  } else {
    $("#loading-modal").modal('hide')
  }
}

$(document).ready(function () {
  $('.modal').on('shown.bs.modal', function (event) {
    $(event.target).css("display", "flex");
  });
});

function criaAlertaMensagem(mensagem, parent) {
  var parent = parent ? parent : '#alert-placeholder';
  $(parent).html('<div class="alert alert-danger alert-dismissable" style="margin: 0;"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><span>' + mensagem + '</span></div>')
  $(parent).fadeTo(5000, 1000).slideUp(1000, function () {
    $("#success-alert").alert('close');
  });
}

function criaAlertaInfo(mensagem, parent) {
    var parent = parent ? parent : '#alert-placeholder';
    $(parent).html('<div class="alert alert-info alert-dismissable" style="margin: 0;"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><span>' + mensagem + '</span></div>')
    $(parent).fadeTo(5000, 1000).slideUp(1000, function () {
        $("#success-alert").alert('close');
    });
}


function criaModalMensagem(titulo, mensagem, onClickYes, onClickNo) {
  ReactDOM.render(
    window.Modal(titulo, mensagem, onClickYes, onClickNo),
    document.getElementById('modal')
  )
  $("#modal").modal();
}

function logoff() {
    const form = document.querySelector('#logoutForm');
    form.submit();
}