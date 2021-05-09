using System;
using System.Reflection;
using System.Linq;
using Castle.DynamicProxy;
using Core.Aspects.Autofac.Exception;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public AspectInterceptorSelector()
        {
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes); //Burasi Webapi de metodlarin üzerine bakip attributes varsa onlari ekliyor. Yazdigimiz aspect attribute ler bu sekilde ekleniyor.
            classAttributes.Add(new ExceptionLogAspect(typeof(DatabaseLogger))); //Tüm projede calismasini istedigimiz icin log isleminin buraya yazdik
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
