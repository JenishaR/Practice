using AspNetEmployeeSurvey.Areas.Identity.Data;

namespace AspNetEmployeeSurvey
{
    public class AssignmentStatusUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _applicationDbContext;

        public AssignmentStatusUpdaterService(IServiceProvider serviceProvider, ApplicationDbContext applicationDbContext)
        {
            _serviceProvider = serviceProvider;
            _applicationDbContext = applicationDbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    DateTime expirationThreshold = DateTime.UtcNow.AddMinutes(-10);
                    var expiredAssignments = dbContext.SurveyAssignments
                        .Where(assignment => assignment.Status == "Active" && assignment.AssignmentDate <= expirationThreshold)
                        .ToList();

                    foreach (var assignment in expiredAssignments)
                    {
                        assignment.Status = "Expired";
                    }

                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Run every hour
            }
        }
    }

}
