using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DV.FrenteLoja.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
	    public string TipoVenda { get; set; }

	    public string Name { get; set; }
        public string Email { get; set; }
        public int PerfilAcesso { get; set; }
	    public string FilialId { get; set; }
		public string ConvenioPadrao { get; set; }
	    public string IdOperador { get; set; }
	    public string LojaPadraoCampoCodigo { get; set; }

	    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here => this.OrganizationId is a value stored in database against the user
            
            if(this.UserName.Equals("ADMINPORTAL", StringComparison.InvariantCultureIgnoreCase))
            {
                userIdentity.AddClaim(new Claim("Name", this.Name));
                userIdentity.AddClaim(new Claim("Email", this.Email));
                userIdentity.AddClaim(new Claim("PerfilAcesso", this.PerfilAcesso.ToString()));
                return userIdentity;
            }

	        if (this.Name != null)
	        {
		        userIdentity.AddClaim(new Claim("Name", this.Name));
                userIdentity.AddClaim(new Claim("Email", this.Email));
                userIdentity.AddClaim(new Claim("PerfilAcesso", this.PerfilAcesso.ToString()));
		        userIdentity.AddClaim(new Claim("FilialId", this.FilialId));
				userIdentity.AddClaim(new Claim("TipoVenda", this.TipoVenda));
		        userIdentity.AddClaim(new Claim("ConvenioPadrao", this.ConvenioPadrao));
		        userIdentity.AddClaim(new Claim("IdOperador", this.IdOperador));
		        userIdentity.AddClaim(new Claim("LojaPadraoCampoCodigo", this.LojaPadraoCampoCodigo));
			}

	        return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext():base("name=DellaviaContexto")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);
} 
    }
}