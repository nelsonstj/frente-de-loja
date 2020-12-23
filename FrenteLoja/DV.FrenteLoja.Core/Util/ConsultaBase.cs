using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Util
{
    class ConsultaBase
    {
        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

        public static long getIdVersaoMotor(IRepositorio<VersaoMotor> versaoMotorRepositorio, string descricaoVersaoMotor)
        {
            VersaoMotor v = (from versaoMotor in versaoMotorRepositorio.GetAll()
                             where versaoMotor.Descricao == descricaoVersaoMotor
                             select versaoMotor)
                             .FirstOrDefault();
            if (v != null)
            {
                return v.Id;
            }
            else
            {
                VersaoMotor versaoMotor = new VersaoMotor()
                {
                    Descricao = descricaoVersaoMotor
                };
                return versaoMotorRepositorio.Add(versaoMotor).Id;
            }
        }

        public static long getIdMarcaModeloVersao(IRepositorio<MarcaModeloVersao> marcaModeloVersaoRepositorio, long idModelo, string descricaoVersao)
        {
            MarcaModeloVersao v = (from marcaModeloVersao in marcaModeloVersaoRepositorio.GetAll()
                                    where marcaModeloVersao.IdMarcaModelo == idModelo
                                        && marcaModeloVersao.Descricao == descricaoVersao
                                    select marcaModeloVersao)
                                    .FirstOrDefault();
            if (v != null)
            {
                return v.Id;
            }
            else
            {
                MarcaModeloVersao marcaModeloVersao = new MarcaModeloVersao()
                {
                    IdMarcaModelo = idModelo,
                    Descricao = descricaoVersao,
                    RegistroInativo = false,
                    DataAtualizacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local),
                    UsuarioAtualizacao = UsuarioAtualizacaoServico
                };
                return marcaModeloVersaoRepositorio.Add(marcaModeloVersao).Id;
            }
        }

        public static long getIdModelo(IRepositorio<MarcaModelo> marcaModeloRepositorio, long idMarca, string descricaoModelo)
        {
            MarcaModelo m = (from marcaModelo in marcaModeloRepositorio.GetAll()
                             where marcaModelo.IdMarca == idMarca
                                && marcaModelo.Descricao == descricaoModelo
                             select marcaModelo)
                             .FirstOrDefault();
            if (m != null)
            {
                return m.Id;
            }
            else
            {
                MarcaModelo marcaModelo = new MarcaModelo()
                {
                    IdMarca = idMarca,
                    Descricao = descricaoModelo,
                    DataAtualizacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local),
                    UsuarioAtualizacao = UsuarioAtualizacaoServico
                };
                return marcaModeloRepositorio.Add(marcaModelo).Id;
            }
        }

        public static long getIdMarca(IRepositorio<Marca> marcaRepositorio, string descricaoMarca)
        {
            var errosBuilder = new StringBuilder();
            Marca m = (from marca in marcaRepositorio.GetAll()
                       where marca.Descricao == descricaoMarca
                       select marca)
                       .FirstOrDefault();
            if (m != null)
            {
                return m.Id;
            }
            else
            {
                Marca marca = new Marca()
                {
                    Descricao = descricaoMarca,
                    RegistroInativo = false,
                    DataAtualizacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local),
                    UsuarioAtualizacao = "UsuarioAtualizacaoServico"
                };
                return marcaRepositorio.Add(marca).Id;
            }
        }
    }
}
