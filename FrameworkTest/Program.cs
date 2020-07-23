using FrameworkTest.RpcControllers;
using FrameworkTest.Services;
using System;

namespace FrameworkTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //Natasha 4.0 注册编译组件
            NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
            NSucceedLog.Enabled = true;
            
            //注入接口和实现
            FrameworkService.AddInjection<DefaultTypeService>();
            FrameworkService.AddInjection<IHelloServices, DefaultHelloService>();
            FrameworkService.RefreshRpcControllers();


            //添加AOP
            Aop<HelloRpc>.Before["GetHello"] = (instance,parameters) => { Console.WriteLine("Aop In Before()!"); };
            Aop<HelloRpc>.After["GetHello"] = (instance, parameters) => { Console.WriteLine("Aop In After()!"); };


            //接收路由及参数
            string controller = args[0];
            string parameter = args[1];

            Console.WriteLine();

            //传递路由和参数
            var result = FrameworkCaller.Caller(controller, parameter);
            Console.WriteLine("Output : " + result);

            Console.WriteLine();
        }

    }

}
