using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumsService.AsyncDataServices;
using ForumsService.Data;
using ForumsService.EventProcessing;
using ForumsService.SyncDataServices.Grpc;
using ForumsService.SyncDataServices.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace ForumsService
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
           
            services.AddDbContext<AppDbContext> (optionsAction =>
            optionsAction.UseInMemoryDatabase("InMemnam"));
            services.AddScoped<IForumRepo, ForumRepo>(); //IF they ask for IUser Repo we give them user repo
            services.AddControllers();
            services.AddHttpClient<ICommentDataClient, HttpCommentDataClient>();
            services.AddHostedService<MessageBusSubscriber>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddSingleton<IEventProcessor, EventProcessor>(); //Singletone ->> all time alongside 
             services.AddGrpc();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUserDataClient, UserDataClient>();  //Registering it
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForumsService", Version = "v1" });
            });

            Console.WriteLine($"--> Comment Service Endpoint is {Configuration["CommentService"]}");

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            
                
                app.UseSwagger(c=>
                c.RouteTemplate = "api/f/swagger/{documentName}/swagger.json");
            
            

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcForumService>();
                endpoints.MapGet("/protocols/forums.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protocols/forums.proto"));
                });
                
            });
            PrepDb.PrepPopulation(app);
        }
    }
}