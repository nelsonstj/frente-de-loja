'use strict';


var processaDescricaoSigla = function (result, page) {
    var data = $.map(result.data, function (obj) {
        return {
            text: obj.text || obj.Descricao + " " + obj.Sigla,
            id: obj.id || obj.Id
        };
    });
    return processa(data, result.count, page);
};
