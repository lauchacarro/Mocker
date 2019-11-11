using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mocker.Web.Extensions.Middlewares;
using Mocker.Web.Models.Settings;
using Mocker.Web.Services.Abstracts;
using Mocker.Web.Services.Concretes;

namespace Mocker.Web
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
            services.AddControllers().AddXmlSerializerFormatters();
            services.AddOptions();
            services.Configure<GitHubSetting>(Configuration.GetSection(nameof(GitHubSetting)));
            services.AddSingleton<IGitHubService, GitHubService>();
            services.AddTransient<IMockService, MockService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IGetMockMiddlewareService, GetMockMiddlewareService>();
            services.AddTransient<IReverseProxyService, ReverseProxyService>();
            services.AddHttpClient();
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseRouting();

            app.UseDelay();
            app.UseMocker();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
