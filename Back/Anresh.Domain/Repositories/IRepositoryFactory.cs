namespace Anresh.Domain.Repositories
{
    public interface IRepositoryFactory
    {
        IDepartmentRepository CreateDepartmentRepository();
        IEmployeeRepository CreateEmployeeRepository();
        IEmployeeSkillRepisitory CreateEmployeeSkillRepository();
        ISkillRepository CreateSkillRepository();
        IUserRepository CreateUserRepository();
    }
}
