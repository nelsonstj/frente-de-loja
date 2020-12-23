using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Grafo.Enumerator;
using DV.FrenteLoja.Core.Grafo.Exception;
using DV.FrenteLoja.Core.Grafo.Infra;
using Neo4jClient;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Grafo.Conversores
{
    public class SQLToGrafoCatalogo
    {

        #region Métodos de ajuda
        private static string RandomString(int Size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public void Importar(List<ElasticsearchItemDto> elasticsearchItemDtos, GraphClient cliente, OrigemCarga origemCarga)
        {

            foreach (var item in elasticsearchItemDtos)
            {
                try
                {
                    //criar itens de filtro

                    /* CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Marca);
                     CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Modelo);
                     CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Versao);
                     CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.AnoInicial);
                     CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.AnoFinal);
                     CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.ProdutoCodGrupo);
                     CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Descricao);

                     //criar relacionamentos de itens de filtro
                     CriarCypherRelacionamentos(cliente, item, TypeNode.Modelo, TypeNode.Marca);
                     CriarCypherRelacionamentos(cliente, item, TypeNode.Modelo, TypeNode.Versao);*/

                    //criar produto
                    CriarCypherLabel(cliente, item, CypherCreate.Normal, TypeNode.ProdutoCadastro, origemCarga);

                    //criar relacionamentos de itens de produtos
                    /* CriarCypherRelacionamentos(cliente, item, TypeNode.ProdutoCadastro, TypeNode.Marca);
                     CriarCypherRelacionamentos(cliente, item, TypeNode.ProdutoCadastro, TypeNode.Modelo);
                     CriarCypherRelacionamentos(cliente, item, TypeNode.ProdutoCadastro, TypeNode.Versao);*/
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
        }


        #endregion
        /// <summary>
        /// Estrutura para ser chamada para criar Nós de cadastro no grapho
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="elasticsearchItemDto"></param>
        /// <param name="modo"></param>
        /// <param name="tipoNo"></param>
        /// <param name="OrigemCarga"></param>
        #region Métodos de criação
        public void CriarCypherLabel(GraphClient cliente, ElasticsearchItemDto elasticsearchItemDto, CypherCreate modo, TypeNode tipoNo, OrigemCarga origemCarga)
        {

            Debug.WriteLine("Iniciando Processo de cadastro");

            var node = Enum.GetName(typeof(TypeNode), tipoNo);

            switch (tipoNo)
            {
                case TypeNode.ProdutoCadastro:
                    if (origemCarga == OrigemCarga.Catalogo)
                    {
                        var existsCadastro = cliente.Cypher
                                                .Match("(dado:" + node + " {ProdutoCodFabricante:'" + elasticsearchItemDto.ProdutoCodFabricante + "'})")
                                                .Where((ElasticsearchItemDto dado) => dado.ProdutoCodFabricante == elasticsearchItemDto.ProdutoCodFabricante
                                                                                    && dado.VeiculoMarca == elasticsearchItemDto.VeiculoMarca
                                                                                    && dado.VeiculoModelo == elasticsearchItemDto.VeiculoModelo
                                                                                    && dado.VeiculoVersao == elasticsearchItemDto.VeiculoVersao
                                                                                    && dado.VeiculoAnoInicial == elasticsearchItemDto.VeiculoAnoInicial
                                                                                    && dado.VeiculoAnoFinal == elasticsearchItemDto.VeiculoAnoFinal
                                                                                    && dado.VersaoMotor == elasticsearchItemDto.VersaoMotor)
                                                .Return(dado => dado.Node<ElasticsearchItemDto>())
                                                .Results
                                                .Any();

                        if (!existsCadastro)
                        {
                            try
                            {
                                cliente.Cypher
                               .Merge("(item:" + node + " { Id: {id} })")
                               .OnCreate()
                               .Set("item = {catalogo}")
                               .WithParams(new
                               {
                                   id = elasticsearchItemDto.ProdutoCodFabricante,
                                   catalogo = elasticsearchItemDto
                               })
                               .ExecuteWithoutResults();
                            }
                            catch (System.Exception e)
                            {
                                throw e;
                            }


                        }
                        else
                        {
                            try
                            {
                                cliente.Cypher
                                  .Match("(dado:" + node + " {ProdutoCodFabricante:'" + elasticsearchItemDto.ProdutoCodFabricante + "'})")
                                  .Where((ElasticsearchItemDto dado) => dado.ProdutoCodFabricante == elasticsearchItemDto.ProdutoCodFabricante
                                                                     && dado.VeiculoMarca == elasticsearchItemDto.VeiculoMarca
                                                                     && dado.VeiculoModelo == elasticsearchItemDto.VeiculoModelo
                                                                     && dado.VeiculoVersao == elasticsearchItemDto.VeiculoVersao
                                                                     && dado.VeiculoAnoInicial == elasticsearchItemDto.VeiculoAnoInicial
                                                                     && dado.VeiculoAnoFinal == elasticsearchItemDto.VeiculoAnoFinal
                                                                     && dado.VersaoMotor == elasticsearchItemDto.VersaoMotor)
                                  .Set("dado.ProdutoDescricao = {produtoDescricao} , dado.ProdutoCodDellavia = {produtoCodDellavia} , dado.ProdutoInformacaoComplementar = {produtoInformacaoComplementar} , dado.ProdutoFabricantePeca = {produtoFabricantePeca} , dado.ProdutoCodGrupo = {produtoCodGrupo}, dado.PrioridadeOrdenacao = {prioridadeOrdenacao}, dado.VersaoMotor = {VersaoMotor}")
                                  .WithParams(new
                                  {
                                      produtoDescricao = elasticsearchItemDto.ProdutoDescricao,
                                      produtoCodDellavia = elasticsearchItemDto.ProdutoCodDellavia,
                                      produtoInformacaoComplementar = elasticsearchItemDto.ProdutoInformacaoComplementar,
                                      produtoFabricantePeca = elasticsearchItemDto.ProdutoFabricantePeca,
                                      produtoCodGrupo = elasticsearchItemDto.ProdutoCodGrupo,
                                      prioridadeOrdenacao = elasticsearchItemDto.PrioridadeOrdenacao,
                                      VersaoMotor = elasticsearchItemDto.VersaoMotor
                                  })
                                  .ExecuteWithoutResults();
                            }
                            catch (System.Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                    else
                    {
                        var existsCadastro = cliente.Cypher
                                               .Match("(dado:" + node + " {ProdutoCodDellavia:'" + elasticsearchItemDto.ProdutoCodDellavia + "'})")
                                               .Where((ElasticsearchItemDto dado) => dado.ProdutoCodDellavia == elasticsearchItemDto.ProdutoCodDellavia
                                                                                   && dado.VeiculoMarca == elasticsearchItemDto.VeiculoMarca
                                                                                   && dado.VeiculoModelo == elasticsearchItemDto.VeiculoModelo
                                                                                   && dado.VeiculoVersao == elasticsearchItemDto.VeiculoVersao
                                                                                   && dado.VeiculoAnoInicial == elasticsearchItemDto.VeiculoAnoInicial
                                                                                   && dado.VeiculoAnoFinal == elasticsearchItemDto.VeiculoAnoFinal
                                                                                   && dado.VersaoMotor == elasticsearchItemDto.VersaoMotor)
                                               .Return(dado => dado.Node<ElasticsearchItemDto>())
                                               .Results
                                               .Any();

                        if (!existsCadastro)
                        {
                            try
                            {
                                cliente.Cypher
                               .Merge("(item:" + node + " { Id: {id} })")
                               .OnCreate()
                               .Set("item = {catalogo}")
                               .WithParams(new
                               {
                                   id = elasticsearchItemDto.ProdutoCodDellavia,
                                   catalogo = elasticsearchItemDto
                               })
                               .ExecuteWithoutResults();
                            }
                            catch (System.Exception e)
                            {
                                throw e;
                            }


                        }
                        else
                        {
                            try
                            {
                                cliente.Cypher
                                  .Match("(dado:" + node + " {ProdutoCodDellavia:'" + elasticsearchItemDto.ProdutoCodDellavia + "'})")
                                  .Where((ElasticsearchItemDto dado) => dado.ProdutoCodDellavia == elasticsearchItemDto.ProdutoCodDellavia
                                                                     && dado.VeiculoMarca == elasticsearchItemDto.VeiculoMarca
                                                                     && dado.VeiculoModelo == elasticsearchItemDto.VeiculoModelo
                                                                     && dado.VeiculoVersao == elasticsearchItemDto.VeiculoVersao
                                                                     && dado.VeiculoAnoInicial == elasticsearchItemDto.VeiculoAnoInicial
                                                                     && dado.VeiculoAnoFinal == elasticsearchItemDto.VeiculoAnoFinal
                                                                     && dado.VersaoMotor == elasticsearchItemDto.VersaoMotor)
                                  .Set("dado.ProdutoDescricao = {produtoDescricao} , dado.ProdutoCodFabricante = {produtoCodFabricante} , dado.ProdutoInformacaoComplementar = {produtoInformacaoComplementar} , dado.ProdutoFabricantePeca = {produtoFabricantePeca} , dado.ProdutoCodGrupo = {produtoCodGrupo}, dado.PrioridadeOrdenacao = {prioridadeOrdenacao}, dado.VersaoMotor = {VersaoMotor}")
                                  .WithParams(new
                                  {
                                      produtoDescricao = elasticsearchItemDto.ProdutoDescricao,
                                      produtoCodFabricante = elasticsearchItemDto.ProdutoCodFabricante,
                                      produtoInformacaoComplementar = elasticsearchItemDto.ProdutoInformacaoComplementar,
                                      produtoFabricantePeca = elasticsearchItemDto.ProdutoFabricantePeca,
                                      produtoCodGrupo = elasticsearchItemDto.ProdutoCodGrupo,
                                      prioridadeOrdenacao = elasticsearchItemDto.PrioridadeOrdenacao,
                                      VersaoMotor = elasticsearchItemDto.VersaoMotor
                                  })
                                  .ExecuteWithoutResults();
                            }
                            catch (System.Exception e)
                            {
                                throw e;
                            }
                        }
                    }

                    break;

                case TypeNode.Marca:
                case TypeNode.Modelo:
                case TypeNode.Versao:
                case TypeNode.AnoInicial:
                case TypeNode.AnoFinal:
                default:

                    try
                    {
                        NestedRootClass root = RootResolver(tipoNo, elasticsearchItemDto);

                        var exists =
                        cliente.Cypher
                            .Match("(dado:" + node + " {descricao:'" + root.descricao + "'})")
                            .Where((NestedRootClass dado) => dado.descricao == root.descricao)
                            .Return(dado => dado.Node<NestedRootClass>())
                            .Results
                            .Any();

                        if (!exists)
                            switch (modo)
                            {
                                case CypherCreate.Normal:

                                    cliente.Cypher
                                       .Create("(" + elasticsearchItemDto.ProdutoDescricao + ":" + node + " {ProdutoDescricao:'" + elasticsearchItemDto.ProdutoDescricao + "'})")
                                       .ExecuteWithoutResults();
                                    break;
                                case CypherCreate.Unique:

                                    cliente.Cypher
                                        .Merge("(item:" + node + " {descricao" + node + ": {descricao}})")
                                        .OnCreate()
                                        .Set("item = {catalogo}")
                                        .WithParams(new
                                        {
                                            catalogo = elasticsearchItemDto
                                        })
                                        .ExecuteWithoutResults();
                                    break;
                                case CypherCreate.UniqueRoot:
                                    cliente.Cypher
                                        .Merge("(item:" + node + " {descricao" + node + ": {descricao}})")
                                        .OnCreate()
                                        .Set("item = {root}")
                                        .WithParams(new
                                        {
                                            descricao = root.descricao,
                                            root
                                        })
                                        .ExecuteWithoutResults();
                                    break;
                            }
                    }
                    catch (CypherException ex)
                    {
                        throw new CypherException(ex);
                    }

                    break;
            }



        }

        #endregion

        #region Métodos de Atualização

        /// <summary>
        /// Geração de relacionamentos pelo Cypher
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="root"></param>
        /// <param name="tipoNo"></param>
        /// <param name="tipoNoPai"></param>
        /// <param name="nomeRelacionamento"></param>
        private void CypherRelacionamento(GraphClient cliente, NestedRootClass root, TypeNode tipoNo, TypeNode tipoNoPai, string nomeRelacionamento, bool inverso = false)
        {

            if (inverso)
            {

                var exists =
                cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((NestedRootClass esquerda) => esquerda.descricao == root.descricaoPai)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricao)
                    .Return(esquerda => esquerda.Node<NestedRootClass>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((NestedRootClass esquerda) => esquerda.descricao == root.descricaoPai)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricao)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }
            else
            {

                var exists =
                cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((NestedRootClass esquerda) => esquerda.descricao == root.descricao)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<NestedRootClass>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((NestedRootClass esquerda) => esquerda.descricao == root.descricao)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }


        }


        /// <summary>
        /// Geração de relacionamentos pelo Cypher
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="root"></param>
        /// <param name="tipoNo"></param>
        /// <param name="tipoNoPai"></param>
        /// <param name="nomeRelacionamento"></param>
        private void CypherRelacionamento(GraphClient cliente, NestedRootClass root, ElasticsearchItemDto elasticsearchItemDto, TypeNode tipoNo, TypeNode tipoNoPai, string nomeRelacionamento)
        {

            if (elasticsearchItemDto.ProdutoCodDellavia == null)
            {
                var exists = cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((ElasticsearchItemDto esquerda) => esquerda.ProdutoCodFabricante == elasticsearchItemDto.ProdutoCodFabricante)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<CatalogoDto>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((ElasticsearchItemDto esquerda) => esquerda.ProdutoCodFabricante == elasticsearchItemDto.ProdutoCodFabricante)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }
            else if (elasticsearchItemDto.ProdutoCodFabricante == null)
            {
                var exists = cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((ElasticsearchItemDto esquerda) => esquerda.ProdutoCodDellavia == elasticsearchItemDto.ProdutoCodDellavia)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<ElasticsearchItemDto>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((ElasticsearchItemDto esquerda) => esquerda.ProdutoCodDellavia == elasticsearchItemDto.ProdutoCodDellavia)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }
            else
            {
                var exists = cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((ElasticsearchItemDto esquerda) => esquerda.ProdutoCodDellavia == elasticsearchItemDto.ProdutoCodDellavia && esquerda.ProdutoCodFabricante == elasticsearchItemDto.ProdutoCodFabricante)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<ElasticsearchItemDto>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((ElasticsearchItemDto esquerda) => esquerda.ProdutoCodDellavia == elasticsearchItemDto.ProdutoCodDellavia && esquerda.ProdutoCodFabricante == elasticsearchItemDto.ProdutoCodFabricante)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }


        }

        public void CriarCypherRelacionamentos(GraphClient cliente, ElasticsearchItemDto elasticsearchItemDto, TypeNode tipoNo, TypeNode tipoNoPai)
        {
            NestedRootClass root = RootRelationshipResolver(tipoNo, tipoNoPai, elasticsearchItemDto);
            try
            {
                string nomeRelacionamento = Enum.GetName(typeof(TypeNode), tipoNo).ToUpper() + "_" + Enum.GetName(typeof(TypeNode), tipoNoPai).ToUpper();
                if (tipoNo == TypeNode.ProdutoCadastro)
                {
                    CypherRelacionamento(cliente, root, elasticsearchItemDto, tipoNo, tipoNoPai, nomeRelacionamento);
                }
                else
                {
                    CypherRelacionamento(cliente, root, tipoNo, tipoNoPai, nomeRelacionamento);
                }

                //if (tipoNo == TypeNode.Modelo)
                //{
                //    root = RootRelationshipResolver(tipoNo, TypeNode.Marca, elasticsearchItemDto);
                //    nomeRelacionamento = Enum.GetName(typeof(TypeNode), tipoNo).ToUpper() + "_" + Enum.GetName(typeof(TypeNode), TypeNode.Marca).ToUpper();
                //    CypherRelacionamento(cliente, root, tipoNo, TypeNode.Modelo, nomeRelacionamento);
                //}

            }
            catch (CypherException ex)
            {
                throw new CypherException(ex);
            }
        }

        #endregion

        #region Métodos de Exclusão 

        private void DeleteCypherNode(GraphClient cliente, string node, string nodeToRelate, string codDellavia, string codFabricante, string relationshipType)
        {
            try
            {
                cliente.Cypher
                    .OptionalMatch("(item:{relationshipType})<-[r]-()")
                    .Where((ElasticsearchItemDto catalogo) => catalogo.ProdutoCodDellavia == codDellavia && catalogo.ProdutoCodFabricante == codFabricante)
                    .Delete("r, item")
                    .ExecuteWithoutResults();
            }
            catch (CypherException ex)
            {
                throw new CypherException(ex);
            }
        }

        #endregion


        #region helpers

        private NestedRootClass RootResolver(TypeNode tipoNo, ElasticsearchItemDto elasticsearchItemDto)
        {

            switch (tipoNo)
            {

                case TypeNode.Marca:
                    return new NestedRootClass { descricao = elasticsearchItemDto.VeiculoMarca };
                case TypeNode.Modelo:
                    return new NestedRootClass { descricao = elasticsearchItemDto.VeiculoModelo, descricaoPai = elasticsearchItemDto.VeiculoMarca };
                case TypeNode.Versao:
                    return new NestedRootClass { descricao = elasticsearchItemDto.VeiculoVersao, descricaoPai = elasticsearchItemDto.VeiculoModelo };
                case TypeNode.AnoInicial:
                    return new NestedRootClass { descricao = elasticsearchItemDto.VeiculoAnoInicial.ToString(), descricaoPai = elasticsearchItemDto.VeiculoAnoInicial.ToString() };
                case TypeNode.ProdutoCodGrupo:
                    return new NestedRootClass { descricao = elasticsearchItemDto.ProdutoCodGrupo.ToString(), descricaoPai = elasticsearchItemDto.ProdutoCodGrupo.ToString() };
                case TypeNode.Descricao:
                    return new NestedRootClass { descricao = elasticsearchItemDto.ProdutoDescricao.ToString(), descricaoPai = elasticsearchItemDto.ProdutoDescricao.ToString() };
                case TypeNode.AnoFinal:
                    return new NestedRootClass { descricao = elasticsearchItemDto.VeiculoAnoFinal.ToString(), descricaoPai = elasticsearchItemDto.VeiculoAnoFinal.ToString() };
                case TypeNode.ProdutoCadastro:
                    return new NestedRootClass { descricao = elasticsearchItemDto.ProdutoDescricao, descricaoPai = elasticsearchItemDto.VeiculoVersao };
                case TypeNode.VersaoMotor:
                    return new NestedRootClass { descricao = elasticsearchItemDto.VersaoMotor, descricaoPai = elasticsearchItemDto.VeiculoVersao };
                default:
                    return null;
            }

        }

        private NestedRootClass RootRelationshipResolver(TypeNode tipoNo, TypeNode tipoNoPai, ElasticsearchItemDto elasticsearchItemDto)
        {

            NestedRootClass relacionamento = new NestedRootClass();

            switch (tipoNo)
            {

                case TypeNode.Marca:
                    relacionamento.descricao = elasticsearchItemDto.VeiculoMarca;
                    break;
                case TypeNode.Modelo:
                    relacionamento.descricao = elasticsearchItemDto.VeiculoModelo;
                    break;
                case TypeNode.Versao:
                    relacionamento.descricao = elasticsearchItemDto.VeiculoVersao;
                    break;
                case TypeNode.ProdutoCadastro:
                    relacionamento.descricao = elasticsearchItemDto.ProdutoDescricao;
                    break;
                case TypeNode.VersaoMotor:
                    relacionamento.descricao = elasticsearchItemDto.VersaoMotor;
                    break;
                default:
                    return null;

            }

            switch (tipoNoPai)
            {

                case TypeNode.Marca:
                    relacionamento.descricaoPai = elasticsearchItemDto.VeiculoMarca;
                    break;
                case TypeNode.Modelo:
                    relacionamento.descricaoPai = elasticsearchItemDto.VeiculoModelo;
                    break;
                case TypeNode.Versao:
                    relacionamento.descricaoPai = elasticsearchItemDto.VeiculoVersao;
                    break;
                case TypeNode.ProdutoCadastro:
                    relacionamento.descricaoPai = elasticsearchItemDto.ProdutoDescricao;
                    break;
                case TypeNode.VersaoMotor:
                    relacionamento.descricaoPai = elasticsearchItemDto.VersaoMotor;
                    break;
                default:
                    return null;

            }


            return relacionamento;


        }

        #endregion
    }

    public class NestedRootClass : IEntidadeRoot
    {
        public string descricao { get; set; }
        public string descricaoPai { get; set; }

    }
}
