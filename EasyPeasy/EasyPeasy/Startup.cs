using EasyPeasy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EasyPeasy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnString = Configuration.GetConnectionString("DefaultConnection");
        }

        public static IConfiguration Configuration { get; set;}
        public static string ConnString { get; set;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //  services.AddDbContext<EasyPeasyDBContext>(x=>
            // x.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));  

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyPeasy", Version = "v1" });
            });
            services.AddCors(x=>x.AddPolicy("postgres", builder=>{
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));
             services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
             services.AddControllers().AddNewtonsoftJson(x => 
 x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyPeasy v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
             app.UseCors("postgres");
             app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
