using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplicationSewingCompany.Models;
using WebApplicationSewingCompany.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Html;

namespace WebApplicationSewingCompany
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = "Data Source=GOSHKO\\SQLEXPRESS;Initial Catalog=SewingCompany;Integrated Security=True";
            services.AddDbContext<SewingCompanyContext>(options => options.UseSqlServer(connection));
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<CashedCustomer>();
            services.AddScoped<CashedOrder>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string strResponse =
                    "<html><head><title>Info</title>" +
                    "<style> " +
                    "\r\n.text { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                    "\r\n</style></head>" +
                    "<body><div class='text'>" +
                        "<p><a href = '/'>To main</a></p>" +
                        "<p>Information:</p>" +
                        "Server: " + context.Request.Host +
                        "<BR> Path: " + context.Request.PathBase +
                        "<BR> Protocol: " + context.Request.Protocol +
                    "</div></body></html>";
                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.Map("/searchInfo1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string name = string.Empty;
                    if (context.Request.Cookies.ContainsKey("name"))
                    {
                        name = context.Request.Cookies["name"] ?? string.Empty;
                    }

                    string strResponse = "<html><head><title>SearchForm</title>" +
                                        "<meta charset = 'utf-8'>" +
                                        "<style> " +
                                        "\r\n.text { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                                        "\r\n</style></head><body><div class = 'text'" +
                                        "<p><a href = '/'>To main</a></p>" +
                                        "<form action = / >" +
                                            "Search customer by name:<br><input type = 'text', name = 'name', value = " + name + ">" +
                                            "<br><br><input type = 'submit' value = 'Submit' >" +
                                        "</form>" +
                                        "</body></html>";

                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("name");
            }, HandleSearch1);
            app.Map("/searchInfo2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string id = string.Empty;
                    if (context.Request.Cookies.ContainsKey("id"))
                    {
                        id = context.Request.Cookies["id"] ?? string.Empty;
                    }

                    string strResponse = "<html><head><title>SearchForm</title>" +
                                        "<meta charset = 'utf-8'>" +
                                        "<style> " +
                                        "\r\n.text { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                                        "\r\n</style></head><body><div class = 'text'" +
                                        "<p><a href = '/'>To main</a></p>" +
                                        "<form action = / >" +
                                            "Search order by Id:<br><input type = 'text', name = 'id', value = " + id + ">" +
                                            "<br><br><input type = 'submit' value = 'Submit' >" +
                                        "</form>" +
                                        "</body></html>";

                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("id");
            }, HandleSearch2);
            app.Map("/customers", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    CashedCustomer cashedCustomerService = context.RequestServices.GetService<CashedCustomer>();
                    IEnumerable<Customer> customers = cashedCustomerService.GetCustomers("Cust20", 20);
                    string HtmlString = "<html><head><title>Main</title>" +
                    "<style> " +
                    "\r\n.menu { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                    "\r\np { margin: 0;}" +
                    "\r\n</style></head>" +
                    "<body><div class='menu'>" +
                        "<p><a href = '/'>To main</a></p><br>" +
                        "<p>List of customers:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Name</th>" +
                        "<th>Orders</th>" +
                        "</tr>";
                    foreach (var customer in customers)
                    {
                        HtmlString += "<tr>" +
                        $"<td>{customer.Id}</td>" +
                        $"<td>{customer.Name}</td>";
                        string orders = string.Join("", customer.Orders.Select(x => "<option>Id: " + x.Id.ToString() + "</option>").ToArray());
                        HtmlString += $"<td>{(orders == "" ? "No" : $"<select>{orders}</select>")}</td>" +
                        "</tr>";
                    }
                    HtmlString += "</table></div></body></html>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/orders", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    CashedOrder cashedContractService = context.RequestServices.GetService<CashedOrder>();
                    IEnumerable<Order> orders = cashedContractService.GetOrders(20);
                    string HtmlString = cashedContractService.GetTable(orders);
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Run((context) =>
            {
                CashedCustomer cashedAuthorService = context.RequestServices.GetService<CashedCustomer>();
                CashedOrder cashedOrder = context.RequestServices.GetService<CashedOrder>();
                cashedAuthorService.AddCustomers("Cust20", 20);
                //cashedOrder.AddOrders("Ord20", 20);
                string HtmlString = "<HTML><HEAD><TITLE>Main</TITLE><style>" +
                    "\r\n.menu { font-family: Consolas; font-size: 30px; font-weight: bold; }" +
                    "\r\np { margin: 0; font-size: 35px}" +
                    "\r\n</style></head>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><div class='menu'>";
                HtmlString += "<BR><A href='/info'>Info</A></BR>";
                HtmlString += "<BR><A href='/customers'>Customers</A></BR>";
                HtmlString += "<BR><A href='/orders'>Orders</A></BR>";
                HtmlString += "<BR><A href='/searchInfo1'>Find customer</A></BR>";
                HtmlString += "<BR><A href='/searchInfo2'>Find order</A></BR>";
                HtmlString += "</div></BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }
        public static void HandleSearch1(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                CashedCustomer cached =
                    context.RequestServices.GetService<CashedCustomer>();
                if (context.Request.Cookies.ContainsKey("name"))
                {
                    context.Response.Cookies.Delete("name");
                }
                context.Response.Cookies.Append("name", context.Request.Query["name"]);
                await context.Response.WriteAsync(cached.GetTable(context.Request.Query["name"]));
            });
        }
        public static void HandleSearch2(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                CashedOrder cached =
                    context.RequestServices.GetService<CashedOrder>();
                if (context.Request.Cookies.ContainsKey("id"))
                {
                    context.Response.Cookies.Delete("id");
                }
                context.Response.Cookies.Append("id", context.Request.Query["id"]);
                await context.Response.WriteAsync(cached.GetTable(context.Request.Query["id"]));
            });
        }
    }
}