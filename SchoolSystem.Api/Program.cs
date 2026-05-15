using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SchoolSystem.Api.Data;
using SchoolSystem.Api.Handlers;
using SchoolSystem.Api.Services;
using SchoolSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication only (Cookie auth is in SchoolSystem.Web)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234567890";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SchoolSystem";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SchoolSystem";

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Configure Authorization
builder.Services.AddAuthorization(options => {
    // Policy-based authorization
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.Requirements.Add(new SuperAdminRequirement()));

    options.AddPolicy("TeacherOnly", policy =>
        policy.Requirements.Add(new TeacherRequirement()));

    options.AddPolicy("HomeroomOnly", policy =>
        policy.Requirements.Add(new HomeroomRequirement()));

    options.AddPolicy("ParentOnly", policy =>
        policy.Requirements.Add(new ParentRequirement()));

    options.AddPolicy("TeacherOrHomeroom", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Teacher") ||
            context.User.IsInRole("Homeroom") ||
            context.User.IsInRole("SuperAdmin")));

    options.AddPolicy("SuperAdminOrHomeroom", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("SuperAdmin") ||
            context.User.IsInRole("Homeroom")));

    options.AddPolicy("SuperAdminOrHomeroomOrTeacher", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("SuperAdmin") ||
            context.User.IsInRole("Homeroom") ||
            context.User.IsInRole("Teacher")));

    options.AddPolicy("SuperAdminOrParent", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("SuperAdmin") ||
            context.User.IsInRole("Parent")));
});

// Register custom authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, SuperAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TeacherHandler>();
builder.Services.AddScoped<IAuthorizationHandler, HomeroomHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ParentHandler>();

// Register application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAcademicService, AcademicService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IGradebookService, GradebookService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IMonthlyScoreService, MonthlyScoreService>();
builder.Services.AddScoped<IMonthlyReportService, MonthlyReportService>();
builder.Services.AddScoped<ISemesterReportService, SemesterReportService>();
builder.Services.AddScoped<IYearlyReportService, YearlyReportService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
