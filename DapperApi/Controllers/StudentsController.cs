using Microsoft.AspNetCore.Mvc;
using DapperApi.Models;
using DapperApi.Repositories;

namespace DapperApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;

    public StudentsController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentRepository.GetAll();
        return Ok(students);
    }

    // GET /api/students/courses
    [HttpGet("courses")]
    public async Task<IActionResult> GetAllWithCourses()
    {
        var students = await _studentRepository.GetAllWithCourses();
        return Ok(students);
    }

    // GET /api/students/search?name=abc
    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Vui lòng cung cấp tham số 'name' để tìm kiếm.");

        var students = await _studentRepository.SearchByName(name);
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentRepository.GetById(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        var newId = await _studentRepository.Create(student);
        student.Id = newId;
        
        return CreatedAtAction(nameof(GetById), new { id = newId }, student);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Student student)
    {
        // Kiểm tra xem sinh viên có tồn tại hay không
        var existingStudent = await _studentRepository.GetById(student.Id);
        if (existingStudent == null)
        {
            return NotFound();
        }

        await _studentRepository.Update(student);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // Kiểm tra xem sinh viên có tồn tại hay không
        var existingStudent = await _studentRepository.GetById(id);
        if (existingStudent == null)
        {
            return NotFound();
        }

        await _studentRepository.Delete(id);
        return NoContent();
    }
}
