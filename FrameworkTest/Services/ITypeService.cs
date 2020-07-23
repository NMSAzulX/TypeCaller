using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkTest.Services
{
    public class ITypeService
    {
        public virtual void Show()
        {
            Console.WriteLine("Run : In TypeService! Means : Dependency injection Succeed! ");
        }
    }
}
