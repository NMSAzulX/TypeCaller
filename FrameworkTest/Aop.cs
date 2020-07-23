using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FrameworkTest
{
    public static class Aop<TClass>
    {
        public static readonly ConcurrentDictionary<string, Action<TClass, object[]>> Before;
        public static readonly ConcurrentDictionary<string, Action<TClass, object[]>> After;

        static Aop()
        {
            Before = new ConcurrentDictionary<string, Action<TClass, object[]>>();
            After = new ConcurrentDictionary<string, Action<TClass, object[]>>();
        }
    }
}
