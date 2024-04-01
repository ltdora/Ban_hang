using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class BaseRespone<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
        public string Type { get; set; }

    }
}
