using Food.GateWaySolution.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

namespace Food.GateWaySolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddAppAuthentication();
            builder.Configuration.AddJsonFile("ocelot.json",optional:false,reloadOnChange:true);
            builder.Services.AddOcelot(builder.Configuration);



            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.UseOcelot().GetAwaiter().GetResult();
            app.Run();
        }
    }
}
