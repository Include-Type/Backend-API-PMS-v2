namespace IncludeTypeBackend.Dtos;

public class CompleteUserDto
{
    public User User { get; set; } = new();
    public ProfessionalProfile ProfessionalProfile { get; set; } = new();
    public Privacy Privacy { get; set; } = new();
}
