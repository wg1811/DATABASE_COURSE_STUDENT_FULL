using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";
    [Required]
    public int Age { get; set; } = 0;

    [Required]
    public string Email { get; set; } = "";

    //many to many between student-course

    public List<Course> Courses { get; set; } = new List<Course>();

}