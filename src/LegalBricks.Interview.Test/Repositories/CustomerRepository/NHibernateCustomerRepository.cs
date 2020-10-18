using System;
using System.Threading.Tasks;
using NHibernate;
using DomainModels = LegalBricks.Interview.Test.Models;
using DatabaseModels = LegalBricks.Interview.Database;
using NHibernate.Linq;
using LegalBricks.Interview.Database;
using LegalBricks.Interview.Test.Exceptions;
using NHibernate.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace LegalBricks.Interview.Test.Repositories.CustomerRepository
{
    public class NHibernateCustomerRepository : ICustomerRepository
    {
        private readonly ISession _session;

        public NHibernateCustomerRepository(ISession session)
        {
            _session = session;
        }

        public async Task<DomainModels.Customer> CreateAsync(DomainModels.Customer newCustomer)
        {
            // probably automapper or something would be better here.
            // but the nhibernate models represent schema so definitely we want to break it out
            var dbEntity = new DatabaseModels.Customer
            {
                Id = newCustomer.Id,
                FirstName = newCustomer.FirstName,
                Surname = newCustomer.Surname,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email
            };

            using (var trans = _session.BeginTransaction())
            {
                try
                {
                    if (await _session.Query<DatabaseModels.Customer>()
                        .AnyAsync(c => c.Id == dbEntity.Id))
                        throw new DuplicateRecordException();

                    var result = await _session.SaveAsync(dbEntity);
                    trans.Commit();
                    newCustomer.Id = (int)result;
                    return newCustomer;
                }
                catch (Exception e)
                {
                    trans.Rollback();

                    if (e is GenericADOException && e.Message.Contains("Email"))
                        throw new DuplicateRecordException();

                    throw;
                }
            }
        }

        public async Task<IEnumerable<DomainModels.Customer>> GetAsync()
        {
            return await _session.Query<DatabaseModels.Customer>()
                .Select(c => new DomainModels.Customer
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    Surname = c.Surname,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                })
                  .ToListAsync();
        }

        public async Task<DomainModels.Customer> GetAsync(int id)
        {
            return await _session.Query<DatabaseModels.Customer>()
                .Select(c => new DomainModels.Customer
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    Surname = c.Surname,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                })
                  .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
