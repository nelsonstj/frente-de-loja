using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Infra.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CheckupServico : ICheckupServico<CheckupDto>
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<CheckupCar> _checkupCarRepositorio;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;
        private readonly IRepositorio<Checkup> _checkupRepositorio;

        public CheckupServico(IRepositorioEscopo escopo)
        {
            _escopo = escopo;
            _checkupCarRepositorio = _escopo.GetRepositorio<CheckupCar>();
            _orcamentoRepositorio = _escopo.GetRepositorio<Orcamento>();
            _vendedorRepositorio = _escopo.GetRepositorio<Vendedor>();
            _checkupRepositorio = _escopo.GetRepositorio<Checkup>();
        }

        public CheckupDto ObterPorId(long? id)
        {
            var lojas = HttpContext.Current.User.Identity.GetLojaPadrao();
            var checkupCar = _checkupRepositorio.GetSingle(x => x.Id == id);
            if (checkupCar == null)
                throw new NegocioException("Checkup não encontrado.");
            if (!lojas.Contains(checkupCar.Orcamento.IdLojaDellaVia))
                throw new NegocioException("Filial não tem permissão para acessar esse checkup");

            return Mapper.Map<CheckupDto>(checkupCar);
        }

        public List<CheckupDto> ObterCheckupsCar()
        {
            return ObterCheckupsQuery().ProjectTo<CheckupDto>().ToList();
        }

        private IQueryable<Checkup> ObterCheckupsQuery()
        {
            var lojas = HttpContext.Current.User.Identity.GetLojaPadrao();
            return from checkupcar in _checkupRepositorio.GetAll()
                   where lojas.Contains(checkupcar.Orcamento.IdLojaDellaVia)
                   && checkupcar.IsCheckupCar
                   orderby checkupcar.Id
                   select checkupcar;
        }

        public List<CheckupDto> ObterCheckupsTruck()
        {
            return ObterCheckupsTruckQuery().ProjectTo<CheckupDto>().ToList();
        }

        private IQueryable<Checkup> ObterCheckupsTruckQuery()
        {
            var lojas = HttpContext.Current.User.Identity.GetLojaPadrao();
            return from checkup in _checkupRepositorio.GetAll()
                   where lojas.Contains(checkup.Orcamento.IdLojaDellaVia)
                   && !checkup.IsCheckupCar
                   orderby checkup.Id
                   select checkup;
        }


        public List<CheckupDto> ObterCheckupsNomeCliente(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCheckupsQuery(termoBusca, "NOMECLIENTE")
                .ProjectTo<CheckupDto>()
                .ToList();
        }

        public List<CheckupDto> ObterCheckupsCPF(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCheckupsQuery(termoBusca, "CPFCNPJ")
                .ProjectTo<CheckupDto>()
                .ToList();
        }

        public List<CheckupDto> ObterCheckupsCNPJ(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCheckupsQuery(termoBusca, "CPFCNPJ")
                .ProjectTo<CheckupDto>()
                .ToList();
        }

        public List<CheckupDto> ObterCheckupsPlaca(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCheckupsQuery(termoBusca, "PLACA")
                .ProjectTo<CheckupDto>()
                .ToList();
        }
        public List<CheckupDto> ObterCheckupsVeiculo(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCheckupsQuery(termoBusca, "VEICULO")
                .ProjectTo<CheckupDto>()
                .ToList();
        }

        public List<CheckupDto> ObterCheckupsIdOrcamento(string termoBusca)
        {
            termoBusca = termoBusca.ToLower();
            return ObterCheckupsIdOrcamentoQuery(Int64.Parse(termoBusca))
                .ProjectTo<CheckupDto>()
                .ToList();
        }

        public List<CheckupDto> ObterCheckupsUsuario()
        {
            return ObterCheckupsQuery(string.Empty, string.Empty)
                .OrderByDescending(a => a.Id)
                .Take(100)
                .ProjectTo<CheckupDto>()
                .ToList();
        }

        private IQueryable<Checkup> ObterCheckupsIdOrcamentoQuery(long termoBusca)
        {
            return
               from checkup in _checkupRepositorio.GetAll()
               where checkup.Orcamento.Id == termoBusca
               orderby checkup.Id
               select checkup;
        }


        private IQueryable<Checkup> ObterCheckupsQuery(string termoBusca, string tipoBusca)
        {
            var lojas = HttpContext.Current.User.Identity.GetLojaPadrao();
            switch (tipoBusca)
            {
                case "NOMECLIENTE":
                    {
                        termoBusca = string.Format("{0}%", termoBusca).ToLower();
                        return from checkup in _checkupRepositorio.GetAll()
                               where lojas.Contains(checkup.Orcamento.IdLojaDellaVia)
                               && DbFunctions.Like(checkup.Orcamento.Cliente.Nome, termoBusca)
                               orderby checkup.Id
                               select checkup;
                    }
                case "CPFCNPJ":
                    {
                        return from checkup in _checkupRepositorio.GetAll()
                               where lojas.Contains(checkup.Orcamento.IdLojaDellaVia)
                               && checkup.Orcamento.Cliente.CNPJCPF.Equals(termoBusca, StringComparison.InvariantCultureIgnoreCase)
                               orderby checkup.Id
                               select checkup;
                    }
                case "PLACA":
                    {
                        return from checkup in _checkupRepositorio.GetAll()
                               where lojas.Contains(checkup.Orcamento.IdLojaDellaVia)
                               && checkup.Orcamento.Placa.Equals(termoBusca, StringComparison.InvariantCultureIgnoreCase)
                               orderby checkup.Id
                               select checkup;
                    }
                case "VEICULO":
                    {
                        termoBusca = string.Format("{0}%", termoBusca).ToLower();
                        return from checkup in _checkupRepositorio.GetAll()
                               where lojas.Contains(checkup.Orcamento.IdLojaDellaVia)
                               //&& (DbFunctions.Like(checkup.Orcamento.MarcaModeloVersao.MarcaModelo.Marca.Descricao, termoBusca)
                               //|| DbFunctions.Like(checkup.Orcamento.MarcaModeloVersao.MarcaModelo.Descricao, termoBusca))
                               && (DbFunctions.Like("", termoBusca)
                               || DbFunctions.Like("", termoBusca))
                               orderby checkup.Id
                               select checkup;
                    }
                default:
                    {
                        return from checkup in _checkupRepositorio.GetAll()
                               where lojas.Contains(checkup.Orcamento.IdLojaDellaVia)
                               orderby checkup.Id
                               select checkup;
                    }
            }

        }

        public long Cadastrar(CheckupDto entidadeDto)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CheckupDto> Obter()
        {
            throw new NotImplementedException();
        }

        public void Atualizar(CheckupDto entidadeDto)
        {
            throw new NotImplementedException();
        }

        public void Excluir(long id)
        {
            try
            {
                _checkupRepositorio.Remove(a => a.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _escopo.Finalizar();
            }
        }
    }
}
