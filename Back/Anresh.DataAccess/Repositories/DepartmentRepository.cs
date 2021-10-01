using Anresh.Domain;
using Anresh.Domain.Repositories;
using System.Data;

namespace Anresh.DataAccess.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbConnection db) : base(db, TableNames.Departments)
        {
        }
    }
}
