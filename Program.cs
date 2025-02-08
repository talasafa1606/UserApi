using UserApi.Filters;
using UserApi.Middlewares;
using UserApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddControllers(options =>
//    {
//        options.Filters.Add<LoggingActionFilter>();
//    });
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();//or AddSingleton or AddTransient
//i used scoped since using transient would create multiple instances which is unnecessary in our case
//singleton might be bit problematic due to simultaneous changes to the static list of users

builder.Services.AddControllers(options =>
       {
           options.Filters.Add<GlobalExceptionHandler>();
       });
builder.Services.AddScoped<LoggingActionFilter>();
builder.Services.AddScoped<IObjectMapperService, ObjectMapperService>();
builder.Services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


/*
	ExceptionHandler
	HSTS self signed certificate
	HTTPS redirection
	Static Files
	Routing
	CORS
	Authentication
	Authorzation
    custom as many needed.
*/
// app.UseExceptionHandler();
// app.UseHsts();
app.UseHttpsRedirection();
// app.UseStaticFiles();
app.UseRouting();
// app.UseCors();
// app.UseAuthentication();
// app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();


app.MapControllers();
app.Run();
