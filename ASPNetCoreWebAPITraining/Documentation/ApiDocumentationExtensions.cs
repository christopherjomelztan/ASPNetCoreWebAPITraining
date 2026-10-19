namespace ASPNetCoreWebAPITraining.Documentation;

public static class ApiDocumentationExtensions
{
    private const string OpenApiDocumentUrl = "/openapi/v1.json";
    private const string RedocUrl = "/redoc";

    private const string RedocPage = $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <title>Person API</title>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1" />
        </head>
        <body>
            <redoc spec-url="{{OpenApiDocumentUrl}}"></redoc>
            <script src="https://cdn.redoc.ly/redoc/latest/bundles/redoc.standalone.js"></script>
        </body>
        </html>
        """;

    public static IServiceCollection AddPersonApiDocumentation(this IServiceCollection services) =>
        services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
        {
            document.Info.Title = "Person API";
            return Task.CompletedTask;
        }));

    public static WebApplication MapPersonApiDocumentation(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapGet(RedocUrl, () => Results.Content(RedocPage, "text/html")).ExcludeFromDescription();
        return app;
    }
}
