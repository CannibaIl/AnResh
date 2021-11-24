using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anresh.Application.Services.Department.Contracts;
using Anresh.Application.Services.Department.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;

namespace Anresh.Application.Services.Department.Implementations
{
    public sealed class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public DepartmentService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<Domain.Department> CreateAsync(Create request)
        {
            if (await _departmentRepository.CheckNameAsync(request.Name))
            {
                throw new Exception($"Department with name: {request.Name} already created!");
            }

            var department = new Domain.Department()
            {
                Name = request.Name
            };

            var id = await _departmentRepository.SaveAsync(department);
            department.Id = id;

            return department;
        }

        public async Task<Domain.Department> UpdateAsync(Update request)
        {
            var department = await _departmentRepository.FindByIdAsync(request.Id);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with id:{request.Id} not found");
            }

            department.Name = request.Name;

            await _departmentRepository.UpdateAsync(department);

            return department;
        }

        public async Task DeleteAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<Domain.Department> GetSimpleByIdAsync(int id)
        {
            return await _departmentRepository.FindByIdAsync(id);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            return await _departmentRepository.FindAllWithEmployeeCountAsync();
        }

        public async Task<IEnumerable<Domain.Department>> GetAllSimpleAsync()
        {
            return await _departmentRepository.FindAllAsync();
        }

        public async Task MoveEmployeesAsync(int oldDepartmentId, int newDepartmentId)
        {
            if (!_departmentRepository.IsExistsAsync(newDepartmentId).Result)
            {
                throw new KeyNotFoundException($"Department with id={newDepartmentId} not found");
            }

            if (!_departmentRepository.IsExistsAsync(oldDepartmentId).Result)
            {
                throw new KeyNotFoundException($"Department with id={oldDepartmentId} not found");
            }

            await _employeeRepository.TransferToDepartmentAsync(oldDepartmentId, newDepartmentId);
        }
    }
}