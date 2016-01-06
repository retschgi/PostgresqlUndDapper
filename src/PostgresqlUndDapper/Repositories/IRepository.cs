using System.Collections.Generic;

namespace PostgresqlUndDapper.Repositories
{
    public interface IRepository<TEntity, in TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
    {
        IEnumerable<TEntity> Get();

        TEntity Get(TPrimaryKey id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}
