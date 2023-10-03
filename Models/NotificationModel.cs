namespace AspNetEmployeeSurvey.Models
{
    public class NotificationModel
    {
      
            public int ID { get; set; }
            public string EmployeeEmail { get; set; }
            public string NotificationMessage { get; set; }
            public bool IsRead { get; set; }
            public DateTime NotificationDate { get; set; }
            public string NotificationLink { get; set; }
        
    }
}
