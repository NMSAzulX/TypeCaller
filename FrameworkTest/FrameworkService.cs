using FrameworkTest.Controllers;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FrameworkTest
{
    public static class FrameworkService
    {
        public static readonly ConcurrentDictionary<Type, Type> _injectionMapping;
        static FrameworkService()
        {
            _injectionMapping = new ConcurrentDictionary<Type, Type>();
        }


        /// <summary>
        /// 重新生成映射
        /// </summary>
        public static void RefreshRpcControllers()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetExportedTypes();
            foreach (var item in types)
            {
                if (typeof(BaseRpc).IsAssignableFrom(item) && typeof(BaseRpc) != item)
                {
                    FrameworkCaller.HandlerType(item);
                }
            }

        }

        public static void AddInjection<TImplement>()
        {
            AddInjection<TImplement, TImplement>();
        }
        public static void AddInjection<TStandard, TImplement>()
        {
            _injectionMapping[typeof(TStandard)] = typeof(TImplement);
        }



        public static (StringBuilder script, string instanceName) HandlerCtor(Type type,int deepth = 0)
        {

            if (type != typeof(string) && type != typeof(object) && type.IsClass)
            {
                
                var instance = "instance" + (deepth == 0 ? "" : deepth.ToString());

                deepth += 1;

                var fields = type
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(item => item.IsInitOnly);


                StringBuilder ctorBuilder = new StringBuilder();
                if (fields.Count()>0)
                {

                    if (_injectionMapping.ContainsKey(type))
                    {
                        ctorBuilder.AppendLine($"var {instance} = new {_injectionMapping[type].GetDevelopName()}();");
                    }
                    else
                    {
                        ctorBuilder.AppendLine($"var {instance} = new {type.GetDevelopName()}();");
                    }

                    foreach (var item in fields)
                    {

                        var result = HandlerCtor(item.FieldType, deepth);
                        if (result != default)
                        {
                            ctorBuilder.Insert(0, result.script);
                            ctorBuilder.AppendLine($"Unsafe.AsRef({instance}.{item.Name})={result.instanceName};");
                        }

                    }
                    
                }
                else
                {

                    if (_injectionMapping.ContainsKey(type))
                    {
                        instance = $" new {_injectionMapping[type].GetDevelopName()}()";
                    }
                    else
                    {
                        instance = $" new {type.GetDevelopName()}()";
                    }

                }
                return (ctorBuilder, instance);


            }
            return default;

        }
    }
}
