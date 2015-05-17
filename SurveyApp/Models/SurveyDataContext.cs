using System.Data.Entity;
namespace SurveyApp.Models
{
    public class SurveyDataContext : DbContext
    {
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
    }
}