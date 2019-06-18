using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    class PocoSingleton<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance => _instance ?? (_instance = new T());
    }
}
