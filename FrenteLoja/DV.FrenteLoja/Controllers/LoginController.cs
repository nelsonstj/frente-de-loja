using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.ProtheusAPI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DV.FrenteLoja.Models;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;

namespace DV.FrenteLoja.Controllers
{
    //[Authorize]
    public class LoginController : Controller
    {
        private readonly DellaviaContexto _contexto;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IRepositorio<LojaDellaVia> _lojaRepositorio;

        public LoginController(DellaviaContexto contexto)
        {
            _contexto = contexto;
            _lojaRepositorio = _contexto.GetRepository<LojaDellaVia>();
        }

        protected LoginController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login", "Login");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.UserName = model.UserName.ToUpper();
            if (model.UserName.Equals("ADMINPORTAL"))
            {
                if (WebConfigurationManager.AppSettings["passwordAdmin"] != model.Password)
                {
                    ModelState.AddModelError("", "A senha está incorreta");
                    return View(model);
                }

                var retorno = await this.AdminLoginAsync(model);
                if (string.IsNullOrEmpty(retorno))
                    return Redirect(Url.Action("Index", "Home"));
                else
                {
                    ModelState.AddModelError("", retorno);
                    return View(model);
                }
            }
            var loginProtheusApi = new LoginApi();
            JObject resultProtheusLogin;
            // Chamada do Protheus para verificar dados de login
            try
            {
                resultProtheusLogin = await loginProtheusApi.LoginUsuario(model.UserName, model.Password);
            }
            catch (Exception exception)
            {
                var msg = exception?.InnerException?.Message ?? string.Empty;
                if (msg.Contains("-"))
                    msg = msg.Split('-')[0];
                msg = msg.Replace("invalid", "inválid").Replace("suari","suári").Replace(" nao ", " não ");
                ModelState.AddModelError("", msg);
                return View(model);
            }
            // Uma vez recuperado dados do protheus, verificar se já existe esse usuário na nossa base
            var user = await UserManager.FindByEmailAsync(model.Email);

            // Se não existir devemos cadastrá-lo
            if (user == null)
            {
                var campoCod = resultProtheusLogin["Filial"].ToString();
                var idsLoja = string.Empty;
                for (int i = 0; i < campoCod.Split(',').Length; i++)
                {
                    if (i != 0)
                        idsLoja += ",";
                    string idLoja = campoCod.Split(',')[i];
                    idsLoja += _lojaRepositorio.GetSingle(loja => loja.CampoCodigo == idLoja).Id;
                }
                // var idLoja = _lojaRepositorio.GetSingle(loja => loja.CampoCodigo == campoCod).Id;
                var usuario = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = resultProtheusLogin["Nome"].ToString(),
                    PerfilAcesso = Convert.ToInt32(resultProtheusLogin["Perfil"].ToString()),
                    FilialId = idsLoja,
                    ConvenioPadrao = resultProtheusLogin["ConvenioPadrao"].ToString(),
                    IdOperador = resultProtheusLogin["IdOperador"].ToString(),
                    TipoVenda = resultProtheusLogin["TipoVenda"].ToString(),
                    LojaPadraoCampoCodigo = campoCod
                };
                var resultIdentity = await UserManager.CreateAsync(usuario, model.Password);
                if (resultIdentity.Succeeded)
                    user = await UserManager.FindByEmailAsync(model.Email);
                else
                {
                    AddErrors(resultIdentity);
                    return View(model);
                }
            }
            try
            {
                if (resultProtheusLogin != null && user != null && (!user.ConvenioPadrao.Equals(resultProtheusLogin["ConvenioPadrao"].ToString())))
                    AtualizaConvenioPadrao(resultProtheusLogin["ConvenioPadrao"].ToString(), user);

                //if (resultProtheusLogin != null && user != null && (!user.Email.Equals(resultProtheusLogin["Email"].ToString())))
                //    AtualizaEmail(resultProtheusLogin["Email"].ToString(), user);

                SignInStatus result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                if (result != SignInStatus.Success)
                {
                    if (resultProtheusLogin != null && user != null && (model.Email.Equals(user.Email)))
                    {
                        AtualizaSenha(model.Password, user);
                        result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                    }
                }
                switch (result)
                {
                    case SignInStatus.Success:
                        return Redirect(Url.Action("Index", "Home"));
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        private void AtualizaSenha(string password, ApplicationUser user)
        {
            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, password);
        }

        private void AtualizaConvenioPadrao(string convenioProtheus, ApplicationUser user)
        {
            user.ConvenioPadrao = convenioProtheus;
            UserManager.Update(user);
        }

        private void AtualizaEmail(string emailProtheus, ApplicationUser user)
        {
            user.Email = emailProtheus;
            UserManager.Update(user);
        }

        private async Task<string> AdminLoginAsync(LoginViewModel model)
        {
            try
            {
                if (UserManager.FindByEmail("admin@admin") == null)
                {
                    ApplicationUser newUser = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Name = "Administrador",
                        PerfilAcesso = 3,
                        Id = Guid.NewGuid().ToString(),
                        Email = "admin@admin",

                    };
                    var resultIdentity = await UserManager.CreateAsync(newUser, model.Password);
                }
                var user = UserManager.FindByEmail("admin@admin");
                SignInStatus status = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                return string.Empty;
            }
            catch (Exception e)
            {
                return "Erro ao logar com o ADM: " + e.Message + " " + e.InnerException?.Message;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {}

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                    properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}