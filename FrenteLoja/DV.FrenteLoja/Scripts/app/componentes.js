'use strict';

function criaSelect2(config) {
  var ajax = {
    quietMillis: 300,
    url: config.endpoint,
    data: config.data,
    results: config.processaResult
  };

  return $(config.selector).select2({
    placeholder: "selecione",
    minimumInputLength: config.minimumInputLength,
    allowClear: true,
    ajax: config.endpoint ? ajax : undefined,
    minimumResultsForSearch: config.minimumResultsForSearch
  });
};

function select2configFactory(selector, endpoint, data, processaResult, minimumResultsForSearch, minimumInputLength) {
  minimumResultsForSearch = minimumResultsForSearch != undefined ? minimumResultsForSearch : 0;
  return {
    selector: selector,
    endpoint: endpoint,
    data: data,
    processaResult: processaResult,
    minimumResultsForSearch: minimumResultsForSearch,
    minimumInputLength: minimumInputLength ? minimumInputLength : 0
  }
}

function criaKendoGrid(seletor, columns, data, fields, elasticSearch) {
  var dataSource = {
    pageSize: 25,
    data: data,
    schema: {
      model: {
        fields: fields
      }
    }
  };

  if (elasticSearch) {
    dataSource = new kendo.data.ElasticSearchDataSource(dataSource);
  }

  return $(seletor).kendoGrid({
    dataSource: dataSource,
    sortable: false,
    pageable: true,
    filterable: false,
    columns: columns
  });
}

jQuery.fn.Multiplier = function (pai) {
  var $input = this;
  var $parent = document.getElementById(pai);
  $input.change(function (event) {
    var target = this.getAttribute('data-target');
    var valorTarget = this.getAttribute('valor-total');
    var multiplicador = event.target.value;

    var textoValor = $parent[valorTarget].value;
    var valorTotal = multiplicador * parseFloat(textoValor.replace(',', '.'));
    $parent.getElementsByTagName('span')[target].textContent = "R$" + valorTotal.toFixed(2).replace('.', ',');
  });

  return this;
}

jQuery.fn.QuantityButtons = function ($options) {
  var $input = this;
  var inputId = $input[0].id;
  var minus = $('<div class="btn-minus" data-input-action="-1" data-input-box="#' + inputId + '">-</div>');
  var plus = $('<div class="btn-plus" data-input-action="1" data-input-box="#' + inputId + '">+</div>');
  $input.after(plus);

  var calculate = function (e) {
    var $target = jQuery(this.getAttribute('data-input-box'));
    var quantity = parseInt($target.val()) + parseInt(this.getAttribute('data-input-action'));
    quantity = (quantity >= 0) ? quantity : 0;
    $target.val(quantity);
    e.stopPropagation();
  }

  jQuery(minus).on('click', calculate);
  jQuery(plus).on('click', calculate);

  return this;

};

function requestTermoController(term, page) {
  return {
    tamanhoPagina: pageSize,
    numeroPagina: page,
    termoBusca: term
  };
}

function setFormDisable(formId) {
  document.querySelectorAll(formId + " input, select, textarea").forEach(function (item) {
    item.disabled = true;
    });    
}

function selectDefaultEvents(selects) {
  selects.forEach(function (item) {
    item.on("select2-selected", function (event) {
      var id = $(item).attr('id');
      $("#" + id + "Descricao").val(event.choice.text);
    });
  });
}