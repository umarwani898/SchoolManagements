using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models;

public partial class TeacherSubject
{
    [Key]
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("TeacherSubjects")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TeacherSubjects")]
    public virtual Teacher Teacher { get; set; } = null!;
}
