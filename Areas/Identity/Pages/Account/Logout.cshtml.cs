// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AspNetEmployeeSurvey.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AspNetEmployeeSurvey.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<EmployeeSurveyUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<EmployeeSurveyUser> _userManager;
        public LogoutModel(SignInManager<EmployeeSurveyUser> signInManager, ILogger<LogoutModel> logger, ApplicationDbContext applicationDbContext, UserManager<EmployeeSurveyUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            var email = User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(email);
            if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var employeeLog = _applicationDbContext.LogTable.FirstOrDefault(a => a.EmailId == email &&  a.LogOutTime == null);          
                if (employeeLog != null)
                {
                    employeeLog.LogOutTime = DateTime.Now;
                    _applicationDbContext.SaveChanges();
                }
            }
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
