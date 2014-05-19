using MongoDB.Driver;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    
    [Route("/hello")]
    [Route("/hello/{Name}")]
    public class Hello
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    [Authenticate]
    public class HomeAnalysisService : Service
    {
        public MongoClient DBClient;
        public MongoServer DBServer;
        public MongoDatabase DB;

        public HomeAnalysisService()
        {
            DBClient = new MongoClient(ConfigUtils.GetConnectionString("homeanalysis"));
            DBServer = DBClient.GetServer();
            DB = DBServer.GetDatabase("homeanalysis");
        }

        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    } 
}