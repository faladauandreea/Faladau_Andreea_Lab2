
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
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LibraryIdentityContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/BookPages");
    options.Conventions.AllowAnonymousToPage("/BookPages/Index");
    options.Conventions.AllowAnonymousToPage("/BookPages/Details");
    options.Conventions.AuthorizeFolder("/MemberPages", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Publishers", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/CategoryPages", "AdminPolicy");
});

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
