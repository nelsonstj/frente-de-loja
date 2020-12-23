var enviarEmailFinalizacao = '/Orcamento/EnviarPorEmail';
var enviarProtheus = '/Orcamento/EnviarProtheus';

function EnviarEmailFinalizacao() {
    generatePdfFinalizacao('email');
    $('#emailModal').modal('hide');
}

function generatePdfFinalizacao(tipo) {
    setVisibilidadeLoading(true);
    var orcamentoId = $("#Id").val();
    var email = $("#EmailCliente").val();
    document.getElementById('orcamentoEmailFinalizacao').style.display = 'block';

    //if (!$("#orcamentoAnexado").is(":checked")) {
    if (tipo == "email") {
        var quotes = document.getElementById('orcamentoEmailFinalizacao');
        $.ajax({
            url: enviarEmailFinalizacao,
            data: JSON.stringify({ orcamentoId: orcamentoId, orcamento: quotes.innerHTML, emailCliente: email, orcamentoAnexado: false }),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8"
        }).done(function (response) {
            criaAlertaInfo("Enviado com sucesso!");
            setVisibilidadeLoading(false);
        }).fail(function (response) {
            criaAlertaMensagem(response.responseJSON);
            setVisibilidadeLoading(false);
        });
    } else {
        var a4 = [595.28, 841.89]; // Widht e Height de uma folha a4
        $("#orcamentoEmailFinalizacao").width((a4[0] * 1.33333) - 120).css('max-width', 'none');
        var quotes = document.getElementById('part1');

        var numeroPaginas = quotes.clientHeight / 1050;
        var _numeroPaginasParaCima = Math.ceil((quotes.clientHeight + document.getElementById('part2').clientHeight) / 1040);
        var _tamanhoPagina = 0;
        var quebra = false;
        for (var i = 0; i <= _numeroPaginasParaCima; i++) {
            if (_numeroPaginasParaCima > 1) {
                if (i == _numeroPaginasParaCima - 1) {
                    var tamanhoPart1 = $("#part1").height();
                    if (tamanhoPart1 > _tamanhoPagina) {
                        if ((tamanhoPart1 - _tamanhoPagina) + $("#part2").height() > 1040) {
                            quebra = true;
                            break;
                        }
                    } else {
                        if (tamanhoPart1 + $("#part2").height() > 1040) {
                            quebra = true;
                            break;
                        }
                    }
                }
            }
            _tamanhoPagina += 1050;
        }
        var ua = navigator.userAgent.toLowerCase();
        var Browser = {};
        Browser.edge = ua.indexOf('edge') > -1;
        Browser.safari = !Browser.edge && ua.indexOf('safari') > -1;
        Browser.opera = !Browser.edge && ua.indexOf('opera') > -1;
        Browser.chrome = !Browser.edge && ua.indexOf('chrome') > -1;
        Browser.firefox = ua.indexOf('firefox') > -1;
        Browser.ie = !Browser.chrome && !Browser.firefox && !Browser.opera && !Browser.edge;
        delete ua;
        if (quebra) {
            html2canvas(quotes, {
                useCORS: true,
                onrendered: function (canvas) {
                    var pdf = new jsPDF('p', 'pt', 'a4');

                    var tamanhoPagina = 0;
                    for (var i = 0; i <= numeroPaginas; i++) {
                        //! This is all just html2canvas stuff
                        var srcImg = canvas;
                        var sX = 0;
                        var page = 1120;
                        if (i > 0)
                            page = 1050;
                        var sY = page * i; // start 980 pixels down for every new page
                        var sWidth = $("#part1").width();
                        var sHeight = 1120;
                        var dX = 50;
                        var dY = 70;
                        var dWidth = $("#part1").width();
                        var dHeight = 1120;
                        if ((Browser.ie || Browser.edge) && $("#part1").height() < 1120) {
                            sHeight = $("#part1").height();
                            dHeight = $("#part1").height() + 200;
                            if (dHeight > 1120) {
                                dHeight = 1120;
                            }
                        }
                        window.onePageCanvas = document.createElement("canvas");
                        onePageCanvas.setAttribute('width', 778);
                        onePageCanvas.setAttribute('height', 1120);
                        tamanhoPagina += 1050;
                        var ctx = onePageCanvas.getContext('2d');
                        // details on this usage of this function:
                        // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Using_images#Slicing
                        ctx.imageSmoothingEnabled = false;
                        ctx.webkitImageSmoothingEnabled = false;
                        ctx.msImageSmoothingEnabled = false;
                        ctx.imageSmoothingEnabled = false;
                        ctx.drawImage(srcImg, sX, sY, sWidth, sHeight, dX, dY, dWidth, dHeight);

                        var canvasDataURL = onePageCanvas.toDataURL("image/png", 1.0);
                        var width = onePageCanvas.width;
                        var height = onePageCanvas.clientHeight;

                        //! If we're on anything other than the first page,
                        // add another page
                        if (i > 0) {
                            pdf.addPage(595, 842); //8.5" x 11" in pts (in*72)
                        }
                        //! now we declare that we're working on that page
                        pdf.setPage(i + 1);
                        //! now we add content to that page!
                        pdf.addImage(canvasDataURL, 'PNG', 0, 0, (width * .72), (sHeight * .71));
                        var quebraPagina = false;

                        if (_numeroPaginasParaCima > 1) {
                            if (i == _numeroPaginasParaCima - 2) {
                                var tamanhoPart1 = $("#part1").height();
                                var addPart2 = false;
                                if (tamanhoPart1 > _tamanhoPagina) {
                                    if ((tamanhoPart1 - _tamanhoPagina) + $("#part2").height() > 1040) {
                                        addPart2 = true;
                                    }
                                } else {
                                    if (tamanhoPart1 + $("#part2").height() > 1040) {
                                        addPart2 = true;
                                    }
                                }
                                if (addPart2) {
                                    html2canvas(document.getElementById('part2'), {
                                        allowTaint: true,
                                        onrendered: function (canvas) {
                                            var ctx = canvas.getContext('2d');
                                            var imgData = canvas.toDataURL("image/png", 1.0);
                                            var htmlH = $(".page2").height() + 100;
                                            var width = canvas.width;
                                            var height = canvas.clientHeight;
                                            pdf.addPage(595, 842);
                                            pdf.addImage(imgData, 'PNG', 50, 70, (width * .72), (height * .71));
                                            quebraPagina = true;
                                        }
                                    });
                                }
                            }
                        }
                    }
                    //! after the for loop is finished running, we save the pdf.
                    setTimeout(function () {
                        if (tipo == "email") {
                            var binary = pdf.output();
                            var reqData = binary ? btoa(binary) : "";
                            $.ajax({
                                url: enviarEmailFinalizacao,
                                data: JSON.stringify({ orcamentoId: orcamentoId, orcamento: reqData, emailCliente: email, orcamentoAnexado: true }),
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8"
                            }).done(function (response) {
                                criaAlertaInfo("Enviado com sucesso!");
                                setVisibilidadeLoading(false);
                            }).fail(function (response) {
                                criaAlertaMensagem(response.responseJSON);
                                setVisibilidadeLoading(false);
                            });
                        } else {
                            pdf.save('Orçamento-' + orcamentoId + '-DELLAVIA.pdf');
                            setVisibilidadeLoading(false);
                        }
                    }, 2000);
                }
            });
        } else {
            quotes = document.getElementById('orcamentoEmailFinalizacao');
            html2canvas(quotes, {
                useCORS: true,
                onrendered: function (canvas) {
                    var pdf = new jsPDF('p', 'pt', 'a4');
                    var tamanhoPagina = 0;
                    for (var i = 0; i <= numeroPaginas; i++) {
                        //! This is all just html2canvas stuff
                        var srcImg = canvas;
                        var sX = 0;
                        var page = 1120;
                        if (i > 0)
                            page = 1050;
                        var sY = page * i; // start 980 pixels down for every new page
                        var sWidth = $("#orcamentoEmailFinalizacao").width();
                        var sHeight = 1120;
                        var dX = 50;
                        var dY = 70;
                        var dWidth = $("#orcamentoEmailFinalizacao").width();
                        var dHeight = 1120;

                        if ($("#orcamentoEmailFinalizacao").height() < 1120) {
                            if (Browser.ie) {
                                sHeight = $("#orcamentoEmailFinalizacao").height() + 27;
                                dHeight = $("#orcamentoEmailFinalizacao").height();
                            }
                            if (Browser.edge) {
                                sHeight = $("#orcamentoEmailFinalizacao").height();
                                dHeight = $("#orcamentoEmailFinalizacao").height();
                            }
                        }
                        window.onePageCanvas = document.createElement("canvas");
                        onePageCanvas.setAttribute('width', 778);
                        onePageCanvas.setAttribute('height', 1120);
                        tamanhoPagina += 1050;
                        var ctx = onePageCanvas.getContext('2d');
                        // details on this usage of this function:
                        // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Using_images#Slicing
                        ctx.webkitImageSmoothingEnabled = false;
                        ctx.mozImageSmoothingEnabled = false;
                        ctx.imageSmoothingEnabled = false;

                        ctx.drawImage(srcImg, sX, sY, sWidth, sHeight, dX, dY, dWidth, dHeight);

                        var canvasDataURL = onePageCanvas.toDataURL("image/png", 1.0);
                        var width = onePageCanvas.width;
                        var height = onePageCanvas.clientHeight;

                        //! If we're on anything other than the first page,
                        // add another page
                        if (i > 0) {
                            pdf.addPage(595, 842); //8.5" x 11" in pts (in*72)
                        }
                        //! now we declare that we're working on that page
                        pdf.setPage(i + 1);

                        //! now we add content to that page!
                        pdf.addImage(canvasDataURL, 'PNG', 0, 0, (width * .72), (sHeight * .71));
                    }
                    //! after the for loop is finished running, we save the pdf.
                    setTimeout(function () {
                        if (tipo == "email") {
                            var binary = pdf.output();
                            var reqData = binary ? btoa(binary) : "";
                            $.ajax({
                                url: enviarEmailFinalizacao,
                                data: JSON.stringify({ orcamentoId: orcamentoId, orcamento: reqData, emailCliente: email, orcamentoAnexado: true }),
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                            }).done(function (response) {
                                criaAlertaInfo("Enviado com sucesso!");
                                setVisibilidadeLoading(false);
                            }).fail(function (response) {
                                criaAlertaMensagem(response.responseJSON);
                                setVisibilidadeLoading(false);
                            });
                        } else {
                            pdf.save('Orçamento-' + orcamentoId + '-DELLAVIA.pdf');
                            setVisibilidadeLoading(false);
                        }
                    }, 2000);
                }
            });
        }
    }
    document.getElementById('orcamentoEmailFinalizacao').style.display = 'none';
}

function abreModalEnvioProtheus() {
    $('#modal-protheus').modal();
}

function enviarOrcamentoProtheus() {
    $(".modal-feedback").empty();
    $("#modal-protheus-body").show();
    var orcamentoId = $("#Id").val();
    setVisibilidadeLoading(true);
    $('#modal-protheus').css('z-index', 1030);
    $.ajax({
        type: "POST",
        url: enviarProtheus,
        data: { id: orcamentoId }
    }).done(function () {
        //if (response == true) {
        window.location = "/Orcamento/Finalizacao/" + orcamentoId;
        //} else {
        //    $(".modal-feedback").append("<b>Orçamento enviado com sucesso!</b>");
        //    setVisibilidadeLoading(false);
        //    $('#modal-protheus').css('z-index', "");
        //}
    }).fail(function (response) {
        $(".modal-feedback").append("<p class='principal'>Falha ao enviar o orçamento para o Protheus!</p>");
        $(".modal-feedback").append("<p class='cinza'>Entre em contato com o suporte, informando o erro a seguir.</p>");
        $(".modal-feedback").append("<p class='mensagem-erro'>" + response.responseJSON + "</p>");
        setVisibilidadeLoading(false);
        $('#modal-protheus').css('z-index', "");
    });
}
