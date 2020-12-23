using System;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using FluentValidation;

namespace DV.FrenteLoja.Core.Contratos.Validator
{
    public class ClienteValidator : AbstractValidator<ClienteDto>
    {
        
        public ClienteValidator()
        {
            RuleFor(x => x.MotivoBloqueioCredito).Must(VerificarSituacaoCliente).WithMessage(x => $"Cliente bloqueado. Motivo: {x.MotivoBloqueioCredito.GetDescription()}");
        }

        private bool VerificarSituacaoCliente(StatusCreditoCliente status)
        {
            switch (status)
            {
                case StatusCreditoCliente.Liberado:
                    return true;
                default:
                    return false;
            }
        }
    }
}