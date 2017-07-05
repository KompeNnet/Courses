using HtmlAgilityPack;
using System.Collections.Generic;

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
            for (int i = 1; i <= GetPageCount(request); i++)
                GetItemsFromSubLists(ref result, request, i);
            return result;
        }

        private void GetItemsFromSubLists(ref List<Item> result, string request, int i)
        {
            try
            {
                List<string> links = GetLinksOnPage(request, i);
                GetItemsFromEachSubList(ref result, links);
            }
            catch { }
        }

        private void GetItemsFromEachSubList(ref List<Item> result, List<string> links)
        {
            foreach (string url in links)
                for (int i = 1; i <= GetPageCount(url); i++) GetItemsFromSubListPages(ref result, url, i);
        }

        private void GetItemsFromSubListPages(ref List<Item> result, string url, int i)
        {
            HtmlNodeCollection nodes = GetNodeCollection(url + $"?page={i}", "//*[@class='item__content']");
            foreach (HtmlNode node in nodes) result.Add(GetItem(node));
        }

        private HtmlNodeCollection GetNodeCollection(string htmlDocLink, string selectParams)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(htmlDocLink);
            return htmlDoc.DocumentNode.SelectNodes(selectParams);
        }

        private List<string> GetLinksOnPage(string request, int i)
        {
            HtmlNodeCollection nodes = GetNodeCollection(request + $"&page={i}", "//*[@class='link group-header']");
            List<string> result = new List<string>();
            foreach (HtmlNode node in nodes) result.Add(@"https://www.ru-chipdip.by" + CheckUrl(node));
            return result;
        }

        private string CheckUrl(HtmlNode node)
        {
            if (node.Attributes["href"].Value.Contains("catalog-show"))
                return node.Attributes["href"].Value;
            string url = node.Attributes["href"].Value;
            return url.Replace("catalog", "catalog-show");
        }

        private Item GetItem(HtmlNode node)
        {
            Item item = new Item()
            {
                ItemLink = @"https://www.ru-chipdip.by" + node.SelectNodes(".//*[@class='link link_no-underline']")[0].Attributes["href"].Value,
                Name = node.SelectNodes(".//*[@class='link']")[0].InnerText,
                ImageLink = node.SelectNodes(".//img")?[0].Attributes["src"].Value,
                Price = node.SelectNodes(".//*[@class='price__value']")[0].InnerText.Split(' ')[0].Replace('.', ',') + " BYN",
                Existance = GetExistance(node)
            };
            return item;
        }

        private string GetExistance(HtmlNode node)
        {
            if (node.SelectNodes(".//*[@class='item__avail_available']") != null)
                return node.SelectNodes(".//*[@class='item__avail_available']")[0].InnerText;
            if (node.SelectNodes(".//*[@class='item__avail_order']") != null)
                return node.SelectNodes(".//*[@class='item__avail_order']")[0].InnerText;
            return node.SelectNodes(".//*[@class='item__avail_awaiting']")[0].InnerText;
        }

        private int? GetPageCount(string request)
        {
            HtmlDocument htmlDoc = (new HtmlWeb()).Load(request);
            return htmlDoc.DocumentNode.SelectNodes("//*[@id='pager']/ul")?[0].SelectNodes("./li").Count ?? 1;
        }
    }
}