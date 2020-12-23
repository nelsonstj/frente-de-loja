using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Infra.Security;
using DV.FrenteLoja.Core.Util;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace DV.FrenteLoja.Util
{
    public class EnvioEmail
    {
        public static void EnviarEmail(string emailDestinatario, string assunto, string mensagemHtml, Attachment anexo)
        {
            try
            {
                mensagemHtml = mensagemHtml.Replace("/Content/img/logo.png", FiltroHelper.UrlLogoDellaVia());
                bool bValidaEmail = ValidaEnderecoEmail(emailDestinatario);
                if (bValidaEmail == false)
                    throw new NegocioException("E-mail destinatário é inválido: " + emailDestinatario);
                //"[0]imap-mail.outlook.com;[1]587;[2]t.dev1@dellavia.com.br;[3]Dellavia@2019;[4]orcamento@dellavia.com.br"
                var dadosEmail = System.Configuration.ConfigurationManager.AppSettings["DadosEnvioEmail"].Split(';');
                string host = dadosEmail[0];
                int port = Convert.ToInt32(dadosEmail[1]);
                string username = dadosEmail[2];
                string password = dadosEmail[3];
                string from = dadosEmail[4];
                /*
                if (HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK)
                {
                    from = HttpContext.Current.User.Identity.GetEmailOperador().ToLower().Replace("@mail.com", "@dellavia.com.br");
                    if (string.IsNullOrEmpty(from))
                        from = dadosEmail[4];
                    //throw new NegocioException("O operador não possui e-mail cadastrado.");
                }*/
                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(username, password);
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(from);
                    message.To.Add(emailDestinatario);
                    message.Subject = assunto;
                    message.Body = mensagemHtml;
                    message.IsBodyHtml = true;
                    if (anexo != null)
                        message.Attachments.Add(anexo);
                    smtp.Send(message);
                }
            }
            catch (NegocioException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                //define a expressão regular para validar o email
                string texto_Validar = enderecoEmail;
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                if (expressaoRegex.IsMatch(texto_Validar))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}