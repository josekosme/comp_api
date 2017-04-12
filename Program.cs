using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace comp_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder() 
                .UseKestrel(options => 
                { 
                    options.UseHttps("cosmecert.pem", "xmlsecretkey"); 
                }) 
                .UseUrls("http://*:4000", "https://*:4200") 
                .UseContentRoot(Directory.GetCurrentDirectory()) 
                .UseStartup<Startup>() 
                .Build(); 

            // var host = new WebHostBuilder()
            //     .UseKestrel( options => {
            //         options.UseHttps(null, new X509Certificate2("") )
            //     .UseContentRoot(Directory.GetCurrentDirectory())
            //     //.UseIISIntegration()
            //     .UseUrls("http://*:8000")
            //     .UseStartup<Startup>()
            //     .Build();

            host.Run();
        }
    }
}
