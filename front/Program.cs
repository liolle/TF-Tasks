using apiExo.bll.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("MyApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5291/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddTransient<ITaskService,TaskAPIService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
RouteConfig.RegisterRoutes(app);

app.Run();
