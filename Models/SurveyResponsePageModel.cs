using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetEmployeeSurvey.Models
{
    public class SurveyResponsePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        // List of responses for each question
        public List<QuestionResponse> QuestionResponses { get; set; }

        public SurveyModelClass Survey { get; set; }
    }

    public class QuestionResponse
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool IsYesOrNoQuestion { get; set; }
        public bool YesOrNoResponse { get; set; }
        public string TextResponse { get; set; }
    }
}
