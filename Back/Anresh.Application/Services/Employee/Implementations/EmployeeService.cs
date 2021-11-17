using System;
using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Domain;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Anresh.Application.Services.Department.Contracts;
using Create = Anresh.Application.Services.Employee.Contracts.Create;
using Update = Anresh.Application.Services.Employee.Contracts.Update;

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

        //в целом можно использовать EmployeeDto - набор свойств соответствует
        public async Task<Domain.Employee> Create(Create.Request request)
        {
            if (await _departmentRepository.IsExists(request.DepartmentId) == false)
            {
                throw new KeyNotFoundException($"Department with id: { request.DepartmentId } not found");
            }

            var employe = new Domain.Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary,
            };

            var id = await _employeeRepository.Save(employe);
            employe.Id = id;

            return employe;
        }

        public async Task<Domain.Employee> Update(Update.Request request) {
            var employee = await _employeeRepository.FindById(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with id: { request.Id } not found");
            }

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.MiddleName = request.MiddleName;
            employee.DepartmentId = request.DepartmentId;
            employee.Salary = request.Salary;

            await _employeeRepository.Update(employee);

            return await _employeeRepository.FindById(request.Id);
        }

        public async Task Delete(int id)
        {
            if (await _employeeRepository.IsExists(id) == false)
            {
                throw new KeyNotFoundException($"Employee with id: { id } not found");
            }
            await _employeeRepository.Delete(id);
        }

        public async Task DeleteMultiple(IEnumerable<int> ids)
        {
            await _employeeRepository.DeleteMultiple(ids);
        }

        public async Task<Domain.Employee> GetById(int id)
        {
            var employe = await _employeeRepository.FindById(id);
            if (employe == null)
            {
                throw new KeyNotFoundException($"Employee with id: { id } not found");
            }
            return employe;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            var departments = await _departmentRepository.FindAll();
            var departmentMap = departments.ToDictionary(d => d.Id);
            var employees = await _employeeRepository.FindAll();
            return employees.Select(e => MakeEmployee(e, (id) => departmentMap.GetValueOrDefault(id) ));
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartamentId(int id)
        {
            var employees = await _employeeRepository.FindAllByDepartmentId(id);
            var department = await _departmentRepository.FindById(id);
            return employees.Select(e => MakeEmployee(e, _ => department));
        }

        public async Task DeleteAllByDepartmentId(int id)
        {
            await _employeeRepository.DeleteByDepartmentId(id);
        }

        private static EmployeeDto MakeEmployee(Domain.Employee employee, Func<int, Domain.Department> departmenResolver) {
            var department = departmenResolver(employee.DepartmentId);
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                DepartmentId = department?.Id,
                DepartmentName = department?.Name,
                Salary = employee.Salary
            };
        }
    }
}
