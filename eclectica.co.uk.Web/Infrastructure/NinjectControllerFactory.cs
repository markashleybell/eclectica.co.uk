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
                    .To<FormsAuthenticationProvider>();

                Bind<IUnitOfWork>()
                    .To<UnitOfWork>()
                    .InRequestScope()
                    .WithConstructorArgument("databaseFactory", new DbFactory());

                Bind<IEntryRepository>()
                    .To<EntryRepository>();

                Bind<IAuthorRepository>()
                    .To<AuthorRepository>();

                Bind<ITagRepository>()
                    .To<TagRepository>();

                Bind<ILinkRepository>()
                    .To<LinkRepository>();

                Bind<ICommentRepository>()
                    .To<CommentRepository>();

                Bind<IEntryServices>()
                    .To<EntryServices>();

                Bind<ICommentServices>()
                    .To<CommentServices>();

                Bind<ITagServices>()
                    .To<TagServices>();

                Bind<ILinkServices>()
                    .To<LinkServices>();
            }
        }
    }
}