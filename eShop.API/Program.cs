using eShop.API.EF;
using eShop.Data.Entities;
using eShop.Service.Catalog.Products;
using eShop.Service.Common;
using eShop.Service.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<User,Role>()
    .AddEntityFrameworkStores<eShopDBContext>()
    .AddDefaultTokenProviders();


//Add services to the container
builder.Services.AddDbContext<eShopDBContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Declare DI 
builder.Services.AddTransient<IPublicProductService, PublicProductService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<IManageProductService, ManagerProductService>();
builder.Services.AddTransient<UserManager<User>, UserManager<User>>();
builder.Services.AddTransient<SignInManager<User>, SignInManager<User>>();
builder.Services.AddTransient<RoleManager<Role>, RoleManager<Role>>();
builder.Services.AddTransient<IUserService, UserService>(); 
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