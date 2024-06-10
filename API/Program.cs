using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;
using Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using DotNetEnv;
using Models.DTO.Doctor;
using Services.AdminServices;
using Services.AppointmentService;
using Services.LabService;
using API;
using AppointmentNotificationHandler;


var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer {token}'"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configure Authorization
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
    options.AddPolicy("Doct&Recep", policy =>
       policy.RequireAssertion(context =>
           context.User.HasClaim(c => c.Type == "Role" && (c.Value == "Receptionist" || c.Value == "Doctor"))));
});

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value.PadRight(64, '\0')))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


// Enable CORS for React.js
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactJSDomain", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure Entity Framework and Dependency Injection for services and repositories
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultString"));
});


//=================================================================================================================
//                                Injections list
//=================================================================================================================

//Receptionist-----------------------------------------------------------

builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<IRepository<Appointment>, Repository<Appointment>>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Unable_Date>, Repository<Unable_Date>>();
builder.Services.AddScoped<IRepository<Notification>, Repository<Notification>>();


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

builder.Services.AddScoped<NotificationService>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IRepository<Drug>, Repository<Drug>>();

//Lab--------------------------------------------------------------------
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<ValueService>();
builder.Services.AddScoped<DoctorAnalyticService>();

builder.Services.AddScoped<IRepository<ReportFields>, Repository<ReportFields>>();
builder.Services.AddScoped<IRepository<Test>, Repository<Test>>();
builder.Services.AddScoped<IRepository<LabReport>, Repository<LabReport>>();
builder.Services.AddScoped<IRepository<Record>, Repository<Record>>();

//Login-------------------------------------------------------------------
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<IRepository<Otp>, Repository<Otp>>();

//builder.Services.AddScoped<IAppointmentRepository, AppointmentService>();

//=================================================================================================================
//=================================================================================================================

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{ }
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication is added before authorization
app.UseAuthorization();
app.UseCors("ReactJSDomain");

// Map controllers and SignalR hub
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<AppointmentNotificationHub>("/appointmentnotificationHub");


app.Run();
