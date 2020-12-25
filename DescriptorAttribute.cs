using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrawling.Enums
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ClassDescriptionAttribute : Attribute
    {
        public Type User { get; }

        public ClassDescriptionAttribute(Type user)
        {         
            User = user;
        }
    }
}
