namespace SchoolSystem.Core.DTOs.Class;

public class CreateClassDto {
    public int GradeId { get; set; }
    public short SchoolYear { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? HomeroomUserId { get; set; }
}

public class UpdateClassDto {
    public int GradeId { get; set; }
    public short SchoolYear { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? HomeroomUserId { get; set; }
}

public class ClassResponseDto {
    public int Id { get; set; }
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public short SchoolYear { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? HomeroomUserId { get; set; }
    public string? HomeroomTeacherName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StudentCount { get; set; }
}

public class ClassWithSubjectsDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public short SchoolYear { get; set; }
    public string? HomeroomTeacherName { get; set; }
    public List<SubjectDto> Subjects { get; set; } = new();
}

public class SubjectDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? TeacherUserId { get; set; }
    public string? TeacherName { get; set; }
}