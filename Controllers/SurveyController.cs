using AspNetEmployeeSurvey.Areas.Identity.Data;
using AspNetEmployeeSurvey.Migrations;
using AspNetEmployeeSurvey.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AspNetEmployeeSurvey.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<EmployeeSurveyUser> _userManager;
        private readonly IConfiguration _configuration;

        public SurveyController(ApplicationDbContext applicationDbContext, UserManager<EmployeeSurveyUser> userManager, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _configuration = configuration;
        }
        public IActionResult CreateSurvey()
        {
            var surveyModel = new SurveyModelClass
            {
                Questions = new List<Question>() // Initialize the Questions property with an empty list or load questions from your data source.
            };
            return View(surveyModel);
        }

        [HttpPost]
        public IActionResult CreateSurvey(SurveyModelClass survey)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Surveys.Add(survey);
                _applicationDbContext.SaveChanges();

                return RedirectToAction("CreateSurvey"); // Redirect to a success page or elsewhere
            }

            return View("");
        }
        [HttpGet]
        public async Task<IActionResult> AssignEmployee()
        {
            List<EmployeeDetailModel> employeeList = new List<EmployeeDetailModel>();
            var employees = await _userManager.GetUsersInRoleAsync("Employee");

            foreach (var employee in employees)
            {
                EmployeeDetailModel employeeDetail = new EmployeeDetailModel
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email
                };

                employeeList.Add(employeeDetail);
            }

            return View(employeeList);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeActivity()
        {
            List<EmployeeDetailModel> employeeList = new List<EmployeeDetailModel>();
            var employees = await _userManager.GetUsersInRoleAsync("Employee");

            foreach (var employee in employees)
            {
                EmployeeDetailModel employeeDetail = new EmployeeDetailModel
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email
                };

                employeeList.Add(employeeDetail);
            }

            return View(employeeList);
        }

        [HttpGet]
        public IActionResult LogActivity(string id)
        {
            // List<EmployeeLogModel> employeeList = new List<EmployeeLogModel>();
            var employeeLogList = _applicationDbContext.LogTable.Where(x => x.EmailId == id).OrderByDescending(x => x.LogInTime).ToList();


            return View(employeeLogList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound(); // Handle invalid input
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(); // Handle user not found
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // Successfully deleted the user
                // You can perform additional actions or redirect as needed
                return RedirectToAction("EmployeeActivity"); // Redirect to the employee list page
            }
            else
            {
                // Failed to delete the user
                // Handle the error, display a message, or redirect as needed
                // You can access the error messages via result.Errors
                return View("Error"); // Redirect to an error page or display an error view
            }
        }
        public async  Task<IActionResult> Assign(string id)
        {
            var employee = await _userManager.FindByEmailAsync(id);
            //var employee = GetEmployeeById(employeeId); // Replace with your data access code

            if (employee == null)
            {
                return NotFound(); // Handle not found
            }

            // Retrieve all available surveys
            //var surveys = GetAllSurveys(); // Replace with your data access code
            var surveys = _applicationDbContext.Surveys.ToList();

            // Create the ViewModel
            var viewModel = new AssignSurveyViewModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Surveys = surveys.Select(survey => new SelectListItem
                {
                    Text = survey.Title,
                    Value = survey.Id.ToString()
                }).ToList()
            };

            return View(viewModel);
        }
        

        // Replace with your survey service or repository
        public List<SurveyModelClass> GetAllSurveys()
        {
            // Simulated data access - replace with actual database query
            var surveys = _applicationDbContext.Surveys.ToList();
            return surveys;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSurvey(int SelectedSurveyId, string FirstName, string LastName, string Email)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the selected survey ID
                int selectedSurveyId = SelectedSurveyId;

                // Retrieve employee information from the view model
                string firstName = FirstName;
                string lastName = LastName;
                string email = Email;

                //var newNotification = new NotificationModel
                //{
                //    EmployeeEmail = Email, // Replace with the actual employee's email
                //    NotificationMessage = "You have a new survey assigned.",
                //    IsRead = false,
                //    NotificationDate = DateTime.Now,
                //    NotificationLink = $"/Survey/Notification/{Email}" // Replace with the actual link to the survey
                //};
                //var user = await _userManager.FindByEmailAsync(email);
                //var userId = await _userManager.GetUserIdAsync(user);
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var callbackUrl = Url.Page(
                //     $"/Survey/SurveyResponse/{selectedSurveyId}",
                //    pageHandler: null,
                //    values: new { area = "Identity", userId = userId, code = code },
                //    protocol: Request.Scheme);


                var callbackUrl = Url.Action("SurveyResponse", "Survey", new { surveyId = selectedSurveyId }, Request.Scheme);

                await SendEmailAsync(Email, "You have a new survey assigned.",
                       $"Please take the survey by <a href='{(callbackUrl)}'>clicking here</a>.");


                //_applicationDbContext.Notifications.Add(newNotification);
                //_applicationDbContext.SaveChanges();
                

                AssignSurveyToEmployee(selectedSurveyId, email);

                // Redirect to a success page or return a view with a success message
                return RedirectToAction("AssignEmployee");
            }

            // If ModelState is not valid, return the view with validation errors
            return View("");
        }
        public bool AssignSurveyToEmployee(int surveyId, string userEmail)
        {
            try
            {
                // Retrieve the user by email
                var user = _applicationDbContext.Users.SingleOrDefault(u => u.Email == userEmail);

                if (user == null)
                {
                    // User not found
                    return false;
                }

                // Retrieve the survey by surveyId
                var survey = _applicationDbContext.Surveys.SingleOrDefault(s => s.Id == surveyId);

                if (survey == null)
                {
                    // Survey not found
                    return false;
                }

                // Assign the survey to the user (update your database as needed)
                // For example, you might create a new Assignment record in your database
                var assignment = new SurveyAssignmentModel
                {
                    UserId = user.Id,
                    SurveyId = survey.Id,
                    AssignmentDate = DateTime.Now ,// You can set the assignment date here
                    Status = "ACTIVE"
                };

                

                _applicationDbContext.SurveyAssignments.Add(assignment);
                _applicationDbContext.SaveChanges();

                return true; // Assignment successful
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the assignment
                // You can log the exception or return false if the assignment fails
                return false;
            }
        }

        public Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                string fromEmail = "noreplypoc217@gmail.com";
                string smtpServer = _configuration["EmailSettings:SmtpServer"];
                int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                string smtpUsername = "noreplypoc217@gmail.com";
                string smtpPassword = _configuration["EmailSettings:SmtpPassword"];

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, "cttkyxgmzbfqamgp");
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(fromEmail);

                        mailMessage.To.Add(new MailAddress(email));


                        mailMessage.Subject = subject;
                        mailMessage.Body = message;
                        mailMessage.IsBodyHtml = true; // Set to true if you want to send HTML email

                        smtpClient.Send(mailMessage);
                        return Task.FromResult(true);
                    }
                }
            }
            catch (SmtpException ex)
            {
                return Task.FromResult(false);
            }
        }

        public void AssignTokenToEmployee(string employeeEmail, string token)
        {
           
                var employeeToken = new EmployeeToken
                {
                    EmployeeEmail = employeeEmail,
                    Token = token
                };

                _applicationDbContext.EmployeeTokens.Add(employeeToken);
            _applicationDbContext.SaveChanges();
            
        }

        public IActionResult SurveyResponse( int surveyId)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // User is logged in, redirect to the survey response page
                return RedirectToAction("SurveyResponsePage", new { surveyId });
            }
            else
            {
                //string returnUrl = Url.Action("SurveyResponsePage", new { surveyId });

                // Construct the custom URL with "Identity" before "Account"
                //string customUrl = $"/Identity/Account/Login?returnUrl={returnUrl}";
                // User is not logged in, redirect to the login page with a return URL
                //return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("SurveyResponsePage", new { surveyId }) });
                 return Redirect("/Identity/Account/Login?returnUrl=" + Url.Action("SurveyResponsePage", new { surveyId }));
            }
        }

        // Survey response page
        public IActionResult SurveyResponsePage(int surveyId)
        {
            // Load the survey based on the surveyId and display it to the user
            // You may want to verify that the user has access to this survey
            var survey = GetSurveyById(surveyId);

            if (survey == null)
            {
                // Handle the case where the survey with the given ID doesn't exist
                return NotFound();
            }

            var viewModel = new SurveyResponsePageModel
            {
                Survey = survey,
                QuestionResponses = new List<QuestionResponse>()
            };

            return View(viewModel);
        }

        public SurveyModelClass GetSurveyById(int surveyId)
        {
            // Simulated data access - replace with actual database query
            var survey = _applicationDbContext.Surveys.FirstOrDefault(a => a.Id == surveyId);
            return survey;
        }

    }

}
