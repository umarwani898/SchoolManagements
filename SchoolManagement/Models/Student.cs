using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Validators;

namespace SchoolManagement.Models;

[Index("RollNumber", "Class", Name = "UQ__Students__2771E80D6509E98C", IsUnique = true)]
public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain alphabetic characters and spaces.")]
    public string Name { get; set; } = null!;

    public int Age { get; set; }

    [StringLength(10, ErrorMessage = "Please select a valid Gender.")]
    [RequiredIfNotValue("Select Gender", ErrorMessage = "Please select a valid Gender.")]
    public string Sex { get; set; } = null!;

    [StringLength(255)]
    public string? Image { get; set; }

    [StringLength(50)]
    [RequiredIfNotValue("Select Class", ErrorMessage = "Please select a valid class.")]
    public string? Class { get; set; }

    public int RollNumber { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }
}
