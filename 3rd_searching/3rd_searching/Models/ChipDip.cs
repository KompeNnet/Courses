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
                List<string> links = GetLinksOnPage(request, i);
                foreach (string url in links)
                {
                    HtmlDocument htmlDoc = (new HtmlWeb()).Load(@"https://www.ru-chipdip.by" + url);
                    //TODO
                    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='item__content']");
                    foreach (HtmlNode node in nodes) result.Add(GetItem(node));
                }
            }
            catch { }
        }

        private List<string> GetLinksOnPage(string request, int i)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(request + $"&page={i}");
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='link group-header']");
            List<string> result = new List<string>();
            foreach (HtmlNode node in nodes)
            {
                if (node.Attributes["href"].Value.Contains("catalog-show"))
                    result.Add(node.Attributes["href"].Value);
                else
                {
                    string url = node.Attributes["href"].Value;
                    result.Add(url.Replace("catalog", "catalog-show"));
                }

            }
            return result;
        }

        private Item GetItem(HtmlNode node)
        {
            Item item = new Item();
            item.ItemLink = @"https://www.ru-chipdip.by" + node.SelectNodes(".//*[@class='link link_no-underline']")[0].Attributes["href"].Value;
            item.Name = node.SelectNodes(".//*[@class='link']")[0].InnerText;
            item.ImageLink = node.SelectNodes(".//img")?[0].Attributes["src"].Value;
            item.Price = node.SelectNodes(".//*[@class='price__value']")[0].InnerText.Split(' ')[0].Replace('.', ',') + " BYN";

            if (node.SelectNodes(".//*[@class='item__avail_available']") != null) item.Existance = node.SelectNodes(".//*[@class='item__avail_available']")[0].InnerText;
            else if (node.SelectNodes(".//*[@class='item__avail_order']") != null) item.Existance = node.SelectNodes(".//*[@class='item__avail_order']")[0].InnerText;
            else item.Existance = node.SelectNodes(".//*[@class='item__avail_awaiting']")[0].InnerText;

            return item;
        }

        private int? GetPageCount(string request)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(request);
            return htmlDoc.DocumentNode.SelectNodes("//*[@id='pager']/ul")?[0].SelectNodes("./li").Count ?? 1;
        }
    }
}