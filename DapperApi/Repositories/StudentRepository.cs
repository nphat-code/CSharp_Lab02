using System.Data;
using Dapper;
using DapperApi.Models;
using Npgsql;

namespace DapperApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly string _connectionString;

    // Inject IConfiguration để lấy chuỗi kết nối từ appsettings.json
    public StudentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentNullException("Connection string is missing");
    }

    // Khởi tạo kết nối PostgreSQL bằng NpgsqlConnection
    private IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task<IEnumerable<Student>> GetAll()
    {
        using var connection = CreateConnection();
        var sql = "SELECT id, name, age, email FROM students;";
        
        return await connection.QueryAsync<Student>(sql);
    }

    public async Task<Student?> GetById(int id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT id, name, age, email FROM students WHERE id = @Id;";
        
        return await connection.QuerySingleOrDefaultAsync<Student>(sql, new { Id = id });
    }

    public async Task<int> Create(Student student)
    {
        using var connection = CreateConnection();
        var sql = @"
            INSERT INTO students (name, age, email) 
            VALUES (@Name, @Age, @Email) 
            RETURNING id;";
        
        return await connection.ExecuteScalarAsync<int>(sql, student);
    }

    public async Task<int> Update(Student student)
    {
        using var connection = CreateConnection();
        var sql = @"
            UPDATE students 
            SET name = @Name, age = @Age, email = @Email 
            WHERE id = @Id;";
            
        return await connection.ExecuteAsync(sql, student);
    }

    public async Task<int> Delete(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM students WHERE id = @Id;";
        
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<StudentWithCourses>> GetAllWithCourses()
    {
        using var connection = CreateConnection();
        var sql = @"
            SELECT 
                s.id, s.name, s.age, s.email,
                c.id, c.course_name as CourseName
            FROM students s
            LEFT JOIN student_courses sc ON s.id = sc.student_id
            LEFT JOIN courses c ON sc.course_id = c.id;";

        var studentDict = new Dictionary<int, StudentWithCourses>();

        var students = await connection.QueryAsync<StudentWithCourses, Course, StudentWithCourses>(
            sql,
            (student, course) =>
            {
                if (!studentDict.TryGetValue(student.Id, out var currentStudent))
                {
                    currentStudent = student;
                    studentDict.Add(currentStudent.Id, currentStudent);
                }

                if (course != null && course.Id > 0)
                {
                    currentStudent.Courses.Add(course);
                }

                return currentStudent;
            },
            splitOn: "id"
        );

        return studentDict.Values;
    }

    public async Task<IEnumerable<Student>> SearchByName(string name)
    {
        using var connection = CreateConnection();
        var sql = "SELECT id, name, age, email FROM students WHERE name ILIKE @Name;";
        
        // Thêm ký tự wildcard % vào 2 đầu để tìm kiếm chứa từ khóa (LIKE %name%)
        return await connection.QueryAsync<Student>(sql, new { Name = $"%{name}%" });
    }
}
