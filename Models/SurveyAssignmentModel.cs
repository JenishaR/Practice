using AspNetEmployeeSurvey.Migrations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AspNetEmployeeSurvey.Areas.Identity.Data;

namespace AspNetEmployeeSurvey.Models
{
    public class SurveyAssignmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Users")]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AssignmentDate { get; set; }

       public string Status { get; set; }

        // Navigation properties for relationships
        public EmployeeSurveyUser Users { get; set; } // Replace ApplicationUser with your user class
        public SurveyModelClass Survey { get; set; } 
    }

  
}

