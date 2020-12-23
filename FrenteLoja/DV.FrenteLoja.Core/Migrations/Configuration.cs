namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<DV.FrenteLoja.Core.Infra.EntityFramework.DellaviaContexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DV.FrenteLoja.Core.Infra.EntityFramework.DellaviaContexto context)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var baseDir = Path.GetFullPath(Path.Combine(path, "../../../sql_scripts/sql_script.sql"));

            context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir));
        }
    }
}
