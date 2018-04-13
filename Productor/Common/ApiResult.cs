using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Common
{
    [Serializable]
    public class ApiResult<T> where T : class, new()
    {
        public bool Successed { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
