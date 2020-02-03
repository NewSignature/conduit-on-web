using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Services.Impl;

namespace Todo.Services
{
    public class ServiceInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PasswordHashService>().As<IPasswordHashService>();
            builder.RegisterType<CreateUserService>().As<ICreateUserService>();
            builder.RegisterType<AuthenticateUserService>().As<IAuthenticateUserService>();
            builder.RegisterType<SaveListService>().As<ISaveListService>();
            builder.RegisterType<SaveListItemService>().As<ISaveListItemService>();
            builder.RegisterType<DeleteService>().As<IDeleteService>();
        }
    }
}
