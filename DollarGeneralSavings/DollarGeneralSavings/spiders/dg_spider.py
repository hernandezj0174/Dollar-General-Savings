from pathlib import Path

import scrapy 

class DGSpider(scrapy.Spider):
    name = "dg"

    def start_spider(self):
        urls = [
            "https://quotes.toscrape.com/page/1/"
        ]

        for url in urls:
            yield scrapy.Request(url=url, callback=self.parse)
    
    def parse(self, response):
        page = response.url.split("/")[-2]
        filename = f"dg-{page}.html"
        Path(filename).write_bytes(response.body)
        self.log(f"Saved file {filename}")