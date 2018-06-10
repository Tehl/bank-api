using IO.Swagger.Server.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BankApi.Server
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
            // add framework services
            services
                .AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = true
                    });
                });

            // add Swagger components
            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "BankApi",
                        Description = "Developer challenge using Swagger and ASP.NET Core"
                    });
                    c.CustomSchemaIds(type => type.FriendlyId(true));
                    c.DescribeAllEnumsAsStrings();
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // set up mvc framework
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseMvc()
                .UseDefaultFiles()
                .UseStaticFiles();

            // add Swagger components
            app
                .UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger.json", "BankApi"); });
        }
    }
}