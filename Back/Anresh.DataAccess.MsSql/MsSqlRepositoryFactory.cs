using Anresh.DataAccess.MsSql.Repositories;
using Anresh.Domain.Repositories;
using System.Data;

namespace Anresh.DataAccess.MsSql
{
    public class MsSqlRepositoryFactory : IRepositoryFactory
    {
        private readonly IDbConnection _dbConnection;
        public MsSqlRepositoryFactory(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IDepartmentRepository CreateDepartmentRepository() => new MsSqlDepartmentRepository(_dbConnection);
        public IEmployeeRepository CreateEmployeeRepository() => new MsSqlEmployeeRepository(_dbConnection);
        public IEmployeeSkillRepisitory CreateEmployeeSkillRepository() => new MsSqlEmployeeSkillRepisitory(_dbConnection);
        public ISkillRepository CreateSkillRepository() => new MsSqlSkillRepository(_dbConnection);
        public IUserRepository CreateUserRepository() => new MsSqlUserRepository(_dbConnection);
    }
}
