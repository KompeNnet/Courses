using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rd_searching.Models
{
    public class BelChip
    {
        public List<Item> Parse(string query)
        {
            string request = @"http://belchip.by/search/?query=" + query;
            HtmlDocument html = (new HtmlWeb()).Load(request);

            var items = html.DocumentNode.SelectNodes("//*[@class='cat-item']");
            List<Item> results = new List<Item>();

            if (items != null)
            {
                foreach (HtmlNode item in items)
                {
                    Item exemplar = new Item()
                    {
                        ItemLink = @"http://belchip.by/" + item.SelectNodes(".//h3/a")[0].Attributes["href"].Value,
                        Name = item.SelectNodes(".//h3/a")[0].InnerText,
                        PictureLink = @"http://belchip.by/" + item.SelectNodes(".//a/img")?[1].Attributes["src"].Value,
                        Price = item.SelectNodes(".//*[@class='denoPrice']")?[0].InnerText.Split(' ')[0] ?? "цена по запросу"
                    };
                    if (exemplar.Price == "цена по запросу") exemplar.Existance = "по запросу";
                    else { exemplar.Existance = "на складе"; exemplar.Price += "BYN"; }
                    results.Add(exemplar);
                }
            }
            return results;
        }
    }
}