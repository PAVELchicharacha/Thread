using Lab3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
IServiceCollection serviceCollection = builder.Services.AddDbContext<ModelDB>(options => options.UseSqlServer(connection));
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapGet("/api/PriceLists", async (ModelDB db) => await db.PriceLists!.ToListAsync());
app.MapGet("/api/PriceLists/{name}", async (ModelDB db, string name) => await db.PriceLists!.Where(u => u.Name == name).ToListAsync());

app.MapPost("/api/PriceList", async (PriceList PriceList, ModelDB db) =>
{
    await db.PriceLists!.AddAsync(PriceList);
    await db.SaveChangesAsync();
    return PriceList;
});
app.MapPost("/api/Product", async (Product product, ModelDB db) =>
{
    await db.products!.AddAsync(product);
    await db.SaveChangesAsync();
    return product;
});
app.MapDelete("/api/PriceList/{id:int}", async (int id, ModelDB db) =>
{
    PriceList? PriceList = await db.PriceLists!.FirstOrDefaultAsync(u => u.Id == id);
    if (PriceList == null) return Results.NotFound(new { message = "Группа не найдена" });
    db.PriceLists!.Remove(PriceList);
    await db.SaveChangesAsync();
    return Results.Json(PriceList);
});
app.MapDelete("/api/Product/{id:int}", async (int id, ModelDB db) =>
{
    Product? product = await db.products!.FirstOrDefaultAsync(u => u.Id == id);
    if (product == null) return Results.NotFound(new { message = "Студент не найден" });
    db.products!.Remove(product);
    await db.SaveChangesAsync();
    return Results.Json(product);
});
app.MapPut("/api/PriceList", async (PriceList PriceList, ModelDB db) =>
{
    PriceList? g = await db.PriceLists!.FirstOrDefaultAsync(u => u.Id == PriceList.Id);
    if (g == null) return Results.NotFound(new { message = "Группа не найдена" });
    g.Name = PriceList.Name;
    g.Coast = PriceList.Coast;
    g.Id = PriceList.Id;
    await db.SaveChangesAsync();
    return Results.Json(g);
});
app.MapPut("/api/product", async (Product product, ModelDB db) =>
{
    Product? st = await db.products!.FirstOrDefaultAsync(u => u.Id == product.Id);
    if (st == null) return Results.NotFound(new { message = "Группа не найдена" });
    st.SaleDate = product.SaleDate;
    st.Quantity = product.Quantity;
    st.ProductCoast = product.ProductCoast;
    await db.SaveChangesAsync();
    return Results.Json(st);
});
app.Run();
