namespace SchoolManagement.Models
{
    public class TeacherSubjectDTO
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = null!;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
    }

    public class TeacherSubjectRequestDTO
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}
