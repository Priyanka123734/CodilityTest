using CodilityTest.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodilityTest.Controllers
{
    public class SecondTechnicalChallenge : Controller
    {
        public int minValue = 0, maxValue = 0;
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Second Technical Challenge";
            await CalculateMinMaxIdFromJson();
            return Json(new { MinValue = minValue, MaxValue = maxValue });
        }
        public async Task CalculateMinMaxIdFromJson()
        {
            //Make a call to API
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/users/Hadley/orgs");

            var userAgent = new System.Net.Http.Headers.ProductInfoHeaderValue("User-Agent", "request");
            request.Headers.UserAgent.Add(userAgent);
            var response = await httpClient.SendAsync(request);

            //Read the result from API call
            string jsonString = await response.Content.ReadAsStringAsync();

            var jsonArray = JArray.Parse(jsonString);
            foreach (JObject root in jsonArray)
            {
                var idVal = (Int32)root["id"];
                if (minValue == 0 && maxValue == 0)
                {
                    minValue = idVal;
                    maxValue = idVal;
                }
                else
                {
                    if (idVal < minValue)
                    {
                        minValue = idVal;
                    }
                    if (idVal > maxValue)
                    {
                        maxValue = idVal;
                    }
                }
            }

        }
    }
}
