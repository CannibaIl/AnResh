using Anresh.Application.Services.Department.Interfaces;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Implementations
{
    public sealed class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<IEnumerable<Domain.Department>> GetAll(CancellationToken cancellationToken)
        {
            return await _departmentRepository.FindAll(cancellationToken);
        }
    }
}
