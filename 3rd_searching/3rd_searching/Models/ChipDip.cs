using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rd_searching.Models
{
    public class ChipDip
    {
        public List<Item> Parse(string query)
        {
            string request = @"https://www.ru-chipdip.by/search?searchtext=" + query;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(request);
            int? pageCount = htmlDoc.DocumentNode.SelectNodes("//*[@id='pager']/ul")?[0].SelectNodes("./li").Count ?? 1;
            List<Item> result = new List<Item>();

            for (int i = 1; i <= pageCount; i++)
            {
                string url = request + $"&page={i}";
                htmlDoc = web.Load(url);
                HtmlNodeCollection table = htmlDoc.DocumentNode.SelectNodes("//*[@id='search_items']");
                HtmlNodeCollection items;
                try { items = table[0].SelectNodes("//*[@class='with-hover']"); }
                catch { break; }
                foreach (var item in items)
                {
                    Item chip = new Item()
                    {
                        ItemLink = @"https://www.ru-chipdip.by" + item.SelectNodes(".//*[@class='link']")[0].Attributes["href"].Value,
                        Name = item.SelectNodes(".//*[@class='link']")[0].InnerText,
                        ImageLink = item.SelectNodes(".//img")?[0].Attributes["src"].Value,
                        Price = item.SelectNodes(".//*[@class='price_mr']")[0].InnerText.Split(' ')[0].Replace('.', ',') + " BYN",
                        Existance = item.SelectNodes(".//*[@class='item__avail_available']")?[0].InnerText ?? item.SelectNodes(".//*[@class='item__avail_order']")?[0].InnerText
                    };
                    result.Add(chip);
                }
            }
            return result;
        }
    }
}