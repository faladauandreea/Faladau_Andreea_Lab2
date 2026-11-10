
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("Faladau_Andreea_Lab2Context")
    ?? throw new InvalidOperationException(
        "Connection string 'Faladau_Andreea_Lab2Context' not found.");

builder.Services.AddDbContext<Faladau_Andreea_Lab2Context>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<LibraryIdentityContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LibraryIdentityContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
