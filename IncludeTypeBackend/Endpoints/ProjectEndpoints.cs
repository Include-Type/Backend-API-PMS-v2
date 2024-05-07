namespace IncludeTypeBackend.Endpoints;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpoints = builder.MapGroup("project");

        endpoints.MapGet(
            "/getallprojects",
            ([FromServices] ProjectService projectService) => GetAllProjects(projectService)
        )
        .WithName("GetAllProjects")
        .WithDescription("Get all projects.")
        .WithOpenApi();

        endpoints.MapGet(
            "/gettotalprojects",
            ([FromServices] ProjectService projectService) => GetTotalProjects(projectService)
        )
        .WithName("GetTotalProjects")
        .WithDescription("Get the total number of projects.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getallprojectsbyusername/{username}",
            ([FromServices] ProjectService projectService, [FromRoute] string username) =>
                GetAllProjectsByUsername(projectService, username)
        )
        .WithName("GetAllProjectsByUsername")
        .WithDescription("Get all projects of a specific user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getprojectdetails/{projectWithUsername}",
            ([FromServices] ProjectService projectService, [FromRoute] string projectWithUsername) =>
                GetProjectDetails(projectService, projectWithUsername)
        )
        .WithName("GetProjectDetails")
        .WithDescription("Get details of a specific project.")
        .WithOpenApi();

        endpoints.MapPost(
            "/addproject/{username}",
            ([FromServices] ProjectService projectService,
                [FromRoute] string username,
                [FromBody] Project project) =>
                AddProject(projectService, username, project)
        )
        .WithName("AddProject")
        .WithDescription("Add a new project.")
        .WithOpenApi();

        endpoints.MapPost(
            "/updateproject/{projName}",
            ([FromServices] ProjectService projectService,
                [FromRoute] string projName,
                [FromBody] Project updatedProject) =>
                UpdateProject(projectService, projName, updatedProject)
        )
        .WithName("UpdateProject")
        .WithDescription("Update a project.")
        .WithOpenApi();

        endpoints.MapPost(
            "/updateprojectmembers/{projName}",
            ([FromServices] ProjectService projectService,
                [FromRoute] string projName,
                [FromBody] List<ProjectMember> projectMembers) =>
                UpdateProjectMembers(projectService, projName, projectMembers)
        )
        .WithName("UpdateProjectMembers")
        .WithDescription("Update the project members of a specific project.")
        .WithOpenApi();

        endpoints.MapDelete(
            "/deleteallterminatedprojects",
            ([FromServices] ProjectService projectService) => DeleteAllTerminatedProjects(projectService)
        )
        .WithName("DeleteAllTerminatedProjects")
        .WithDescription("Delete all the terminated projects.")
        .WithOpenApi();

        return endpoints;
    }

    private static async Task<IResult> GetAllProjects(ProjectService projectService) =>
        TypedResults.Ok(await projectService.GetAllProjectsAsync());

    private static async Task<IResult> GetTotalProjects(ProjectService projectService) =>
        TypedResults.Ok(await projectService.GetTotalProjectsAsync());

    private static async Task<IResult> GetAllProjectsByUsername(ProjectService projectService, string username) =>
        TypedResults.Ok(await projectService.GetAllProjectsByUsernameAsync(username));

    private static async Task<IResult> GetProjectDetails(ProjectService projectService, string projectWithUsername)
    {
        try
        {
            string[] temp = projectWithUsername.Split('&');
            Project project = await projectService.GetProjectAsync(temp[0]);
            return TypedResults.Ok(await projectService.GetProjectDetailsAsync(temp[0], temp[1]));
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> AddProject(ProjectService projectService, string username, Project project)
    {
        try
        {
            Guid guid = Guid.NewGuid();
            project.Id = $"{guid}";
            DateTime dateTime = DateTime.Now;
            project.Date = dateTime.ToString("MMM-dd-yyyy");
            await projectService.AddProjectAsync(project, username);
            return TypedResults.Ok("Project successfully added.");
        }
        catch (Exception ex)
        {
            return ex.Message.Length switch
            {
                > 15 => TypedResults.BadRequest("Invalid data!"),
                _ => TypedResults.BadRequest(ex.Message)
            };
        }
    }

    private static async Task<IResult> UpdateProject(ProjectService projectService, string projName, Project updatedProject)
    {
        try
        {
            Project existingProject = await projectService.GetProjectAsync(projName);
            await projectService.UpdateProjectAsync(existingProject, updatedProject);
            return TypedResults.Ok("Project successfully updated.");
        }
        catch (Exception ex)
        {
            return ex.Message.Length switch
            {
                > 18 => TypedResults.BadRequest("Invalid data!"),
                _ => TypedResults.BadRequest(ex.Message)
            };
        }
    }

    private static async Task<IResult> UpdateProjectMembers(ProjectService projectService, string projName, List<ProjectMember> projectMembers)
    {
        try
        {
            Project project = await projectService.GetProjectAsync(projName);
            await projectService.UpdateProjectMembersAsync(projName, projectMembers);
            return TypedResults.Ok("Project members updated.");
        }
        catch (Exception ex)
        {
            return ex.Message.Length switch
            {
                > 18 => TypedResults.BadRequest("Invalid data!"),
                _ => TypedResults.BadRequest(ex.Message)
            };
        }
    }

    private static async Task<IResult> DeleteAllTerminatedProjects(ProjectService projectService)
    {
        await projectService.DeleteAllTerminatedProjectsAsync();
        return TypedResults.Ok("All Terminated projects deleted successfully.");
    }
}