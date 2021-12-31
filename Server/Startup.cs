using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using QuizFoot.Server.Hubs;
using QuizFoot.Server.Abstractions;
using QuizFoot.Server.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using QuizFoot.Abstractions;
using QuizFoot.Core;
using QuizFoot.Domain;

namespace QuizFoot.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<QuizDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });
            services.AddTransient<ILobbyRepository, LobbyRepository>();
            services.AddTransient<ICodeGenerator, CodeGenerator>();
            services.AddTransient<IQuizRepository, QuizRepository>();
            services.AddTransient<IQuizFootUnitOfWork, QuizFootUnitOfWork>();

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<QuizDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, QuizDbContext>()
                .AddProfileService<UserIdClaimsProvider>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>,ConfigureJwtBearerOptions>();

            services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<LobbyHub>("/lobbyHub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
