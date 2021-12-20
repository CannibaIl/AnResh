using Anresh.Application.Services.Employee.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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

        public async Task<IEnumerable<EmployeeDto>> GetAllByDepartamentIdAsync(int id)
        {
            return await _employeeRepository.FindAllByDepartmentIdWithDepartmentNameAsync(id);
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeDto request)
        {
            if (await _departmentRepository.IsExistsAsync(request.DepartmentId) is false)
            {
                throw new KeyNotFoundException($"Department with id: { request.DepartmentId } not found");
            }
            request.Id = await _employeeRepository.SaveAsync(new Domain.Employee(request));

            if (request.Skills.Count > 0)
            {
                await _employeeSkillRepisitory.SaveMultipleAsync(request.Skills, request.Id);
            }

            return request;
        }

        public async Task<EmployeeDto> UpdateAsync(EmployeeDto request)
        {
            if (await _employeeRepository.IsExistsAsync(request.Id) is false)
            {
                throw new KeyNotFoundException($"Employee with id: { request.Id } not found");
            }
            await _employeeRepository.UpdateAsync(new Domain.Employee(request));

            var employeeSkills = await _employeeSkillRepisitory.FindByEmployeeIdAsync(request.Id);
            
            if (request.Skills.Count > 0)
            {
                var deleteSkillsList = employeeSkills.Where(x => request.Skills.Any(r => r.Id == x.SkillId) is false).Select(x => x.Id);
                if (deleteSkillsList.Any())
                {
                    await _employeeSkillRepisitory.DeleteMultipleAsync(deleteSkillsList);
                }

                var addSkillsList = request.Skills.Where(x => employeeSkills.Any(e => e.SkillId == x.Id) is false).ToList();
                if (addSkillsList.Any())
                {
                    await _employeeSkillRepisitory.SaveMultipleAsync(addSkillsList, request.Id);
                } 
            }
            else if (employeeSkills.Any())
            {
                await _employeeSkillRepisitory.DeleteMultipleAsync(employeeSkills.Select(x => x.Id));
            }

            return request;
        }

        public async Task DeleteAsync(int id)
        {
            if (await _employeeRepository.IsExistsAsync(id) is false)
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

        public async Task<IEnumerable<EmployeeDto>> GetPagedAsync(PageParams pageParams)
        {
            return await _employeeRepository.FindWithDepartmentNameAndSkillsAsync(pageParams);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartamentIdPagedAsyncAsync(PageParams pageParams, int id)
        {
            return await _employeeRepository.FindWithDepartmentNameAndSkillsAsync(pageParams, id);
        }

        public async Task DeleteAllByDepartmentIdAsync(int id)
        {
            await _employeeRepository.DeleteByDepartmentIdAsync(id);
        }

        public async Task<int> GetTotalRows()
        {
            return await _employeeRepository.GetTotalRows();
        }

        public async Task<EmployeesFiltredPage> GetFiltredWithDepartmentNameAndSkillsAsync(EmployeesFilter employeesFilter)
        {
            return await _employeeRepository.FindFiltredWithDepartmentNameAndSkillsAsync(employeesFilter);
        }
    }
}