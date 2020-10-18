using LegalBricks.Interview.Database;

namespace LegalBrikes.Interview.Test.UnitTests.Repositories
{
    public interface IRepositoryFactory
    {
        TEntity Add<TEntity>(TEntity data) where TEntity : IEntity;

        T Create<T>();
    }
}
