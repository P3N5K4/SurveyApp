using System.Data.Entity;
using System.Web.Http;
using SurveyApp.Models;
using System.Collections.Generic;
using System.Linq;
namespace SurveyApp.Controllers
{
    public class SurveyController : ApiController
    {
        protected SurveyDataContext dataContext = new SurveyDataContext();

        [HttpPost]
        public SurveyResponse PostResponse(SurveyResponse response)
        {
            // check that the response has valid data
            if (response == null || !ModelState.IsValid) return null;

            // determine the user's IP address
            response.IpAddress = GetUserIpAddress();

            // check if there is already a response from this IP address
            var dbResponse = dataContext.SurveyResponses.Find(response.IpAddress);
            if (dbResponse != null)
            {
                // we already have a response from this IP address, so update it
                dbResponse.Age = response.Age;
                dbResponse.FavouriteColour = response.FavouriteColour;
                dataContext.Entry(dbResponse).State = EntityState.Modified;
            }
            else
            {
                // no response from this IP address yet, so create a new one
                dataContext.SurveyResponses.Add(response);
            }

            // save the changes we just made
            dataContext.SaveChanges();
            return dbResponse;
        }

        [HttpGet]
        public IEnumerable<SurveyResponse> GetResponses()
        {
            // return all of the responses in the database
            return dataContext.SurveyResponses.ToList();
        }

        // helper method to determine the user's IP address
        private string GetUserIpAddress()
        {
            // based on stackoverflow response at http://stackoverflow.com/questions/735350/how-to-get-a-users-client-ip-address-in-asp-net#answer-740431
            var context = System.Web.HttpContext.Current;
            var ipAddressList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddressList))
            {
                var ipAddresses = ipAddressList.Split(',');
                if (ipAddresses.Length > 0) return ipAddresses[0];
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}