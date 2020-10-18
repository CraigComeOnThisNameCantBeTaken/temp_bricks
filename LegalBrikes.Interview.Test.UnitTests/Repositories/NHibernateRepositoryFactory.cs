using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LegalBricks.Interview.Database;
using LegalBricks.Interview.Test.Repositories.CustomerRepository;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace LegalBrikes.Interview.Test.UnitTests.Repositories
{
    public class NHibernateRepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceCollection _services;

        public readonly ISession Session;

        public NHibernateRepositoryFactory()
        {
            _services = new ServiceCollection();
            var configuration = new NHibernate.Cfg.Configuration();

            // NOTE THIS IS DUPLICATE LOGIC FROM THE DATA ACCESS PROJ EXTENSION
            // ONLY BECAUSE THE IN-MEMORY DB IS DELETED WHEN SCOPE CLOSES
            // BETTER DRY SOLUTION WOULD BE WANTED
            var fluentConfig = Fluently.Configure()
               .Database(SQLiteConfiguration.Standard.InMemory())
               .Mappings(m => m.FluentMappings
                   .AddFromAssemblyOf<Customer>()
                   .Conventions
                   .AddFromAssemblyOf<Customer>())
                   .ExposeConfiguration(cfg => configuration = cfg);

            var sessionFactory = fluentConfig.BuildSessionFactory();

            _services.AddSingleton<ISessionFactory>(sessionFactory);
            _services.AddScoped<ISession>(provider =>
                provider.GetService<ISessionFactory>()
                .OpenSession()
            );

            var sp = _services.BuildServiceProvider();
            var scope = sp.CreateScope();

            Session = scope.ServiceProvider.GetRequiredService<ISession>();
            new SchemaExport(configuration).Execute(false, true, false, Session.Connection, null);
        }

        public TEntity Add<TEntity>(TEntity data) where TEntity : IEntity
        {
            data.Id = (int)Session.Save(data);
            return data;
        }

        public T Create<T>()
        {
            var temp = typeof(T);
            // this should make use of the service collection to resolve...
            if (typeof(NHibernateCustomerRepository).Name == temp.Name)
                return (T)(object)new NHibernateCustomerRepository(Session);

            throw new NotImplementedException();
        }
    }
}
