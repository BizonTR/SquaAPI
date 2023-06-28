using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Business.Security.Abstract;
using Business.Security.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationService>();

            builder.RegisterType<UserStore<User>>().As<IUserStore<User>>();
            builder.RegisterType<ContextDb>().As<IdentityDbContext<User>>().SingleInstance();
            builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole>>();
            builder.RegisterType<ContextDb>().As<DbContext>().SingleInstance();
            builder.RegisterType<UserManager<User>>();
            builder.RegisterType<RoleManager<IdentityRole>>();
            builder.RegisterType<SignInManager<User>>();

            //var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
            //    .EnableInterfaceInterceptors(new ProxyGenerationOptions()
            //    {
            //        Selector = new AspectInterceptorSelector()
            //    }).SingleInstance();
        }
    }
}
