using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LegalBricks.Interview.Test
{
    public class Program
    {
        // only session factory is thread safe and so a static session probably
        // isnt wanted: https://nhibernate.info/doc/nhibernate-reference/transactions.html#transactions-threads
        // split into a different project for separation of concerns

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
