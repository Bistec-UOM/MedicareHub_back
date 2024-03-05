using DataAccessLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Services;
using Services.AppointmentService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//for enable cores in react.js in below code for front end admin
builder.Services.AddCors(options => {
    options.AddPolicy("ReactJSDomain",
        policy => policy.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        );
});

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defaultString"));
});


//=================================================================================================================
//                                Injections list
//=================================================================================================================

//Receptionist-----------------------------------------------------------

//Doctor-----------------------------------------------------------------

builder.Services.AddScoped<PrescriptionService>();
builder.Services.AddScoped<IRepository<Prescription>, Repository<Prescription>>();
//Pharmacy---------------------------------------------------------------
builder.Services.AddScoped<DrugsService>();
builder.Services.AddScoped<IRepository<Drug>, Repository<Drug>>();

//Admin------------------------------------------------------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

//Lab--------------------------------------------------------------------
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<TemplateService>();

builder.Services.AddScoped<IRepository<ReportFields>,Repository<ReportFields>>();
builder.Services.AddScoped<IRepository<Test>, Repository<Test>>();


builder.Services.AddScoped<IAppointmentRepository, AppointmentService>();

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

//pass policy name for react
app.UseCors("ReactJSDomain");

app.UseAuthorization();

app.MapControllers();

app.Run();
