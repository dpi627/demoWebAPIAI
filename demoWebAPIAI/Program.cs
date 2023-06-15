using OpenAI.Managers;
using OpenAI;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/AskAi", async (string api_key, string content) =>
{
    var openAiService = new OpenAIService(new OpenAiOptions()
    {
        ApiKey = api_key
    });

    var prompt = $@"
        請用繁體中文回答
        ###
        {content}
        ###
    ";

    var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
    {
        Prompt = prompt,
        Model = Models.TextDavinciV3,
        MaxTokens = 100
    });

    return completionResult.Choices.FirstOrDefault().Text;
});

app.MapGet("/Read", () => { return 1; });
app.MapPut("/Update", () => { return 1; });
app.MapDelete("/Delete", () => { return 1; });

app.Run();