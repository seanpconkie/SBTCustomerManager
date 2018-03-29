﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SBTCustomerManager.Views.Company
{
    public static class UserNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string UserProfile => "UserProfile";

        public static string UserContact => "UserContact";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ContactNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserContact);

        public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserProfile);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
