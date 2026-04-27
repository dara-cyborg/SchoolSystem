namespace SchoolSystem.Core.DTOs.Subject;

public class CreateSubjectDto {
    public string Name { get; set; } = string.Empty;
}

public class UpdateSubjectDto {
    public string Name { get; set; } = string.Empty;
}

public class SubjectResponseDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ClassSubjectCount { get; set; }
}