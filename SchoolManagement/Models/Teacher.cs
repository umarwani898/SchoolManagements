using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Validators;

namespace SchoolManagement.Models;

public partial class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain alphabetic characters and spaces.")]
    public string Name { get; set; } = null!;

    public int Age { get; set; }

    [StringLength(10, ErrorMessage = "Please select a valid Gender.")]
    [RequiredIfNotValue("Select Gender", ErrorMessage = "Please select a valid Gender.")]
    public string Sex { get; set; } = null!;

    [StringLength(255)]
    public string? Image { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
