using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetEmployeeSurvey.Models
{
    public class EmployeeLogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmailId { get; set; }
        public DateTime LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }


    }
}
