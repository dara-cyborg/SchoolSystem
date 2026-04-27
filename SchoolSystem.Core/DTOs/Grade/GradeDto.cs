namespace SchoolSystem.Core.DTOs.Grade;

public class CreateGradeDto {
    public string Name { get; set; } = string.Empty;
}

public class UpdateGradeDto {
    public string Name { get; set; } = string.Empty;
}

public class GradeResponseDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ClassCount { get; set; }
}