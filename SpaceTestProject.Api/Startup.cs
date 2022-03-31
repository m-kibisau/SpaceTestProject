using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SpaceTestProject.Application.Options;
using SpaceTestProject.Application.Services.ImdbApiService;
using SpaceTestProject.Application.Titles.Queries.GetAll;
using SpaceTestProject.Persistence;
using SpaceTestProject.Persistence.Abstractions;
using SpaceTestProject.Persistence.Contexts;

namespace SpaceTestProject.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpaceTestProject", Version = "v1" });
            });
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ImdbIntegrationDb"),
                    b => b.MigrationsAssembly("SpaceTestProject.Api")));

            services.Configure<ImdbSettingsOptions>(Configuration.GetSection(ImdbSettingsOptions.SECTION_NAME));
            services.AddTransient<IImdbApiService, ImdbApiService>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddHttpClient();
            services.AddMediatR(typeof(GetAllTitlesQuery).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpaceTestProject v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
