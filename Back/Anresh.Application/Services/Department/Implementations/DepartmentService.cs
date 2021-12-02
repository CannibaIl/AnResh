using Anresh.Application.Services.Auth.Interfaces;
using Anresh.Application.Services.Department.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anresh.Application.Services.Department.Implementations
{
    public sealed class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly IAuthService _authService;

        public DepartmentService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository
            //,IAuthService authService
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            //_authService = authService;
        }

        public async Task<Domain.Department> CreateAsync(Domain.Department request)
        {
            if (await _departmentRepository.CheckNameAsync(request.Name))
            {
                throw new Exception($"Department with name: {request.Name} already created!");
            }

            var id = await _departmentRepository.SaveAsync(request);
            request.Id = id;

            return request;
        }

        public async Task<Domain.Department> UpdateAsync(Domain.Department request)
        {
            if (await _departmentRepository.IsExistsAsync(request.Id) == false)
            {
                throw new KeyNotFoundException($"Department with id:{request.Id} not found");
            }

            await _departmentRepository.UpdateAsync(request);
            return request;
        }

        public async Task DeleteAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<Domain.Department> GetSimpleByIdAsync(int id)
        {
            return await _departmentRepository.FindByIdAsync(id);
        }

        public async Task<IEnumerable<DepartmentDto>> GetPagedAsync(PageParams pageParams)
        {
            return await _departmentRepository.FindWithEmployeeCountAsync(pageParams);
        }

        public async Task<IEnumerable<Domain.Department>> GetAllSimpleAsync()
        {
            return await _departmentRepository.FindAllAsync();
        }

        public async Task MoveEmployeesAsync(int oldDepartmentId, int newDepartmentId)
        {
            if (await _departmentRepository.IsExistsAsync(newDepartmentId) == false)
            {
                throw new KeyNotFoundException($"Department with id={newDepartmentId} not found");
            }
            if (await _departmentRepository.IsExistsAsync(oldDepartmentId) == false)
            {
                throw new KeyNotFoundException($"Department with id={oldDepartmentId} not found");
            }

            await _employeeRepository.TransferToDepartmentAsync(oldDepartmentId, newDepartmentId);
        }

        public async Task<int> GetTotalRows()
        {
            return await _departmentRepository.GetTotalRows();
        }
    }
}