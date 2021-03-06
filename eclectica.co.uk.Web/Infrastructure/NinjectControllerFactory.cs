﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;
using System.Configuration;
using eclectica.co.uk.Domain.Concrete;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Service.Concrete;
using eclectica.co.uk.Service.Abstract;
using System.Data;
using System.Data.SqlServerCe;
using eclectica.co.uk.Domain.Entities;
using eclectica.co.uk.Web.Abstract;
using eclectica.co.uk.Web.Concrete;
using eclectica.co.uk.Caching;
using eclectica.co.uk.Caching.Concrete;
using eclectica.co.uk.Caching.Abstract;

namespace eclectica.co.uk.Web.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        // The kernel is the thing that can supply object instances
        private IKernel kernel = new StandardKernel(new NinjectConfig());

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;

            return (IController)kernel.Get(controllerType);
        }

        private class NinjectConfig : NinjectModule
        {
            public override void Load()
            {
                Bind<IFormsAuthenticationProvider>().To<FormsAuthenticationProvider>().InRequestScope();

                var serverType = ConfigurationManager.AppSettings["ServerType"];

                if(serverType == "SQLCE4")
                {
                    Bind<IConnectionFactory>()
                        .To<SqlCeConnectionFactory>()
                        .InRequestScope()
                        .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["SqlCe"].ConnectionString)
                        .WithConstructorArgument("serverType", DbServerType.SQLCE4);
                }
                else
                {
                    Bind<IConnectionFactory>()
                        .To<SqlConnectionFactory>()
                        .InRequestScope()
                        .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["Sql"].ConnectionString)
                        .WithConstructorArgument("serverType", (serverType == "SQL2008") ? DbServerType.SQL2008 : DbServerType.SQL2012);
                }

                Bind<IELMAHConnectionFactory>()
                        .To<ELMAHConnectionFactory>()
                        .InRequestScope()
                        .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["elmah-sqlservercompact"].ConnectionString);

                Bind<IConfigurationInfo>()
                        .To<ConfigurationInfo>()
                        .InRequestScope()
                        .WithConstructorArgument("cdn1", ConfigurationManager.AppSettings["CDN1"])
                        .WithConstructorArgument("cdn2", ConfigurationManager.AppSettings["CDN2"])
                        .WithConstructorArgument("imageLibraryFolder", ConfigurationManager.AppSettings["ImageLibraryFolder"])
                        .WithConstructorArgument("indexPageSize", Convert.ToInt32(ConfigurationManager.AppSettings["IndexPageSize"]))
                        .WithConstructorArgument("facebookApplicationID", ConfigurationManager.AppSettings["FacebookApplicationID"])
                        .WithConstructorArgument("facebookPageID", ConfigurationManager.AppSettings["FacebookPageID"])
                        .WithConstructorArgument("twitterConsumerKey", ConfigurationManager.AppSettings["TwitterConsumerKey"])
                        .WithConstructorArgument("twitterConsumerSecret", ConfigurationManager.AppSettings["TwitterConsumerSecret"])
                        .WithConstructorArgument("twitterAccessToken", ConfigurationManager.AppSettings["TwitterAccessToken"])
                        .WithConstructorArgument("twitterAccessTokenSecret", ConfigurationManager.AppSettings["TwitterAccessTokenSecret"])
                        .WithConstructorArgument("twitterAnywhereAPIKey", ConfigurationManager.AppSettings["TwitterAnywhereAPIKey"])
                        .WithConstructorArgument("errorDigestKey", ConfigurationManager.AppSettings["ErrorDigestKey"])
                        .WithConstructorArgument("error403Message", ConfigurationManager.AppSettings["Error403Message"])
                        .WithConstructorArgument("error404Message", ConfigurationManager.AppSettings["Error404Message"])
                        .WithConstructorArgument("error500Message", ConfigurationManager.AppSettings["Error500Message"]);

                Bind<IModelCache>().To<ModelCache>().InRequestScope();

                Bind<ICacheConfigurationInfo>()
                        .To<CacheConfigurationInfo>()
                        .InRequestScope()
                        .WithConstructorArgument("cacheIntervalMin", Convert.ToInt32(ConfigurationManager.AppSettings["CacheIntervalMin"]))
                        .WithConstructorArgument("cacheIntervalShort", Convert.ToInt32(ConfigurationManager.AppSettings["CacheIntervalShort"]))
                        .WithConstructorArgument("cacheIntervalMedium", Convert.ToInt32(ConfigurationManager.AppSettings["CacheIntervalMedium"]))
                        .WithConstructorArgument("cacheIntervalLong", Convert.ToInt32(ConfigurationManager.AppSettings["CacheIntervalLong"]))
                        .WithConstructorArgument("cacheIntervalMax", Convert.ToInt32(ConfigurationManager.AppSettings["CacheIntervalMax"]));

                Bind<IEntryRepository>().To<EntryRepository>().InRequestScope();
                Bind<IAuthorRepository>().To<AuthorRepository>().InRequestScope();
                Bind<ITagRepository>().To<TagRepository>().InRequestScope();
                Bind<ILinkRepository>().To<LinkRepository>().InRequestScope();
                Bind<ICommentRepository>().To<CommentRepository>().InRequestScope();
                Bind<IImageRepository>().To<ImageRepository>().InRequestScope();
                Bind<IRedirectRepository>().To<RedirectRepository>().InRequestScope();
                
                Bind<IEntryServices>().To<CachingEntryServices>().InRequestScope();
                Bind<IEntryServices>().To<EntryServices>().When(r => r.Target.Name == "nonCachingEntryServices").InRequestScope();

                Bind<ICommentServices>().To<CommentServices>().InRequestScope();
                
                Bind<ITagServices>().To<CachingTagServices>().InRequestScope();
                Bind<ITagServices>().To<TagServices>().When(r => r.Target.Name == "nonCachingTagServices").InRequestScope();

                Bind<ILinkServices>().To<LinkServices>().InRequestScope();
                Bind<IRedirectServices>().To<RedirectServices>().InRequestScope();
            }
        }
    }
}