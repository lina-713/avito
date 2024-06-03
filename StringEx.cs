using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avito
{
    public static class StringEx
    {
        public static bool IsEmpty(this string s)
        {
            return s == null || s.Length == 0;
        }
    }
}
