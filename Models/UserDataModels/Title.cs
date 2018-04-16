using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SBTCustomerManager.Data;
using SBTCustomerManager.Models;
using SBTCustomerManager.Models.AccountViewModels;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.RoleViewModels;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Services;

namespace SBTCustomerManager.Models.UserDataModels
{
    public class Title
    {
        #region Public Properties
        public long Id { get; set; }
        public string Value { get; set; }
        #endregion

        #region Public Methods
        public static List<SelectListItem> GetTitles(List<Title> resultList)
        {

            List<SelectListItem> titles = new List<SelectListItem>();

            foreach (var item in resultList)
            {
                titles.Add(new SelectListItem { Value = item.Value, Text = item.Value });
            }

            return titles;
        }
        #endregion
    }
}
