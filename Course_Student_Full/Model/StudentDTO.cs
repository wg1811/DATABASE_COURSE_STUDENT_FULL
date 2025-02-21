public class StudentDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
    public List<CourseDTO> Courses { get; set; } = new();
}