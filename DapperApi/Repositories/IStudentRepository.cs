using DapperApi.Models;

namespace DapperApi.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAll();
    Task<Student?> GetById(int id);
    Task<int> Create(Student student);
    Task<int> Update(Student student);
    Task<int> Delete(int id);
    Task<IEnumerable<StudentWithCourses>> GetAllWithCourses();
    Task<IEnumerable<Student>> SearchByName(string name);
}
