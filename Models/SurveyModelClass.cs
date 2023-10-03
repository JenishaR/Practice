using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetEmployeeSurvey.Models
{
    public class SurveyModelClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public List<Question> Questions { get; set; }
        
    }

    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }//12

        [Required]
        [StringLength(225)]
        public string Qns { get; set; }// where?when?

        [Required]
        public bool IsYesOrNo { get; set; }// yes no
    }
}
