﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online
{
    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {
        }
    }
}
