namespace apiExo.domain.entity;


public class TaskEntity(int id, string title,string status, DateTime createdAt)
{
    public int Id { get; } = id;
    public string Title { get; set; } = title;
    public string Status { get; set; } = status;
    public DateTime CreatedAt { get; set; } = createdAt;
}