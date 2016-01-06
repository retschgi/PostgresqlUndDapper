using PostgresqlUndDapper.Repositories;

namespace PostgresqlUndDapper.Models
{
    public class Employee : BaseEntity<int>
    {
        public string Name { get; set; }

        public int YearOfContract { get; set; }
    }
}
