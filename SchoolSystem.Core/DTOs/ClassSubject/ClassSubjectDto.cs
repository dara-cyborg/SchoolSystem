namespace SchoolSystem.Core.DTOs.ClassSubject;

public class CreateClassSubjectDto {
    public int ClassId { get; set; }
    public int SubjectId { get; set; }
    public int? TeacherUserId { get; set; }
}

public class UpdateClassSubjectDto {
    public int? TeacherUserId { get; set; }
}

public class ClassSubjectResponseDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int? TeacherUserId { get; set; }
    public string? TeacherName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ClassSubjectWithStudentsDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int? TeacherUserId { get; set; }
    public string? TeacherName { get; set; }
    public List<StudentInClassDto> Students { get; set; } = new();
    public string DisplayName => $"{ClassName} - {SubjectName}";
}

public class StudentInClassDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
}