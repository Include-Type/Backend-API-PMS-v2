namespace IncludeTypeBackend.Services;

public class ProjectTaskService
{
    private readonly PostgreSqlContext _db;

    public ProjectTaskService(PostgreSqlContext db) => _db = db;

    public async Task<List<ProjectTask>> GetAllTasksAsync() => await _db.ProjectTask.ToListAsync();

    public async Task<int> GetTotalTasksAsync() => (await GetAllTasksAsync()).Count;

    public async Task<List<ProjectTask>> GetAllTasksByAuthorAsync(string author) =>
        await _db.ProjectTask.Where(task => task.Author.Equals(author)).ToListAsync();

    public async Task<List<ProjectTask>> GetAllTasksByUsernameAsync(string username) =>
        await _db.ProjectTask.Where(task => task.Assigned.Contains(username)).ToListAsync();

    public async Task UpdateAllTasksByAuthorAsync(ProjectTask[] projectTasks, string author)
    {
        foreach (ProjectTask projectTask in _db.ProjectTask)
        {
            if (projectTask.Author.Equals(author))
            {
                _db.ProjectTask.Remove(projectTask);
            }
        }

        foreach (ProjectTask projectTask in projectTasks)
        {
            if (projectTask.Id.Length < 10)
            {
                Guid guid = Guid.NewGuid();
                projectTask.Id = $"{guid}";
            }

            await _db.ProjectTask.AddAsync(projectTask);
        }

        await _db.SaveChangesAsync();
    }

    public async Task UpdateAllTasksByUsernameAsync(ProjectTask[] projectTasks, string username)
    {
        foreach (ProjectTask projectTask in _db.ProjectTask)
        {
            if (projectTask.Assigned.Contains(username))
            {
                _db.ProjectTask.Remove(projectTask);
            }
        }

        foreach (ProjectTask projectTask in projectTasks)
        {
            if (projectTask.Id.Length < 10)
            {
                Guid guid = Guid.NewGuid();
                projectTask.Id = $"{guid}";
            }

            await _db.ProjectTask.AddAsync(projectTask);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<List<ProjectTask>> GetAllTasksForGivenDeadlineAsync(string key) =>
        await _db.ProjectTask.Where(task => task.Deadline.Equals(key)).ToListAsync();

    public async Task AddTaskAsync(ProjectTask task)
    {
        await _db.ProjectTask.AddAsync(task);
        await _db.SaveChangesAsync();
    }
}
