using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SBTCustomerManager.Views.CompanyUser;

namespace SBTCustomerManager.Views.CompanyUser
{
    public static class UserNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string UserDetail => "UserDetail";

        public static string UserDetailNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserDetail);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
