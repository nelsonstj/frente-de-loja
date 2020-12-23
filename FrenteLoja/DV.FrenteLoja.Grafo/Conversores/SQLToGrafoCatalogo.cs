using DV.FrenteLoja.Grafo.Enumeradores;
using DV.FrenteLoja.Grafo.Infra;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DV.FrenteLoja.Core.Contratos.DataObjects;

namespace DV.FrenteLoja.Grafo.Conversores
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

        public void Importar(List<CatalogoDto> catalogo, GraphClient cliente)
        {

            for (int i = 0; i < 10000; i++)
            {
                foreach (var item in catalogo)
                {
                    //test hein tira essa poha
                    item.CodigoFabricante = RandomString(8);
                    //criar itens de filtro
                    CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.TipoVeiculo);
                    CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Marca);
                    CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Modelo);
                    CriarCypherLabel(cliente, item, CypherCreate.UniqueRoot, TypeNode.Versao);

                    //criar relacionamentos de itens de filtro
                    CriarCypherRelacionamentos(cliente, item, TypeNode.Modelo, TypeNode.Marca);
                    CriarCypherRelacionamentos(cliente, item, TypeNode.Modelo, TypeNode.Versao);

                    //criar produto
                    CriarCypherLabel(cliente, item, CypherCreate.Normal, TypeNode.Cadastro);

                    //criar relacionamentos de itens de produtos
                    CriarCypherRelacionamentos(cliente, item, TypeNode.Cadastro, TypeNode.TipoVeiculo);
                    CriarCypherRelacionamentos(cliente, item, TypeNode.Cadastro, TypeNode.Marca);
                    CriarCypherRelacionamentos(cliente, item, TypeNode.Cadastro, TypeNode.Modelo);
                    CriarCypherRelacionamentos(cliente, item, TypeNode.Cadastro, TypeNode.Versao);
                }
            }
        }




        #endregion
        /// <summary>
        /// Estrutura para ser chamada para criar Nós de cadastro no grapho
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="catalogo"></param>
        /// <param name="modo"></param>
        /// <param name="tipoNo"></param>
        #region Métodos de criação
        public void CriarCypherLabel(GraphClient cliente, CatalogoDto catalogo, CypherCreate modo, TypeNode tipoNo)
        {

            Debug.WriteLine("Iniciando Processo de cadastro");

            var node = Enum.GetName(typeof(TypeNode), tipoNo);

            switch (tipoNo)
            {
                case TypeNode.Cadastro:


                    //TODO: trocar para CatalogoDTO
                    var existsCadastro =
                        cliente.Cypher
                            .Match("(dado:" + node + " {CodigoFabricante:'" + catalogo.CodigoFabricante + "'})")
                            .Where((CatalogoDto dado) => dado.CodigoFabricante == catalogo.CodigoFabricante && dado.CodigoDellavia == catalogo.CodigoDellavia)
                            .Return(dado => dado.Node<CatalogoDto>())
                            .Results
                            .Any();

                    if (!existsCadastro)
                    {
                        cliente.Cypher
                            .Merge("(item:" + node + " { Id: {id} })")
                            .OnCreate()
                            .Set("item = {catalogo}")
                            .WithParams(new
                            {
                                id = catalogo.Id,
                                catalogo
                            })
                            .ExecuteWithoutResults();

                    }
                    break;

                case TypeNode.Marca:
                case TypeNode.TipoVeiculo:
                case TypeNode.Modelo:
                case TypeNode.Versao:
                default:

                    try
                    {
                        NestedRootClass root = RootResolver(tipoNo, catalogo);

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
                                       .Create("(" + catalogo.Descricao + ":" + node + " {Descricao:'" + catalogo.Descricao + "'})")
                                       .ExecuteWithoutResults();
                                    break;
                                case CypherCreate.Unique:

                                    cliente.Cypher
                                        .Merge("(item:" + node + " {descricao" + node + ": {descricao}})")
                                        .OnCreate()
                                        .Set("item = {catalogo}")
                                        .WithParams(new
                                        {
                                            id = catalogo.Id,
                                            catalogo
                                        })
                                        .ExecuteWithoutResults();
                                    break;
                                case CypherCreate.UniqueRoot:
                                    cliente.Cypher
                                        .Merge("(item:" + node + " {descricao"+node+": {descricao}})")
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
        private void CypherRelacionamento(GraphClient cliente, NestedRootClass root, CatalogoDto catalogo, TypeNode tipoNo, TypeNode tipoNoPai, string nomeRelacionamento)
        {

            if (catalogo.CodigoDellavia == null)
            {
                var exists = cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((Catalogo esquerda) => esquerda.CodigoFabricante == catalogo.CodigoFabricante)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<CatalogoDto>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((Catalogo esquerda) => esquerda.CodigoFabricante == catalogo.CodigoFabricante)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }
            else if (catalogo.CodigoFabricante == null)
            {
                var exists = cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((Catalogo esquerda) => esquerda.CodigoDellavia == catalogo.CodigoDellavia)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<CatalogoDto>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((Catalogo esquerda) => esquerda.CodigoDellavia == catalogo.CodigoDellavia)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }
            else
            {
                var exists = cliente.Cypher
                    .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + ")-[:" + nomeRelacionamento + "]-(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                    .Where((Catalogo esquerda) => esquerda.CodigoDellavia == catalogo.CodigoDellavia && esquerda.CodigoFabricante == catalogo.CodigoFabricante)
                    .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                    .Return(esquerda => esquerda.Node<CatalogoDto>())
                    .Results
                    .Any();

                if (!exists)
                    cliente.Cypher
                   .Match("(esquerda:" + Enum.GetName(typeof(TypeNode), tipoNo) + "),(direita:" + Enum.GetName(typeof(TypeNode), tipoNoPai) + ")")
                   .Where((Catalogo esquerda) => esquerda.CodigoDellavia == catalogo.CodigoDellavia && esquerda.CodigoFabricante == catalogo.CodigoFabricante)
                   .AndWhere((NestedRootClass direita) => direita.descricao == root.descricaoPai)
                   .Create("(esquerda)-[:" + nomeRelacionamento + "]->(direita)")
                   .ExecuteWithoutResults();
            }
                       

        }

        public void CriarCypherRelacionamentos(GraphClient cliente, CatalogoDto catalogo, TypeNode tipoNo, TypeNode tipoNoPai)
        {
            NestedRootClass root = RootRelationshipResolver(tipoNo,tipoNoPai, catalogo);
            try
            {
                string nomeRelacionamento = Enum.GetName(typeof(TypeNode), tipoNo).ToUpper() + "_" + Enum.GetName(typeof(TypeNode), tipoNoPai).ToUpper();
                if (tipoNo == TypeNode.Cadastro)
                {
                    CypherRelacionamento(cliente, root, catalogo, tipoNo, tipoNoPai, nomeRelacionamento);
                } else
                {
                    CypherRelacionamento(cliente, root, tipoNo, tipoNoPai, nomeRelacionamento);
                }

                if (tipoNo == TypeNode.Modelo)
                {
                    root = RootRelationshipResolver(tipoNo, TypeNode.TipoVeiculo, catalogo);
                    nomeRelacionamento = Enum.GetName(typeof(TypeNode), tipoNo).ToUpper() + "_" + Enum.GetName(typeof(TypeNode), TypeNode.TipoVeiculo).ToUpper();
                    CypherRelacionamento(cliente, root, tipoNo, TypeNode.TipoVeiculo, nomeRelacionamento);
                }

            }
            catch (CypherException ex)
            {
                throw new CypherException(ex);
            }
        }

        #endregion

        #region Métodos de Exclusão 

        private void DeleteCypherNode(GraphClient cliente, string node, string nodeToRelate, long id, string relationshipType)
        {
            try
            {
                cliente.Cypher
                    .OptionalMatch("(item:{relationshipType})<-[r]-()")
                    .Where((Catalogo catalogo) => catalogo.Id == id)
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

        private NestedRootClass RootResolver(TypeNode tipoNo, CatalogoDto catalogo)
        {

            switch (tipoNo)
            {
               
                case TypeNode.Marca:
                    return new NestedRootClass { descricao = catalogo.MarcaVeiculo};
                case TypeNode.Modelo:
                    return new NestedRootClass { descricao = catalogo.ModeloVeiculo, descricaoPai = catalogo.MarcaVeiculo };
                case TypeNode.Versao:
                    return new NestedRootClass { descricao = catalogo.VersaoVeiculo, descricaoPai = catalogo.ModeloVeiculo };
                case TypeNode.Cadastro:
                    return new NestedRootClass { descricao = catalogo.Descricao, descricaoPai = catalogo.VersaoVeiculo };
                case TypeNode.TipoVeiculo:
                    return new NestedRootClass { descricao = Enum.GetName(typeof(TipoVeiculo), catalogo.TipoVeiculo), descricaoPai = catalogo.MarcaVeiculo };
                default:
                    return null;




            }

        }

        private NestedRootClass RootRelationshipResolver(TypeNode tipoNo, TypeNode tipoNoPai, CatalogoDto catalogo)
        {

            NestedRootClass relacionamento = new NestedRootClass();

            switch (tipoNo)
            {

                case TypeNode.Marca:
                    relacionamento.descricao = catalogo.MarcaVeiculo;
                    break;
                case TypeNode.Modelo:
                    relacionamento.descricao = catalogo.ModeloVeiculo;
                    break;
                case TypeNode.Versao:
                    relacionamento.descricao = catalogo.VersaoVeiculo;
                    break;
                case TypeNode.Cadastro:
                    relacionamento.descricao = catalogo.Descricao;
                    break;
                case TypeNode.TipoVeiculo:
                    relacionamento.descricao = Enum.GetName(typeof(TipoVeiculo), catalogo.TipoVeiculo);
                    break;
                default:
                    return null;

            }

            switch (tipoNoPai)
            {

                case TypeNode.Marca:
                    relacionamento.descricaoPai = catalogo.MarcaVeiculo;
                    break;
                case TypeNode.Modelo:
                    relacionamento.descricaoPai = catalogo.ModeloVeiculo;
                    break;
                case TypeNode.Versao:
                    relacionamento.descricaoPai = catalogo.VersaoVeiculo;
                    break;
                case TypeNode.Cadastro:
                    relacionamento.descricaoPai = catalogo.Descricao;
                    break;
                case TypeNode.TipoVeiculo:
                    relacionamento.descricaoPai = Enum.GetName(typeof(TipoVeiculo), catalogo.TipoVeiculo);
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
