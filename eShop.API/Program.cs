using eShop.API.EF;
using eShop.Service.Catalog.Products;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//Add services to the container
builder.Services.AddDbContext<eShopDBContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Declare DI 
builder.Services.AddTransient<IPublicProductService, PublicProductService>();   

//Add controllers
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//Configure the HTTp request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
//Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShop v1");
});

//Define endpoints 
app.MapControllers();
app.Run();