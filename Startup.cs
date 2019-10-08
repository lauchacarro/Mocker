using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mocker.Extensions;
using Mocker.Models.Settings;
using Mocker.Services.Abstracts;
using Mocker.Services.Concretes;

namespace Mocker
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc();

            services.AddOptions();
            services.Configure<GitHubSetting>(_configuration.GetSection(nameof(GitHubSetting)));

            services.AddSingleton<IGitHubService, GitHubService>();
            services.AddTransient<IMockService, MockService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IContentTypeService, ContentTypeService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseGetMock();
            app.UseMvc();
        }
    }
}
