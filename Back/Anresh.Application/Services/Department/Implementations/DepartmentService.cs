using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anresh.Application.Services.Department.Contracts;
using Anresh.Application.Services.Department.Interfaces;
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

        public async Task<Domain.Department> Create(Create.Request request)
        {
            if (await _departmentRepository.CheckName(request.Name))
            {
                throw new Exception($"Department with name: {request.Name} already created!");
            }

            var department = new Domain.Department()
            {
                Name = request.Name
            };

            var id = await _departmentRepository.Save(department);
            department.Id = id;

            return department;
        }

        public async Task<Domain.Department> Update(Update.Request request)
        {
           var department = await _departmentRepository.FindById(request.Id);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with id:{request.Id} not found");
            }

            department.Name = request.Name;

            await _departmentRepository.Update(department);

            return department;
        }

        public async Task Delete(int id)
        {
            await _departmentRepository.Delete(id);
        }

        public async Task<Domain.Department> GetSimpleById(int id)
        {
            return await _departmentRepository.FindById(id);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAll()
        {
            var employeeCount = _employeeRepository.CountAndGroupByDepartment();
            return await _departmentRepository.FindAll()
                                              .ContinueWith(res => res.Result.Select(d => {
                                                  employeeCount.TryGetValue(d.Id, out var count);
                                                  return new DepartmentDto
                                                  {
                                                      Id = d.Id,
                                                      Name = d.Name,
                                                      EmployeeCount = count
                                                  };
                                              }));
        }

        public async Task<IEnumerable<OptionDto>> GetAllAsOptions()
        {
            return await _departmentRepository.FindAll()
                                       .ContinueWith(res => res.Result.Select(d =>
                                                                                    new OptionDto(d.Id, d.Name))
                                                     );
        }

        public async Task<DepartmentDto> MoveEmployees(int oldDepartmentId, int newDepartmentId)
        {
            if (!_departmentRepository.IsExists(newDepartmentId).Result)
            {
                throw new KeyNotFoundException($"Department with id={newDepartmentId} not found");
            }

            if (!_departmentRepository.IsExists(oldDepartmentId).Result)
            {
                throw new KeyNotFoundException($"Department with id={oldDepartmentId} not found");
            }

            await _employeeRepository.TransferToDepartment(oldDepartmentId, newDepartmentId);

            var department = await _departmentRepository.FindById(newDepartmentId);
            var count = _employeeRepository.CountByDepartmentId(newDepartmentId);
            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                EmployeeCount = count
            };
        }
    }
}