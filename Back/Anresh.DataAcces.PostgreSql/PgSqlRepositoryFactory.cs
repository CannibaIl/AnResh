using Anresh.DataAccess.MsSql.Repositories;
using Anresh.Domain.Repositories;
using System.Data;

namespace Anresh.DataAccess.PostgreSql
{
    public class PgSqlRepositoryFactory : IRepositoryFactory
    {
        private readonly IDbConnection _dbConnection;
        public PgSqlRepositoryFactory(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IDepartmentRepository CreateDepartmentRepository() => new PgSqlDepartmentRepository(_dbConnection);
        public IEmployeeRepository CreateEmployeeRepository() => new PgSqlEmployeeRepository(_dbConnection);
        public IEmployeeSkillRepisitory CreateEmployeeSkillRepository() => new PgSqlEmployeeSkillRepisitory(_dbConnection);
        public ISkillRepository CreateSkillRepository() => new PgSqlSkillRepository(_dbConnection);
        public IUserRepository CreateUserRepository() => new PgSqlUserRepository(_dbConnection);
    }
}
