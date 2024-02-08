using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DB_EFCore.DataAccessLayer
{
    public class Initilazier
    {
        public static IConfiguration Configuration;

        public static void Build()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).AddJsonFile("dbsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();    
        }
    }
}
