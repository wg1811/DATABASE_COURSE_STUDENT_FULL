using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Course
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    //relationship
    // many to many student<-> course


    public List<Student> Students { get; set; } = new List<Student>();
}
