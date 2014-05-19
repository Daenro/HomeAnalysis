using MongoDB.Driver;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebAPI.Models;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public class HomeAnalysisAppHost : AppHostBase
        {
            public HomeAnalysisAppHost() : base("Hello Web Services", typeof(HomeAnalysisService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                var appsettings = new AppSettings();
                Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] {
                    new CredentialsAuthProvider(),
                    new FacebookAuthProvider(appsettings)
                    { ConsumerKey="875567449125756", 
                     ConsumerSecret="f146190e765dadd04a6586c12416eec1"}
                }));
                Plugins.Add(new RegistrationFeature());

                var connectionString = ConfigUtils.GetConnectionString("homeanalysis");
                var dbName = "homeanalysis";
                var mongoClient = new MongoClient(connectionString);
                var server = mongoClient.GetServer();
                var db = server.GetDatabase(dbName);


                container.Register<ICacheClient>(new MemoryCacheClient());
                container.Register<IUserAuthRepository>(new MongoDBAuthRepository(db, true));
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new HomeAnalysisAppHost().Init();
        }

    }
}
