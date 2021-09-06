using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using NHibernate.Extensions.NpgSql;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate.NetCore;
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Cfg;
using NHibernate;

namespace iug
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
      var connectionString = Configuration.GetConnectionString("DefaultConnection");
      // services.AddNHibernate(connStr);
      // where is your hibernate.config path
      //   var path = System.IO.Path.Combine(
      //       AppDomain.CurrentDomain.BaseDirectory,
      //       "hibernate.config"
      //   );
      var mapper = new ModelMapper();
      mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);
      HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

      var configuration = new NHibernate.Cfg.Configuration();
      configuration.DataBaseIntegration(c =>
      {
        c.Dialect<NpgSqlDialet>();
        c.ConnectionString = connectionString;
        c.Driver<NpgSqlDriver>();
        c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
        //c.SchemaAction = SchemaAutoAction.Validate;
        c.LogFormattedSql = true;
        c.LogSqlInConsole = true;
      });
      configuration.AddMapping(domainMapping);
      // add NHibernate services;
      services.AddHibernate(configuration);

      services.AddControllersWithViews();

      // In production, the React files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/build";
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
    {
      // Use loggerFactory as NHibernate logger factory.
      loggerFactory.UseAsHibernateLoggerFactory();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
