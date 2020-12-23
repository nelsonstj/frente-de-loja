using AutoMapper;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Linq;
using System.Web;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CheckupTruckServico<CheckupTruckDto> : ICheckupTruckServico<CheckupTruckDto>
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<CheckupTruck> _checkupTruckRepositorio;
        private readonly IRepositorio<Checkup> _checkupRepositorio;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<Vendedor> _vendedorRepositorio;

        public CheckupTruckServico(IRepositorioEscopo escopo)
        {
            _escopo = escopo;
            _checkupTruckRepositorio = _escopo.GetRepositorio<CheckupTruck>();
            _checkupRepositorio = _escopo.GetRepositorio<Checkup>();
            _orcamentoRepositorio = _escopo.GetRepositorio<Orcamento>();
            _vendedorRepositorio = _escopo.GetRepositorio<Vendedor>();
        }

        public void Atualizar(CheckupTruckDto entidadeDto)
        {
            try
            {
                var checkupTruck = Mapper.Map<CheckupTruck>(entidadeDto);
                Checkup check = _checkupRepositorio.GetSingle(a => a.Id == checkupTruck.CheckupId);
                check.Orcamento = _orcamentoRepositorio.GetSingle(a => a.Id == checkupTruck.Checkup.OrcamentoId);
                check.Vendedor = _vendedorRepositorio.GetSingle(a => a.Id == checkupTruck.Checkup.VendedorId);
                check.TecnicoResponsavel = _vendedorRepositorio.GetSingle(a => a.Id == checkupTruck.Checkup.TecnicoResponsavelId);
                checkupTruck.Checkup = check;

                checkupTruck.Checkup.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
                checkupTruck.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();

                _checkupTruckRepositorio.Update(checkupTruck);
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

        public long Cadastrar(CheckupTruckDto entidadeDto)
        {
            try
            {
                var checkupTruck = Mapper.Map<CheckupTruck>(entidadeDto);
                checkupTruck.Checkup.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();
                checkupTruck.UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper();

                return _checkupTruckRepositorio.Add(checkupTruck).CheckupId;
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
                _checkupTruckRepositorio.Remove(a => a.Id == id);
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

        public IQueryable<CheckupTruckDto> Obter()
        {
            var _checkups = _checkupTruckRepositorio.GetAll();
            return Mapper.Map<IQueryable<CheckupTruck>, IQueryable<CheckupTruckDto>>(_checkups);
        }
              
        public CheckupTruckDto ObterPorId(long? id)
        {
            var checkupTruck = _checkupTruckRepositorio.GetSingle(x => x.CheckupId == id);
            if (checkupTruck == null)
                throw new Exception("Checkup não encontrado.");

            return Mapper.Map<CheckupTruckDto>(checkupTruck);
        }
    }
}
