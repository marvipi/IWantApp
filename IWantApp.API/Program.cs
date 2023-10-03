using IWantApp.API.Domain.Endpoints.Security;
using IWantApp.API.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.MSSqlServer(context.Configuration.GetConnectionString("IWantDb"),
        sinkOptions: new MSSqlServerSinkOptions()
        {
            AutoCreateSqlTable = true,
            TableName = "LogAPI"
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

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
    options.AddPolicy("EmployeePolicy", p => p
        .RequireAuthenticatedUser()
        .RequireClaim("EmployeeCode"));
    options.AddPolicy("CPFPolicy", p => p
    .RequireAuthenticatedUser()
    .RequireClaim("CPF"));
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
builder.Services.AddScoped<QueryMostOrderedProducts>();
builder.Services.AddScoped<UserCreator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapMethods(EmployeeGet.Template, EmployeeGet.Methods, EmployeeGet.Handler).WithTags("Employee API");
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler).WithTags("Employee API");
app.MapMethods(EmployeeDelete.Template, EmployeeDelete.Methods, EmployeeDelete.Handler).WithTags("Employee API");
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler).WithTags("Employee API");
app.MapMethods(EmployeePut.Template, EmployeePut.Methods, EmployeePut.Handler).WithTags("Employee API");

app.MapMethods(ClientGet.Template, ClientGet.Methods, ClientGet.Handler).WithTags("Client API");
app.MapMethods(ClientPost.Template, ClientPost.Methods, ClientPost.Handler).WithTags("Client API");

app.MapMethods(CategoryGet.Template, CategoryGet.Methods, CategoryGet.Handler).WithTags("Category API");
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler).WithTags("Category API");
app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handler).WithTags("Category API");
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler).WithTags("Category API");
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler).WithTags("Category API");

app.MapMethods(ProductGet.Template, ProductGet.Methods, ProductGet.Handler).WithTags("Product API");
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handler).WithTags("Product API");
app.MapMethods(ProductGetShowcase.Template, ProductGetShowcase.Methods, ProductGetShowcase.Handler).WithTags("Product API");
app.MapMethods(ProductGetMostOrdered.Template, ProductGetMostOrdered.Methods, ProductGetMostOrdered.Handler).WithTags("Product API");
app.MapMethods(ProductDelete.Template, ProductDelete.Methods, ProductDelete.Handler).WithTags("Product API");
app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handler).WithTags("Product API");
app.MapMethods(ProductPut.Template, ProductPut.Methods, ProductPut.Handler).WithTags("Product API");

app.MapMethods(OrderGet.Template, OrderGet.Methods, OrderGet.Handler).WithTags("Order API");
app.MapMethods(OrderPost.Template, OrderPost.Methods, OrderPost.Handler).WithTags("Order API");

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handler).WithTags("Token API");

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
