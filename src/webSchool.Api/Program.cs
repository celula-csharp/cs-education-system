using application.Extensions;
using Infrastructure.Extensions;

var builder = WebApplication.CreateSlimBuilder(args);

// Conexión a DB
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Default connection string not found");

// Añadimos los controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Onions
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configurar la pipeline HTTP Request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapeamos las rutas de los controllers
app.MapControllers();

app.Run();
