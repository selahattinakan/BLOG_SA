using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Extensions
{
    public static class ElasticsearchExtension
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration configuration)
        {
            string userName = configuration.GetSection("Elastic")["UserName"];
            string password = configuration.GetSection("Elastic")["Password"];
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName, password));
            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client); //Elastic Search singleton olarak kullanmanızı tavsiye ediyor.
        }
    }
}
