using Anresh.Application.Services.Department.Contracts;
using Anresh.Application.Services.Department.Interfaces;
using Anresh.Domain.DTO;
using Anresh.Domain.Repositories;
using System;
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

        public async Task<DepartmentDTO> Create(Create.Request request, CancellationToken cancellationToken)
        {
            if(await _departmentRepository.CheckName(request.Name, cancellationToken))
            {
                throw new Exception($"Отдел с именем: {request.Name} уже создан!");
            }
            var department = new Domain.Department()
            {
                Name = request.Name
            };
            var id = await _departmentRepository.Save(department);

            return await _departmentRepository.FindByIdWithEmployees(id, cancellationToken);
        }
        public async Task<DepartmentDTO> Update(Update.Request request, CancellationToken cancellationToken)
        {
            if (await _departmentRepository.FindById(request.Id, cancellationToken) == null)
            {
                throw new KeyNotFoundException($"Отдел с id:{request.Id} не найден");
            }

            var department = new Domain.Department()
            {
                Id = request.Id,
                Name = request.Name
            };
            await _departmentRepository.Update(department);
            return await _departmentRepository.FindByIdWithEmployees(request.Id, cancellationToken);
        }
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _departmentRepository.Delete(id, cancellationToken);
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAll(CancellationToken cancellationToken)
        {
            return await _departmentRepository.FindAllWithEmployees(cancellationToken);
        }
        public async Task<IEnumerable<Domain.Department>> GetAllLight(CancellationToken cancellationToken)
        {
            return await _departmentRepository.FindAll(cancellationToken);
        }
    }
}
