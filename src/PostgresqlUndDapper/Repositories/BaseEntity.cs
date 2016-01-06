using System.ComponentModel.DataAnnotations;

namespace PostgresqlUndDapper.Repositories
{
    public abstract class BaseEntity<TPrimaryKey> 
    {
        [Key]
        public TPrimaryKey Id { get; set; }
    }
}
