using System;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using FluentValidation;

namespace DV.FrenteLoja.Core.Contratos.Validator
{
	public class OrcamentoValidator : AbstractValidator<Orcamento>
	{
		public OrcamentoValidator()
		{

			RuleFor(x => x.Cliente.MotivoBloqueioCredito).Must(VerificarSituacaoCliente).WithMessage(x => $"Cliente bloqueado. Motivo: {x.Cliente.MotivoBloqueioCredito.GetDescription()}");

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
