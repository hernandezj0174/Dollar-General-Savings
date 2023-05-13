using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebScraper.Classes;


namespace WebScraper.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var x = Deals();
            return null;
        }

        public string Deals()
        {
            string url = "https://www.dollargeneral.com/bin/omni/coupons/search?searchText=&sortOrder=2&sortBy=0&numPageRecords=15&pageIndex=180&categories=&brands=&offerSourceType=1&mixMode=0&deviceId=41783169496082685948046190366069168309&clientOriginStoreNumber=";
            var response = CallUrl(url).Result;

            DollarGeneral dg = JsonSerializer.Deserialize<DollarGeneral>(response);

            return "";
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        public class Deal
        {
            public Deal(string brand, string savings, string description)
            {
                this.brand = brand;
                this.savings = savings;
                this.description = description;
            }
            public string brand;
            public string savings;
            public string description;
        }

    }
}
