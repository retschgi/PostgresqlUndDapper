using PostgresqlUndDapper.Repositories;
using PostgresqlUndDapper.Models;

namespace PostgresqlUndDapper.RepositoriesPostgresql
{
    public class EmployeeRepository : AbstractPostgresqlRepository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(string connectionString) : base(connectionString)
        {

        }

        protected override string TableName
        {
            get
            {
                return "Employees";
            }
        }
    }
}
