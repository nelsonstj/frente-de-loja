const Card = ({ ProdutoDescricao, ProdutoCodDellavia, onAbrirModalEstoque, estoque, Estoque, ImgProduto, ImgGrupoProduto, numeroOrcamento, valor, Valor, ProdutoFabricantePeca, ProdutoCodFabricante }) => {
    let errorImg = true;
    return (
        <div className={ProdutoCodDellavia ? 'cartao' : 'cartao text-danger'}>
            <div className="cartao-img-container">
                <object data={ImgProduto} type="image/png" className="cartao-img-container">
                    <img className="cartao-img-top" src={ImgGrupoProduto} onError={(e) => { if (errorImg) { errorImg = false; e.target.src = '/Content/img/produto-sem-imagem.gif'; } }} />
                </object>
            </div>
            <div className="cartao-body botoes">
                <div className="produto-descricao">
                    <b>{ProdutoDescricao} - {ProdutoFabricantePeca} ({ProdutoCodDellavia})</b>
                </div>
                <div className="cartao-informacao botoes">
                    <b> {ProdutoCodDellavia ? (Valor ? Valor : 'R$0,00') : 'R$0,00'} </b>
                    {ProdutoCodDellavia ? (Estoque ? (Estoque > 0 ? <p>{Estoque} itens</p> : <p>{Estoque} item</p>) : '0') : '0'}
                </div>
                <div className="botoes">
                    <a href="#" className="botao detalhes" onClick={() => abrirModalDetalhes(ProdutoCodDellavia, numeroOrcamento)}>Detalhes</a>
                    <a href="#" className="botao estoque" onClick={() => onAbrirModalEstoque(ProdutoCodDellavia)}>Estoque</a>
                </div>
            </div>
        </div>
    )
};

const abrirModalEstoque = (codDellavia) => {
    var loja = $("#LojaDestino").val();
    if (codDellavia) {
        setVisibilidadeLoading(true);
        $.ajax({
            type: "POST",
            url: "/Orcamento/AbrirModalEstoque/",
            data: { codDellavia: codDellavia, lojaDestino: loja }
        }).done(function (response) {
            $("#estoque").modal();
            $("#estoque").html(response);
            setVisibilidadeLoading(false);
        }).fail(function (response) {
            criaAlertaMensagem(response.responseJSON);
            setVisibilidadeLoading(false);
        });
    }
};

const abrirModalDetalhes = (CodDellavia, numeroOrcamento) => {
    if (CodDellavia) {
        setVisibilidadeLoading(true);
        $.ajax({
            type: "POST",
            url: "/Orcamento/AbrirModalDetalhes/",
            data: { campoCodigo: CodDellavia, numeroOrcamento: numeroOrcamento }
        }).done(function (response) {
            $("#relacionado-grid").html(response);
            $("#relacionado-grid").modal();
            setVisibilidadeLoading(false);
            criaMultiplicadores();
        }).fail(function (response) {
            criaAlertaMensagem(response.responseJSON);
            setVisibilidadeLoading(false);
        });
    }
};

const Main = ({ itens, numeroOrcamento, setVisibilidadeLoading }) => (
    <div className='cartoes'>
        {itens.map((item, index) => <Card key={index} {...item} onAbrirModalEstoque={abrirModalEstoque} numeroOrcamento={numeroOrcamento} />)}
    </div>
);

window.Main = Main;