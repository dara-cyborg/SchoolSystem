namespace SchoolSystem.Core.DTOs.Parent;

public class LinkParentStudentDto {
    public int ParentUserId { get; set; }
    public int StudentId { get; set; }
}

public class ParentStudentResponseDto {
    public int ParentUserId { get; set; }
    public string ParentName { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentClassName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ParentStudentsListDto {
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public List<ParentInfoDto> Parents { get; set; } = new();
}

public class ParentInfoDto {
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Contact { get; set; }
}