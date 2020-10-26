using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using XCentium.Infrastructure;

namespace XCentium
{
    public static class UnityMvcConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ISiteHtmlAgility, SiteHtmlAgility>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}