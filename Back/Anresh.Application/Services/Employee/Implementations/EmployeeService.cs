using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Create = Anresh.Application.Services.Employee.Contracts.Create;
using Update = Anresh.Application.Services.Employee.Contracts.Update;

namespace Anresh.Application.Services.Employee.Implementations
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeSkillRepisitory _employeeSkillRepisitory;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IEmployeeSkillRepisitory employeeSkillRepisitory
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _employeeSkillRepisitory = employeeSkillRepisitory;
        }

        public async Task<EmployeeDto> CreateAsync(Create request)
        {
            if (await _departmentRepository.IsExistsAsync(request.DepartmentId) == false)
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

            var id = await _employeeRepository.SaveAsync(employe);
            employe.Id = id;
            
            if(request.Skills.Count > 0)
            {
                await _employeeSkillRepisitory.SaveMultipleAsync(request.Skills, employe.Id);
            }

            return new EmployeeDto()
            {
                Id = employe.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DepartmentId = request.DepartmentId,
                Salary = employe.Salary,
                Skills = request.Skills
            };
        }

        public async Task<EmployeeDto> UpdateAsync(Update request)
        {
            var employee = await _employeeRepository.FindByIdAsync(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with id: { request.Id } not found");
            }

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.MiddleName = request.MiddleName;
            employee.DepartmentId = request.DepartmentId;
            employee.Salary = request.Salary;

            await _employeeRepository.UpdateAsync(employee);

            if (request.Skills.Count > 0)
            {
                await _employeeSkillRepisitory.SaveMultipleAsync(request.Skills, request.Id);
            }

            return new EmployeeDto()
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DepartmentId = request.DepartmentId,
                Salary = request.Salary,
                Skills = request.Skills
            };
        }

        public async Task DeleteAsync(int id)
        {
            if (await _employeeRepository.IsExistsAsync(id) == false)
            {
                throw new KeyNotFoundException($"Employee with id: { id } not found");
            }
            await _employeeRepository.DeleteAsync(id);
        }

        public async Task DeleteMultipleAsync(IEnumerable<int> ids)
        {
            await _employeeRepository.DeleteMultipleAsync(ids);
        }

        public async Task<Domain.Employee> GetByIdAsync(int id)
        {
            var employe = await _employeeRepository.FindByIdAsync(id);
            if (employe == null)
            {
                throw new KeyNotFoundException($"Employee with id: { id } not found");
            }
            return employe;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            return await _employeeRepository.FindAllWithDepartmentNameAndSkillsAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartamentIdAsync(int id)
        {
            return await _employeeRepository.FindAllByDepartmentIdWithDepartmentNameAsync(id);
        }

        public async Task DeleteAllByDepartmentIdAsync(int id)
        {
            await _employeeRepository.DeleteByDepartmentIdAsync(id);
        }
    }
}