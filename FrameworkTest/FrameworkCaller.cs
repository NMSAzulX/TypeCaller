using Natasha.CSharp;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace FrameworkTest
{
    public class FrameworkCaller
    {
        public static readonly ConcurrentDictionary<string, Func<object[],object>> _invokerMapping;
        static FrameworkCaller()
        {
            _invokerMapping = new ConcurrentDictionary<string, Func<object[], object>>();
        }

        public static object Caller(string caller,params object[] parameters)
        {
            return _invokerMapping[caller](parameters);
        }
        
        public static void HandlerType<T>()
        {
            HandlerType(typeof(T));
        }
        public static void HandlerType(Type type)
        {

            var methods = type.GetMethods();
            foreach (var item in methods)
            {


                var methodCallBuilder = new StringBuilder();
                methodCallBuilder.Append('(');

                var methodParameterBuilder = new StringBuilder();

                foreach (var parameter in item.GetParameters().OrderBy(item=>item.Position))
                {
                    methodParameterBuilder.AppendLine($"var {parameter.Name} = ({parameter.ParameterType.GetDevelopName()})arg[{parameter.Position}];");
                    methodCallBuilder.Append($"{parameter.Name},");
                }

                if (methodCallBuilder.Length>1)
                {
                    methodCallBuilder.Length -= 1;
                    methodCallBuilder.Append(')');
                }
                else
                {
                    methodCallBuilder.Clear();
                }

                //var ctorInfo = FrameworkService.HandlerCtor(type);
                _invokerMapping[$"{type.Name[0..^3]}.{item.Name}"] = NDelegate
                    .RandomDomain(item=> { 
                        item
                        .LogSyntaxError()
                        .UseFileCompile();
                    })
                    .SetClass(item=>item.AllowPrivate(type))
                    .Func<object[], object>(@$"
{methodParameterBuilder}
{FrameworkService.HandlerCtor(type).script}
if(Aop<{type.GetDevelopName()}>.Before.TryGetValue(""{item.Name}"", out var beforeFunc)){{ beforeFunc(instance,arg); }}
var result = instance.{item.Name}({methodCallBuilder});
if(Aop<{type.GetDevelopName()}>.After.TryGetValue(""{item.Name}"", out var afterFunc)){{ afterFunc(instance,arg); }}
return result;
");
            }

        }

    }
}
