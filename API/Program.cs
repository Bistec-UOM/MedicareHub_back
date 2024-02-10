using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defaultString"));
});


//=================================================================================================================
//                                Injections list
//=================================================================================================================

//Receptionist-----------------------------------------------------------

//Doctor-----------------------------------------------------------------

//Pharmacy---------------------------------------------------------------

//Admin------------------------------------------------------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();

//Lab--------------------------------------------------------------------
builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<ICrud<Test>,Test<Test>>();

//=================================================================================================================
//=================================================================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
