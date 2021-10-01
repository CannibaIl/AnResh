using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Employee.Implementations
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IEnumerable<Domain.Employee>> GetAll(CancellationToken cancellationToken)
        {
            return await _employeeRepository.FindAll(cancellationToken);
        }
        public async Task<IEnumerable<Domain.Employee>> GetByDepartamentId(int id, CancellationToken cancellationToken)
        {
            return await _employeeRepository.FindByDepartmentId(id, cancellationToken);
        }
    }
}
