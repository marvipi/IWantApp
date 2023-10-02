using IWantApp.API.Domain.Endpoints.Clients;
using IWantApp.API.Domain.Endpoints.Employees;
using IWantApp.API.Domain.Endpoints.Products;
using IWantApp.API.Domain.Endpoints.Security;
using IWantApp.API.Domain.Endpoints.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionStrings:IWantDb"]);
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
    options.AddPolicy("EmployeePolicy", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("EmployeeCode")
        .Build());
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateActor = true,
    ValidateAudience = true,
    ValidateIssuer = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero,
    ValidAudience = builder.Configuration["JwtTokenSettings:Audience"],
    ValidIssuer = builder.Configuration["JwtTokenSettings:Issuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtTokenSettings:SecretKey"])),
});

builder.Services.AddScoped<QueryAllUsersWithClaimName>();
builder.Services.AddScoped<UserCreator>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(EmployeeGet.Template, EmployeeGet.Methods, EmployeeGet.Handler);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler);
app.MapMethods(EmployeeDelete.Template, EmployeeDelete.Methods, EmployeeDelete.Handler);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler);
app.MapMethods(EmployeePut.Template, EmployeePut.Methods, EmployeePut.Handler);

app.MapMethods(ClientGet.Template, ClientGet.Methods, ClientGet.Handler);
app.MapMethods(ClientPost.Template, ClientPost.Methods, ClientPost.Handler);

app.MapMethods(CategoryGet.Template, CategoryGet.Methods, CategoryGet.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handler);
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);

app.MapMethods(ProductGet.Template, ProductGet.Methods, ProductGet.Handler);
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handler);
app.MapMethods(ProductGetShowcase.Template, ProductGetShowcase.Methods, ProductGetShowcase.Handler);
app.MapMethods(ProductDelete.Template, ProductDelete.Methods, ProductDelete.Handler);
app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handler);
app.MapMethods(ProductPut.Template, ProductPut.Methods, ProductPut.Handler);

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handler);

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) =>
{
    var error = http.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (error is not null)
    {
        switch (error)
        {
            case SqlException:
                return Results.Problem(title: "Database is offline", statusCode: 500);
            case BadHttpRequestException:
                return Results.Problem(title: "Type convertion error",
                    detail: "One of the values in the body of the request could not be converted to the expected type.",
                    statusCode: 500);
        }
    }

    return Results.Problem(title: "An error ocurred.", statusCode: 500);
});

app.Run();
