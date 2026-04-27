using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.Student;

public class StudentDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateStudentDto {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public int ClassId { get; set; }
}

public class UpdateStudentDto {
    public string Name { get; set; } = string.Empty;
    public SexType Sex { get; set; }
    public DateTime? Dob { get; set; }
    public string? Contact { get; set; }
    public int ClassId { get; set; }
}

public class LinkParentDto {
    public int ParentUserId { get; set; }
}