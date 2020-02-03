using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Todo.Data;
using Todo.Services;
using Todo.Web.Context;

namespace Todo.Web.Injection
{
    public static class DependencyInstaller
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<SessionContext>().As<ISessionContext>().InstancePerRequest();

            builder.RegisterModule<DataInjectionModule>();
            builder.RegisterModule<ServiceInjectionModule>();

            return builder.Build();
        }
    }
}