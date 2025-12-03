using ApiProject.Data;
using ApiProject.Helper.Mapper;
using ApiProject.Service.Current;
using ApiProject.Service.Login;
using ApiProject.Service.School;
using ApiProject.Service.Student;
using ApiProject.Service.Report;
using ApiProject.Service.Transport;
using ApiProject.Service.SchoolExpenses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AutoMapper;
using System.Text;
using ApiProject.Service.SchoolFees;
using ApiProject.Service.Parent_Login;
using ApiProject.Service.StudentAttendance;
using ApiProject.Service.Employee;
using ApiProject.Service.Library;
using ApiProject.Service.Parents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});

builder.Services.AddHttpContextAccessor();
#region Service Resgister
builder.Services.AddScoped<ILoginUserService, LoginUserService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISchoolExpensesService, SchoolExpensesService>();
builder.Services.AddScoped<ISchoolFees, SchoolFees>();
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<IParentLoginService, ParentLoginService>();
builder.Services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
builder.Services.AddScoped<IEmployeeServide, EmployeeService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IParentsService, ParentsService>();

builder.Services.AddScoped<IAuthService, AuthService>();
#endregion
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("https://phonepejobs.com", "http://localhost:7000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.Use(async (context, next) =>
//    {
//        context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJVc2VySWQiOiIxIiwiU2Nob29sSWQiOiIxIiwiU2Vzc2lvbklkIjoiMSIsIlVzZXJOYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVIiwiZXhwIjoxNzQ4NTIyMzU4LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyODMvIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MjgzLyJ9.aa4Leuk_fJ7JVw3a2YlvhfQTk7AX7JmOhPP34b18iRg";
//        await next();
//    });
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
