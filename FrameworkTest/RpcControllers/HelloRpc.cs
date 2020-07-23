using FrameworkTest.Controllers;
using FrameworkTest.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkTest.RpcControllers
{
    public class HelloRpc : BaseRpc
    {
        private readonly IHelloServices _helloServices;
        public string GetHello(string name)
        {
            return _helloServices.GetHello(name);
        }
    }


    public static class StaticAopHelloRpc 
    {

        public static Func<HelloRpc, string, string> BeforeGetHello;
        public static Func<HelloRpc, string, string> AfterGetHello;

    }

    public class HelloRpcProxy : HelloRpc
    {

        public new string GetHello(string name)
        {
            StaticAopHelloRpc.BeforeGetHello?.Invoke(this, name);
            var result = base.GetHello(name);
            StaticAopHelloRpc.AfterGetHello?.Invoke(this, name);
            return result;
        }

    }

}
