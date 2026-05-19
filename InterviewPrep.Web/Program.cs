using InterviewPrep.Shared.Contracts;
using InterviewPrep.Shared.Features;
using InterviewPrep.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton(new InventoryService(new Dictionary<string, int>
{
    ["LAPTOP-001"] = 10,
    ["MOUSE-010"] = 50,
    ["BAG-200"] = 20
}));
builder.Services.AddSingleton<OrderCalculationService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("FrontendDev");

app.MapGet("/api/interview/overview", () => Results.Ok(new { title = ".NET 8 Interview Prep Studio", summary = InterviewPrepCatalog.ProjectOverview }));

app.MapGet("/api/interview/topics", () => Results.Ok(InterviewPrepCatalog.Topics
    .Select(topic => new { title = topic.Title, detail = topic.Detail })));

app.MapGet("/api/interview/course", () => Results.Ok(InterviewPrepCatalog.CourseTopics));

app.MapGet("/api/interview/course/{id}", (string id) =>
{
    var topic = InterviewPrepCatalog.GetTopic(id);
    return Results.Ok(topic);
});

app.MapGet("/api/interview/questions", () => Results.Ok(InterviewPrepCatalog.PracticeQuestions));

app.MapGet("/api/interview/feature-map", () => Results.Ok(InterviewPrepCatalog.FeatureMap));

app.MapPost("/api/order/calculate", (OrderRequest request, OrderCalculationService calculator, TimeProvider timeProvider) =>
{
    var response = calculator.Calculate(request, timeProvider.GetUtcNow().UtcDateTime);
    return Results.Ok(response);
});

app.MapFallbackToFile("index.html");
app.Run();
