using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Data
{
    public class DataInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // register the context - one per request
            builder.RegisterType<TodoDataContext>()
                .As<IContext>()
                .InstancePerRequest();
        }
    }
}
