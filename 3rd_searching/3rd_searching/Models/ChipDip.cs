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
            return GetResultList(request);
        }

        private List<Item> GetResultList(string request)
        {
            List<Item> result = new List<Item>();
            for (int i = 1; i <= GetPageCount(request); i++) GetItemsFromPage(ref result, request, i);
            return result;
        }

        private void GetItemsFromPage(ref List<Item> result, string request, int i)
        {
            try
            {
                HtmlNodeCollection nodes = GetNodeCollection(request, i);
                foreach (HtmlNode node in nodes) result.Add(GetItem(node));
            }
            catch { }
        }

        private Item GetItem(HtmlNode node)
        {
            return new Item()
            {
                ItemLink = @"https://www.ru-chipdip.by" + node.SelectNodes(".//*[@class='link']")[0].Attributes["href"].Value,
                Name = node.SelectNodes(".//*[@class='link']")[0].InnerText,
                ImageLink = node.SelectNodes(".//img")?[0].Attributes["src"].Value,
                Price = node.SelectNodes(".//*[@class='price_mr']")[0].InnerText.Split(' ')[0].Replace('.', ',') + " BYN",
                Existance = node.SelectNodes(".//*[@class='item__avail_available']")?[0].InnerText ?? node.SelectNodes(".//*[@class='item__avail_order']")?[0].InnerText
            };
        }

        private HtmlNodeCollection GetNodeCollection(string request, int i)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(request + $"&page={i}");
            HtmlNodeCollection collection = htmlDoc.DocumentNode.SelectNodes("//*[@id='search_items']");
            return collection[0].SelectNodes("//*[@class='with-hover']");
        }

        private int? GetPageCount(string request)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(request);
            return htmlDoc.DocumentNode.SelectNodes("//*[@id='pager']/ul")?[0].SelectNodes("./li").Count ?? 1;
        }
    }
}