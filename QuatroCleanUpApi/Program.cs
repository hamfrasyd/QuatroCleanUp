using QuatroCleanUpBackend.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<PicturesRepository>();
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EventAttendanceRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
 app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAny");

app.MapControllers();

app.Run();

//// Configure PlayFab settings
//PlayFabSettings.staticSettings.TitleId = "1C4FBC";
//PlayFabSettings.staticSettings.DeveloperSecretKey = "MOAHYJ3HPD5NWU14XAMEX5GMPYXOP3OMKNGX4PRKSZ6RBN3NWP";


