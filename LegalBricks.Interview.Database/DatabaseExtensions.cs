using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace LegalBricks.Interview.Database
{
    public static class DatabaseExtensions
    {
        public static void AddDataAccess(
            this IServiceCollection services
        )
        {
            var configuration = new NHibernate.Cfg.Configuration();

            // will scan for mappings 
            var fluentConfig = Fluently.Configure()
               .Database(SQLiteConfiguration.Standard.InMemory())
               .Mappings(m => m.FluentMappings
                   .AddFromAssemblyOf<Customer>()
                   .Conventions
                   .AddFromAssemblyOf<Customer>())
                   .ExposeConfiguration(cfg => configuration = cfg);

            var sessionFactory = fluentConfig.BuildSessionFactory();

            services.AddSingleton<ISessionFactory>(sessionFactory);

            services.AddScoped<ISession>(provider =>
                provider.GetService<ISessionFactory>()
                .OpenSession()
            );

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var session = scope.ServiceProvider.GetService<ISession>();
                new SchemaExport(configuration).Execute(false, true, false, session.Connection, null);
            }
        }
    }
}
