using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CheckupCarServico<CheckupCarDto> : ICheckupCarServico<CheckupCarDto>
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<CheckupCar> _checkupCarRepositorio;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;
        private readonly IRepositorio<Checkup> _checkupRepositorio;

        public CheckupCarServico(IRepositorioEscopo escopo)
        {
            _escopo = escopo;
            _checkupCarRepositorio = _escopo.GetRepositorio<CheckupCar>();
            _orcamentoRepositorio = _escopo.GetRepositorio<Orcamento>();
            _vendedorRepositorio = _escopo.GetRepositorio<Vendedor>();
            _checkupRepositorio = _escopo.GetRepositorio<Checkup>();
        }

        public void Atualizar(CheckupCarDto entidadeDto)
        {
            try
            {
                var checkupCar = Mapper.Map<CheckupCar>(entidadeDto);
                Checkup check = _checkupRepositorio.GetSingle(a => a.Id == checkupCar.CheckupId);
                check.Orcamento = _orcamentoRepositorio.GetSingle(a => a.Id == checkupCar.Checkup.OrcamentoId);
                check.Vendedor = _vendedorRepositorio.GetSingle(a => a.Id == checkupCar.Checkup.VendedorId);
                check.TecnicoResponsavel = _vendedorRepositorio.GetSingle(a => a.Id == checkupCar.Checkup.TecnicoResponsavelId);
                check.IsCheckupCar = true;
                check.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
                checkupCar.Checkup = check;
                
                checkupCar.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();

                _checkupCarRepositorio.Update(checkupCar);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _escopo.Finalizar();
            }
        }

        public long Cadastrar(CheckupCarDto entidadeDto)
        {
            try
            {
                CheckupCar checkupCar = Mapper.Map<CheckupCar>(entidadeDto);
                checkupCar.Checkup.IsCheckupCar = true;
                checkupCar.Checkup.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
                checkupCar.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
                checkupCar = _checkupCarRepositorio.Add(checkupCar);                

                return checkupCar.CheckupId;
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


        public void Excluir(long id)
        {
            try
            {
                _checkupCarRepositorio.Remove(a => a.Id == id);
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

        public IQueryable<CheckupCarDto> Obter()
        {
            throw new NotImplementedException();
            // return _checkupCarRepositorio.GetAll();
        }

        public List<CheckupCarDto> ObterCheckupsCar()
        {
            var _checkupCars = _checkupCarRepositorio.GetAll();
            return Mapper.Map<List<CheckupCarDto>>(_checkupCars);
        }

        public CheckupCarDto ObterPorId(long? id)
        {
            var checkupCar = _checkupCarRepositorio.GetSingle(x => x.CheckupId == id);
            if (checkupCar == null)
                throw new Exception("Checkup não encontrado.");

            return Mapper.Map<CheckupCarDto>(checkupCar);
        }

        public int TamanhoModelosPorTermo(string termoBusca)
        {
            return ObterEspecificacaoQuery(termoBusca).Count();
        }

        public List<string> ObterEspecificacao(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            termoBusca = termoBusca.ToLower();

            return ObterEspecificacaoQuery(termoBusca)
                  .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ToList();
        }
        private List<string> ObterEspecificacaoQuery(string termoBusca)
        {
            List<string> especificacoes = new List<string>();
            especificacoes.AddRange(new[]
            {
                 "5W30",
                 "5W40",
                 "10W40",
                 "15W40",
                 "20W50"
            });
            termoBusca = string.Format("{0}%", termoBusca).ToLower();
            especificacoes.AddRange((from checkupcar in _checkupCarRepositorio.GetAll()
                                     where DbFunctions.Like(checkupcar.Especificacao, termoBusca) 
                                     orderby checkupcar.Especificacao
                                     select checkupcar.Especificacao.ToUpper()));

            return especificacoes.Distinct().OrderBy(a => a).ToList();
        }

    }
}
