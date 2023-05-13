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
            return null;
        }

        public List<Deal> Deals()
        {
            var response = GetScrapedPage("https://www.dollargeneral.com/deals/coupons?sort=0&sortorder=2&type=1");
            var deals = ParseHtml(response);
            return deals;
        }

        public List<Deal> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var coupons = htmlDoc.DocumentNode.Descendants("li")
                .Where(node => node.HasClass("coupons_results-list-item")).ToList();

            
            List<Deal> deals = new List<Deal>();

            foreach (var coupon in coupons)
            {
                var brand = coupon.ChildNodes.Descendants().Where(x => x.HasClass("deal-card__info")).ToList()[0].Descendants().Where(y => y.HasClass("deal-card__brand")).First().InnerText;
                var savings = coupon.ChildNodes.Descendants().Where(x => x.HasClass("deal-card__info")).ToList()[0].Descendants().Where(y => y.HasClass("deal-card__name")).First().InnerText;
                var description = coupon.ChildNodes.Descendants().Where(x => x.HasClass("deal-card__info")).ToList()[0].Descendants().Where(y => y.HasClass("deal-card__description")).First().InnerText;

                deals.Add(new Deal(brand, savings, description));
            }

            return deals;
        }

        public static string GetScrapedPage(string url)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                var pageRequestJson = new System.Net.Http.StringContent
                    (@"{'url':'" + url + "','renderType':'html','outputAsJson':false }");
                var response = client.PostAsync
                    ("https://PhantomJsCloud.com/api/browser/v2/a-demo-key-with-low-quota-per-ip-address/",
                    pageRequestJson).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
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
