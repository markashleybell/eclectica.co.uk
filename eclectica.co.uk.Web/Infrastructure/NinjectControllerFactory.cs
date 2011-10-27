using System;
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
                Bind<IFormsAuthenticationProvider>()
                    .To<FormsAuthenticationProvider>()
                    .InRequestScope();

                Bind<IConnectionFactory>()
                    .To<SqlCeConnectionFactory>()
                    .InRequestScope()
                    .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["Db"].ConnectionString);

                Bind<IEntryRepository>()
                    .To<EntryRepository>()
                    .InRequestScope();

                Bind<IAuthorRepository>()
                    .To<AuthorRepository>()
                    .InRequestScope();

                Bind<ITagRepository>()
                    .To<TagRepository>()
                    .InRequestScope();

                Bind<ILinkRepository>()
                    .To<LinkRepository>()
                    .InRequestScope();

                Bind<ICommentRepository>()
                    .To<CommentRepository>()
                    .InRequestScope();

                Bind<IEntryServices>()
                    .To<EntryServices>()
                    .InRequestScope();

                Bind<ICommentServices>()
                    .To<CommentServices>()
                    .InRequestScope();

                Bind<ITagServices>()
                    .To<TagServices>()
                    .InRequestScope();

                Bind<ILinkServices>()
                    .To<LinkServices>()
                    .InRequestScope();
            }
        }
    }
}