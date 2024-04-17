var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Dodaj obs³ugê sesji
builder.Services.AddDistributedMemoryCache(); // Dodaj pamiêæ podrêczn¹ rozproszon¹
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ustaw czas wygaœniêcia sesji
    options.Cookie.HttpOnly = true; // Ustaw flagê HttpOnly dla ciasteczka sesji
    options.Cookie.IsEssential = true; // Oznacz ciasteczko sesji jako niezbêdne
});

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
