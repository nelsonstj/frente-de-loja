using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace DV.FrenteLoja.Core.Exceptions
{
	public class NegocioException: Exception
	{
		public NegocioException(string message)
			: base(message)
		{
		}

		public NegocioException(string message, IEnumerable<ValidationFailure> errors) : base(message = message + " " + FormataErrosValidacao(errors))
		{
		}

		private static string FormataErrosValidacao(IEnumerable<ValidationFailure> errors)
		{
			string retorno = null;
			foreach (var item in errors)
			{
				retorno += item.ErrorMessage + "\n";
			}
			return retorno;
		}

		public NegocioException(string message, Exception e)
			: base(message, e)
		{
		}
	}
}