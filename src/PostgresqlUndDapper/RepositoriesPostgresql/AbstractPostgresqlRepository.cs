using Dapper;
using Npgsql;
using System.Collections.Generic;
using PostgresqlUndDapper.Repositories;
using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace PostgresqlUndDapper.RepositoriesPostgresql
{
    public abstract class AbstractPostgresqlRepository<TEntity,TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
    {
        private string _connectionString;

        public AbstractPostgresqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TEntity> Get()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = string.Format("SELECT * FROM {0}", TableName);
                return connection.Query<TEntity>(query);
            }
        }

        public TEntity Get(TPrimaryKey id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = string.Format("SELECT * FROM {0} WHERE Id = @Id LIMIT 1", TableName);
                return connection.Query<TEntity>(query, new { Id = id }).First();;
            }
        }

        public void Add(TEntity entity)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                IEnumerable<KeyValuePair<string, string>> RowsAndValues = ResolveProperties(entity);
                IEnumerable<string> keys = RowsAndValues.Select(c => c.Key);
                IEnumerable<string> values = RowsAndValues.Select(c => c.Value);
                string query = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", TableName, string.Join(",",keys), string.Join(",", values));
                connection.Execute(query);
            }
        }

        public void Update(TEntity entity)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                IEnumerable<KeyValuePair<string, string>> RowsAndValues = ResolveProperties(entity);
                IEnumerable<string> keys = RowsAndValues.Select(c => c.Key);
                IEnumerable<string> values = RowsAndValues.Select(c => c.Value);
                string query = string.Format("UPDATE {0} SET ({1}) = ({2}) WHERE Id = @Id;", TableName, string.Join(",", keys), string.Join(",", values));
                connection.Execute(query, new { Id = entity.Id });
            }
        }

        public void Remove(TEntity entity)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = string.Format("DELETE FROM {0} WHERE Id = @Id", TableName);
                connection.Execute(query, new { Id = entity.Id });
            }
        }

        private IEnumerable<KeyValuePair<string, string>> ResolveProperties(TEntity entity)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            PropertyInfo[] infos = entity.GetType().GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if(info.GetCustomAttribute<KeyAttribute>() == null)
                {
                    result.Add(new KeyValuePair<string, string>(info.Name, string.Format("'{0}'", info.GetValue(entity))));
                }
            }

            return result;
        }

        protected abstract string TableName { get; }
    }
}
