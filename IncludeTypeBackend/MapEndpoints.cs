namespace IncludeTypeBackend;

public static class MapEndpoints
{
    public static void MapAllEndpoints(this WebApplication app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api");
        endpoints.MapUserEndpoints();
        endpoints.MapProjectEndpoints();
        endpoints.MapProjectTaskEndpoints();
    }
}