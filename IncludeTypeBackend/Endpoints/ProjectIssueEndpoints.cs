namespace IncludeTypeBackend.Endpoints;

public static class ProjectIssueEndpoints
{
    public static IEndpointRouteBuilder MapProjectIssueEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpoints = builder.MapGroup("projectissue");

        endpoints.MapGet(
            "/getissues",
            ([FromServices] ProjectIssueService issueService) => GetIssues(issueService)
        )
        .WithName("GetIssues")
        .WithDescription("Get all the issues.")
        .WithOpenApi();

        endpoints.MapGet(
            "/gettotalissues",
            ([FromServices] ProjectIssueService issueService) => GetTotalIssues(issueService)
        )
        .WithName("GetTotalIssues")
        .WithDescription("Get the total number of issues.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getissuesbyauthor/{author}",
            ([FromServices] ProjectIssueService issueService, [FromRoute] string author) =>
                GetIssuesByAuthor(issueService, author)
        )
        .WithName("GetIssuesByAuthor")
        .WithDescription("Get all the issues created by a specific author.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getissuesbyusername/{username}",
            ([FromServices] ProjectIssueService issueService, [FromRoute] string username) =>
                GetIssuesByUsername(issueService, username)
        )
        .WithName("GetIssuesByUsername")
        .WithDescription("Get all the issues assigned to a specific user.")
        .WithOpenApi();

        endpoints.MapPost(
            "/updateissuesbyauthor/{author}",
            ([FromServices] ProjectIssueService issueService,
                [FromBody] ProjectIssueDto issueDto,
                [FromRoute] string author) =>
                UpdateIssuesByAuthor(issueService, issueDto, author)
        )
        .WithName("UpdateIssuesByAuthor")
        .WithDescription("Update all the issues created by a specific author.")
        .WithOpenApi();

        endpoints.MapPost(
            "/updateissuesbyusername/{username}",
            ([FromServices] ProjectIssueService issueService,
                [FromBody] ProjectIssueDto issueDto,
                [FromRoute] string username) =>
                UpdateIssuesByUsername(issueService, issueDto, username)
        )
        .WithName("UpdateIssuesByUsername")
        .WithDescription("Update all the issues assigned to a specific user.")
        .WithOpenApi();

        endpoints.MapGet(
            "/getissuesforgivendeadline/{key}",
            ([FromServices] ProjectIssueService issueService, [FromRoute] string key) =>
                GetIssuesForGivenDeadline(issueService, key)
        )
        .WithName("GetIssuesForGivenDeadline")
        .WithDescription("Get all the issues for a specific deadline.")
        .WithOpenApi();

        endpoints.MapPost(
            "/addissue",
            ([FromServices] ProjectIssueService issueService, [FromBody] ProjectIssue projectIssue) =>
                AddIssue(issueService, projectIssue)
        )
        .WithName("AddIssue")
        .WithDescription("Add a new issue.")
        .WithOpenApi();

        return endpoints;
    }

    private static async Task<IResult> GetIssues(ProjectIssueService issueService) =>
        TypedResults.Ok(await issueService.GetAllIssuesAsync());

    private static async Task<IResult> GetTotalIssues(ProjectIssueService issueService) =>
        TypedResults.Ok(await issueService.GetTotalIssuesAsync());

    private static async Task<IResult> GetIssuesByAuthor(ProjectIssueService issueService, string author)
    {
        List<ProjectIssue> projectIssues = await issueService.GetAllIssuesByAuthorAsync(author);
        projectIssues.Sort(new ProjectIssueComparer());
        return TypedResults.Ok(projectIssues);
    }

    private static async Task<IResult> GetIssuesByUsername(ProjectIssueService issueService, string username)
    {
        List<ProjectIssue> projectIssues = await issueService.GetAllIssuesByUsernameAsync(username);
        projectIssues.Sort(new ProjectIssueComparer());
        return TypedResults.Ok(projectIssues);
    }

    private static async Task<IResult> UpdateIssuesByAuthor(ProjectIssueService issueService, ProjectIssueDto issueDto, string author)
    {
        try
        {
            await issueService.UpdateAllIssuesByAuthorAsync(issueDto.Issues, author);
            return TypedResults.Ok("Project issues updated.");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid data!");
        }
    }

    private static async Task<IResult> UpdateIssuesByUsername(ProjectIssueService issueService, ProjectIssueDto issueDto, string username)
    {
        try
        {
            await issueService.UpdateAllIssuesByUsernameAsync(issueDto.Issues, username);
            return TypedResults.Ok("Project issues updated.");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid data!");
        }
    }

    private static async Task<IResult> GetIssuesForGivenDeadline(ProjectIssueService issueService, string key)
    {
        List<ProjectIssue> projectIssues = await issueService.GetAllIssuesForGivenDeadlineAsync(key);
        projectIssues.Sort(new ProjectIssueComparer());
        return TypedResults.Ok(projectIssues);
    }

    private static async Task<IResult> AddIssue(ProjectIssueService issueService, ProjectIssue projectIssue)
    {
        try
        {
            Guid guid = Guid.NewGuid();
            projectIssue.Id = $"{guid}";

            DateTime dateTime = DateTime.Now;
            projectIssue.Date = dateTime.ToString("MMM-dd-yyyy");

            DateTime dueDate = DateTime.Parse(projectIssue.Deadline);
            projectIssue.Deadline = dueDate.ToString("MMM-dd-yyyy");

            await issueService.AddIssueAsync(projectIssue);
            return TypedResults.Ok("Issue successfully added");
        }
        catch
        {
            return TypedResults.BadRequest("Invalid data!");
        }
    }
}