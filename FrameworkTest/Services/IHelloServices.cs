using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkTest.Services
{
    public abstract class IHelloServices
    {
        protected readonly DefaultTypeService _typeService;
        public abstract string GetHello(string name);
    }
}
