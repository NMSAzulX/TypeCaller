using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkTest.Services
{
    public class DefaultTypeService
    {
        public virtual void Show()
        {
            Console.WriteLine("Run : In TypeService! Means : Dependency injection Succeed! ");
        }
    }
}
