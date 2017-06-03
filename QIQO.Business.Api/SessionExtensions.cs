using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QIQO.Business.Api
{
    //public static class SessionExtensions
    //{
    //    public static void Set<T>(this ISession session, string key, T value)
    //    {
    //        session.SetString(key, JsonConvert.SerializeObject(value));
    //    }

    //    public static T Get<T>(this ISession session, string key) where T: class, new()
    //    {
    //        // return session.GetString(key) as T;
    //        if (session.TryGetValue(key, out T val))
    //            return val;
    //        else
    //            return new T();            
    //    }

    //    //public static T CreateObject<T>() where T : class, IAppDomainSetup, new()
    //    //{
    //    //    return new T();
    //    //}
    //}
}
