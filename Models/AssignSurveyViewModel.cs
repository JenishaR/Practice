using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetEmployeeSurvey.Models
{
    public class AssignSurveyViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int SelectedSurveyId { get; set; }
        public List<SelectListItem> Surveys { get; set; }
    }

}
