using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Quartz
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class JobDescriptionAttribute : Attribute
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
    }
}
