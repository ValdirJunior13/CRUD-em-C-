using ProjetoCrud.Data;
using Microsoft.EntityFrameworkCore;
using ProjetoCrud.Services;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options => ...);
builder.Services.AddScoped<ILeadService, LeadService>();

var app = builder.Build();



// ADICIONE ESTA LINHA PARA O SERVIÇO FUNCIONAR:


app.MapControllers();



app.Run();
