﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Commons
{
    public class APIResponse<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
    }
}
