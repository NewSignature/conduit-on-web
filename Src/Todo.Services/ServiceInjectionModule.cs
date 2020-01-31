using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services
{
    public class ServiceInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PasswordHashService>().AsSelf();
            builder.RegisterType<CreateUserService>().AsSelf();
            builder.RegisterType<AuthenticateUserService>().AsSelf();
        }
    }
}
