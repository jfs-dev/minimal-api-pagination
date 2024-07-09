using Microsoft.EntityFrameworkCore;
using minimal_api_pagination.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("minimal-api-pagination"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/Customers/", async (AppDbContext dbContext, int pageNumber = 1, int pageSize = 25) =>
{
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1 || pageSize > 25) pageSize = 25;

    var totalCount = await dbContext.Customers.CountAsync();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

    var customers = await dbContext.Customers
        .AsNoTracking()
        .OrderBy(x => x.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(new
    {
        pageNumber,
        totalPages,
        pageSize,
        totalCount,
        data = customers
    });
});

app.Run();
