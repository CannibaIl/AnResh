using Anresh.Application.Services.Employee.Contracts;
using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Employee.Implementations
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<EmployeeDTO> Create(Create.Request request, CancellationToken cancellationToken)
        {
            if (await _departmentRepository.FindById(request.DepartmentId, cancellationToken) == null)
            {
                throw new KeyNotFoundException($"Отдел с id:{request.DepartmentId} не найден");
            }

            var employe = new Domain.Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MidleName = request.MidleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary,
            };
            var id = await _employeeRepository.Save(employe);

            return await _employeeRepository.FindByIdWithDepartment(id, cancellationToken);
        }
        public async Task<EmployeeDTO> Update(Update.Request request, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.FindById(request.Id, cancellationToken) == null)
            {
                throw new KeyNotFoundException($"Сотрудник с id:{request.Id} не найден");
            }

            var employe = new Domain.Employee()
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MidleName = request.MidleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary
            };
            await _employeeRepository.Update(employe);

            return await _employeeRepository.FindByIdWithDepartment(request.Id, cancellationToken);
        }
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            if (await _employeeRepository.FindById(id, cancellationToken) == null)
            {
                throw new KeyNotFoundException($"Сотрудник с {id} не найден");
            }
            await _employeeRepository.Delete(id, cancellationToken);
        }
        public async Task DeleteList(List<int> listId, CancellationToken cancellationToken)
        {
            var notFoundStringId = "";
            foreach (var id in listId)
            {
                if(await _employeeRepository.FindById(id, cancellationToken) == null) 
                    notFoundStringId += $"{id}, ";
                else
                    await _employeeRepository.Delete(id, cancellationToken);
            }
            if (notFoundStringId != "")
                throw new KeyNotFoundException($"Сотрудники с id: {notFoundStringId} не найдены");
        }
        public async Task<Domain.Employee> GetById(int id, CancellationToken cancellationToken)
        {
            var employe = await _employeeRepository.FindById(id, cancellationToken);
            if (employe == null)
                throw new KeyNotFoundException($"Сотрудник с {id} не найден");

            return employe;
        }
        public async Task<IEnumerable<EmployeeDTO>> GetAll(CancellationToken cancellationToken)
        {
            return await _employeeRepository.FindAllWithDepartment(cancellationToken);
        }
        public async Task<IEnumerable<EmployeeDTO>> GetByDepartamentId(int id, CancellationToken cancellationToken)
        {
            return await _employeeRepository.FindByDepartmentIdWithDepartment(id, cancellationToken);
        }
        public async Task TransferToTheDepartment(int oldDepartmentId, int newDepartmentId, CancellationToken cancellationToken)
        {
            await _employeeRepository.TransferToTheDepartment(oldDepartmentId, newDepartmentId, cancellationToken);
        }
        public async Task DeleteAllByDepartmentId(int id, CancellationToken cancellationToken)
        {
            await _employeeRepository.DeleteByDepartmentId(id, cancellationToken);
        }
    }
}
