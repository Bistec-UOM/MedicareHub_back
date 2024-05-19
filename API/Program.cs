using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;
using Models.DTO.Doctor;
using Services;
using Services.AdminServices;
using Services.AppointmentService;
using Services.LabService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("Recep", policy =>
        policy.RequireClaim("Role", "Receptionist"));
    options.AddPolicy("Doct", policy =>
        policy.RequireClaim("Role", "Doctor"));
    options.AddPolicy("Cash", policy =>
        policy.RequireClaim("Role", "Cashier"));
    options.AddPolicy("Lab", policy =>
        policy.RequireClaim("Role", "Lab Assistant"));
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value!.PadRight(64, '\0')))
    };
});

//for enable cores in react.js in below code for front end admin
builder.Services.AddCors(options => {
    options.AddPolicy("ReactJSDomain",
        policy => policy
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

builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<IRepository<Appointment>, Repository<Appointment>>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Unable_Date>,Repository<Unable_Date>>();

//Doctor-----------------------------------------------------------------

builder.Services.AddScoped<DoctorappoinmentService>();
builder.Services.AddScoped<IRepository<Prescription>, Repository<Prescription>>();
builder.Services.AddScoped<IRepository<AddDrugs>, Repository<AddDrugs>>();
builder.Services.AddScoped<IRepository<Prescript_drug>, Repository<Prescript_drug>>();
builder.Services.AddScoped<IRepository<LabReport>, Repository<LabReport>>();

//Pharmacy---------------------------------------------------------------
builder.Services.AddScoped<DrugsService>();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<IRepository<Drug>, Repository<Drug>>();

//Admin------------------------------------------------------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IRepository<Drug>, Repository<Drug>>();

//Lab--------------------------------------------------------------------
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<ValueService>();
builder.Services.AddScoped<DoctorAnalyticService>();

builder.Services.AddScoped<IRepository<ReportFields>,Repository<ReportFields>>();
builder.Services.AddScoped<IRepository<Test>, Repository<Test>>();
builder.Services.AddScoped<IRepository<LabReport>, Repository<LabReport>>();
builder.Services.AddScoped<IRepository<Record>, Repository<Record>>();

//Login-------------------------------------------------------------------
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<IRepository<Otp>, Repository<Otp>>();

//builder.Services.AddScoped<IAppointmentRepository, AppointmentService>();

//=================================================================================================================
//=================================================================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{ }
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

//pass policy name for react
app.UseCors("ReactJSDomain");

app.UseAuthorization();

app.MapControllers();

app.Run();
