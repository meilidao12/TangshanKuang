﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class Helper
    {
        public static string GetCurrentUri
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }
    }
}
