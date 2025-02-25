using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("students")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET ALL STUDENTS
    [HttpGet("getAllstudents")]
    public async Task<ActionResult> GetAllStudents()
    {
        var students = await _context.Students.Include(s => s.Courses).ToListAsync();

        // Convert EF Models to DTOs
        var studentDTOs = students
            .Select(s => new StudentDTO
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                Email = s.Email,
                Courses = s
                    .Courses.Select(c => new CourseDTO { Id = c.Id, Name = c.Name })
                    .ToList(),
            })
            .ToList();

        return Ok(studentDTOs);
    }

    //SHOW A STUDENT  without course

    [HttpGet("getstudent/{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound();
        return Ok(student);
    }

    [HttpGet("getstudentwithcourse/{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudentWidtCourse(int id)
    {
        var student = await _context
            .Students.Include(s => s.Courses) //we added course
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound(new { Message = "Student not found" });

        //convert to DTO
        var studentDTO = new StudentDTO
        {
            Id = student.Id,
            Name = student.Name,
            Age = student.Age,
            Email = student.Email,
            Courses = student
                .Courses.Select(c => new CourseDTO { Id = c.Id, Name = c.Name })
                .ToList(),
        };

        return Ok(studentDTO);
    }

    //ADD STUDENT
    [HttpPost("addstudent")]
    public async Task<ActionResult<Student>> AddStudent([FromBody] Student student)
    {
        var existedStudent = await _context.Students.FirstOrDefaultAsync(s =>
            s.Email == student.Email
        );
        if (existedStudent != null)
            return BadRequest(new { Message = "This student already exists" });
        //add
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Student Added" });
    }

    //ADD COURSE TO STUDENT
    [HttpPost("{studentId}/addcourse/{courseId}")]
    public async Task<ActionResult> AddCourseToStudent(int studentId, int courseId)
    {
        var student = await _context
            .Students.Include(s => s.Courses) //include course
            .FirstOrDefaultAsync(s => s.Id == studentId);

        var course = await _context.Courses.FindAsync(courseId);

        if (student == null || course == null)
            return NotFound(new { Message = "Student or course not found" });

        //prevent dublicate assignment
        if (!student.Courses.Contains(course))
        {
            student.Courses.Add(course);
            await _context.SaveChangesAsync();
        }
        ;
        return Ok(new { Message = "Course assigned to student" });
    }

    //DELETE A STUDENT
    [HttpDelete("deletestudent/{id}")]
    public async Task<ActionResult<Student>> DeleteStudent(int id)
    {
        var existedStudent = await _context.Students.FindAsync(id);
        if (existedStudent == null)
            return NotFound(new { Message = "Student not exists" });

        _context.Students.Remove(existedStudent);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Student Deleted ", Student = existedStudent });
    }

    //UPDATE STUDENT
    [HttpPut("updatestudent/{id}")]
    public async Task<ActionResult<Student>> UpdateStudent(
        int id,
        [FromBody] Student updatedStudent
    ) // ✅ Include `id` from URL
    {
        if (id != updatedStudent.Id) //  Ensure the ID in the URL matches the ID in the body
            return BadRequest(new { Message = "ID mismatch between URL and body" });

        var existedStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
        if (existedStudent == null)
            return NotFound(new { Message = "Student does not exist" });

        existedStudent.Name = updatedStudent.Name;
        existedStudent.Email = updatedStudent.Email;
        existedStudent.Age = updatedStudent.Age;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Student updated", Student = existedStudent }); // ✅ Return the updated student
    }
}
