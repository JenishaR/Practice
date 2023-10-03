using AspNetEmployeeSurvey.Areas.Identity.Data;
using AspNetEmployeeSurvey.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AspNetEmployeeSurvey.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<EmployeeSurveyUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SurveyModelClass> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<EmployeeLogModel> LogTable { get; set; }
    public DbSet<SurveyAssignmentModel> SurveyAssignments { get; set; }
    public DbSet<NotificationModel> Notifications { get; set; }
    public DbSet<EmployeeToken> EmployeeTokens { get; set; }
    public DbSet<SurveyResponsePageModel> surveyResponsePageModels { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<SurveyResponseModel>().HasNoKey();
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new EmployeeSurveyUserEntityConfiguration());
    }
}

public class EmployeeSurveyUserEntityConfiguration : IEntityTypeConfiguration<EmployeeSurveyUser>
{
    public void Configure(EntityTypeBuilder<EmployeeSurveyUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
    }
}