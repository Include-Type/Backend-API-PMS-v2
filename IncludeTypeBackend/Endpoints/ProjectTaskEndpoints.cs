namespace IncludeTypeBackend.Endpoints;

public static class ProjectTaskEndpoints
{
    public static IEndpointRouteBuilder MapProjectTaskEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpoints = builder.MapGroup("projecttask");

        endpoints.MapGet(
            "/gettasks",
            ([FromServices] ProjectTaskService taskService) => GetTasks(taskService)
        )
        .WithName("GetTasks")
        .WithDescription("Get all the tasks.")
        .WithOpenApi();

        endpoints.MapGet(
            "/gettotaltasks",
            ([FromServices] ProjectTaskService taskService) => GetTotalTasks(taskService)
        )
        .WithName("GetTotalTasks")
        .WithDescription("Get the total number of tasks.")
        .WithOpenApi();

        endpoints.MapGet(
            "/gettasksbyauthor/{author}",
            ([FromServices] ProjectTaskService taskService, [FromRoute] string author) =>
                GetTasksByAuthor(taskService, author)
        )
        .WithName("GetTasksByAuthor")
        .WithDescription("Get all the tasks created by a specific author.")
        .WithOpenApi();

        endpoints.MapGet(
            "/gettasksbyusername/{username}",
            ([FromServices] ProjectTaskService taskService, [FromRoute] string username) =>
                GetTasksByUsername(taskService, username)
        )
        .WithName("GetTasksByUsername")
        .WithDescription("Get all the tasks assigned to a specific user.")
        .WithOpenApi();

        endpoints.MapPost(
            "/updatetasksbyauthor/{author}",
            ([FromServices] ProjectTaskService taskService,
                [FromBody] ProjectTaskDto taskDto,
                [FromRoute] string author) =>
                UpdateTasksByAuthor(taskService, taskDto, author)
        )
        .WithName("UpdateTasksByAuthor")
        .WithDescription("Update all the tasks created by a specific author.")
        .WithOpenApi();

        endpoints.MapPost(
            "/updatetasksbyusername/{username}",
            ([FromServices] ProjectTaskService taskService,
                [FromBody] ProjectTaskDto taskDto,
                [FromRoute] string username) =>
                UpdateTasksByUsername(taskService, taskDto, username)
        )
        .WithName("UpdateTasksByUsername")
        .WithDescription("Update all the tasks assigned to a specific user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/gettasksforgivendeadline/{key}",
            ([FromServices] ProjectTaskService taskService, [FromRoute] string key) =>
                GetTasksForGivenDeadline(taskService, key)
        )
        .WithName("GetTasksForGivenDeadline")
        .WithDescription("Get all the tasks for a specific deadline.")
        .WithOpenApi();

        endpoints.MapPost(
            "/addtask",
            ([FromServices] ProjectTaskService taskService, [FromBody] ProjectTask projectTask) =>
                AddTask(taskService, projectTask)
        )
        .WithName("AddTask")
        .WithDescription("Add a new task.")
        .WithOpenApi();

        return endpoints;
    }

    private static async Task<IResult> GetTasks(ProjectTaskService taskService) =>
        TypedResults.Ok(await taskService.GetAllTasksAsync());

    private static async Task<IResult> GetTotalTasks(ProjectTaskService taskService) =>
        TypedResults.Ok(await taskService.GetTotalTasksAsync());

    private static async Task<IResult> GetTasksByAuthor(ProjectTaskService taskService, string author)
    {
        List<ProjectTask> tasks = await taskService.GetAllTasksByAuthorAsync(author);
        tasks.Sort(new ProjectTaskComparer());
        return TypedResults.Ok(tasks);
    }

    private static async Task<IResult> GetTasksByUsername(ProjectTaskService taskService, string username)
    {
        List<ProjectTask> tasks = await taskService.GetAllTasksByUsernameAsync(username);
        tasks.Sort(new ProjectTaskComparer());
        return TypedResults.Ok(tasks);
    }

    private static async Task<IResult> UpdateTasksByAuthor(ProjectTaskService projectTaskService, ProjectTaskDto taskDto, string author)
    {
        try
        {
            await projectTaskService.UpdateAllTasksByAuthorAsync(taskDto.Tasks, author);
            return TypedResults.Ok("Project tasks updated.");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid data!");
        }
    }

    private static async Task<IResult> UpdateTasksByUsername(ProjectTaskService projectTaskService, ProjectTaskDto taskDto, string username)
    {
        try
        {
            await projectTaskService.UpdateAllTasksByUsernameAsync(taskDto.Tasks, username);
            return TypedResults.Ok("Project tasks updated.");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid data!");
        }
    }

    private static async Task<IResult> GetTasksForGivenDeadline(ProjectTaskService projectTaskService, string key)
    {
        List<ProjectTask> projectTasks = await projectTaskService.GetAllTasksForGivenDeadlineAsync(key);
        projectTasks.Sort(new ProjectTaskComparer());
        return TypedResults.Ok(projectTasks);
    }

    private static async Task<IResult> AddTask(ProjectTaskService projectTaskService, ProjectTask projectTask)
    {
        try
        {
            Guid guid = Guid.NewGuid();
            projectTask.Id = $"{guid}";

            DateTime dateTime = DateTime.Now;
            projectTask.Date = dateTime.ToString("MMM-dd-yyyy");

            DateTime dueDate = DateTime.Parse(projectTask.Deadline);
            projectTask.Deadline = dueDate.ToString("MMM-dd-yyyy");

            await projectTaskService.AddTaskAsync(projectTask);
            return TypedResults.Ok("Task successfully added");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid data!");
        }
    }
}