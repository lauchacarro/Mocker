using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mocker.Extensions.Middlewares;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using Mocker.Services.Concretes;

namespace Mocker
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
