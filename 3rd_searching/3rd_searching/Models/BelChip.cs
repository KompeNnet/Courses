using HtmlAgilityPack;
using System.Collections.Generic;

namespace _3rd_searching.Models
{
    public class BelChip
    {
        public List<Item> Parse(string query)
        {
            HtmlNodeCollection nodes = GetNodeCollection(query);
            List<Item> results = new List<Item>();
            if (nodes != null)
                foreach (HtmlNode node in nodes) results.Add(GetItem(node));
            return results;
        }

        private Item GetItem(HtmlNode node)
        {
            Item item = GetItemValues(node);
            if (item.Price == "цена по запросу") item.Existance = "По запросу";
            else { item.Existance = "Со склада"; item.Price += "BYN"; }
            return item;
        }

        private Item GetItemValues(HtmlNode node)
        {
            return new Item()
            {
                ItemLink = @"http://belchip.by/" + node.SelectNodes(".//h3/a")[0].Attributes["href"].Value,
                Name = node.SelectNodes(".//h3/a")[0].InnerText,
                ImageLink = @"http://belchip.by/" + node.SelectNodes(".//a/img")?[1].Attributes["src"].Value,
                Price = node.SelectNodes(".//*[@class='denoPrice']")?[0].InnerText.Split(' ')[0] ?? "цена по запросу"
            };
        }

        private HtmlNodeCollection GetNodeCollection(string query)
        {
            string request = @"http://belchip.by/search/?query=" + query;
            HtmlDocument html = (new HtmlWeb()).Load(request);
            return html.DocumentNode.SelectNodes("//*[@class='cat-item']");
        }
    }
}