using System.ComponentModel.DataAnnotations;
namespace SurveyApp.Models
{
    public class SurveyResponse
    {
        [Key]
        public string IpAddress { get; set; }
        public int Age { get; set; }
        public string FavouriteColour { get; set; }
    }
}