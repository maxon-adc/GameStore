﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Web.Infrastructure.Attributes;

namespace GameStore.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogEventAttribute());
            filters.Add(new LogExceptionAttribute());
            filters.Add(new LogIpAttribute());
            filters.Add(new LogPerformanceAttribute());
        }
    }
}