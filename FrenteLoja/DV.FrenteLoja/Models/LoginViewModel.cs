using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DV.FrenteLoja.Models
{
	public class LoginViewModel
	{

		[Required(ErrorMessage = "Campo E-mail é obrigtório.")]
		[Display(Name = "Email")]
		public string Email
		{
			get { return UserName + "@dellavia.com.br"; }
		}

		[Required(ErrorMessage = "Por favor informe a senha.")]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Por favor informe o usuário.")]
        public string UserName { get; set; }
	}


}