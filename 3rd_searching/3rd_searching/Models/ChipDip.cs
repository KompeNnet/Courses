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
                    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='item__content']");
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
                result.Add(node.Attributes["href"].Value);
            return result;
        }

        private Item GetItem(HtmlNode node)
        {
            return new Item()
            {
                // TODO: Set item values.
            };
        }

        private int? GetPageCount(string request)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(request);
            return htmlDoc.DocumentNode.SelectNodes("//*[@id='pager']/ul")?[0].SelectNodes("./li").Count ?? 1;
        }
    }
}