using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CRUDApplication.Data.Contexts;
using CRUDApplication.Business.Abstracts;
using CRUDApplication.Business.Concretes;
using CRUDApplication.Data.Repository;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Data.Repositories.Concretes;
using CRUDApplication.Core.MappingProfiles;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Veritabaný baðlantýsýný yapýlandýr
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

 


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// AutoMapper'ý hizmet olarak ekle
builder.Services.AddAutoMapper(typeof(MappingProfile));



// IRepository ile EfRepository sýnýfýný DI container'a kaydediyoruz
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
