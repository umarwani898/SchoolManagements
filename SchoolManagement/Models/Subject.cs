using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagement.Validators;

namespace SchoolManagement.Models;

public partial class Subject
{
    [Key]
    public int SubjectId { get; set; }

    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain alphabetic characters and spaces.")]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [RequiredIfNotValue("Select Class", ErrorMessage = "Please select a valid class.")]
    public string Class { get; set; } = null!;

    [StringLength(500)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain alphabetic characters and spaces.")]
    public string? Languages { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
