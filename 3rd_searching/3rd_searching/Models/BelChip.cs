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
            HtmlNodeCollection nodes = GetNodeCollection(query);
            List<Item> results = new List<Item>();
            if (nodes != null)
                foreach (HtmlNode node in nodes) results.Add(GetItem(node));
            return results;
        }

        private Item GetItem(HtmlNode node)
        {
            Item item = new Item()
            {
                ItemLink = @"http://belchip.by/" + node.SelectNodes(".//h3/a")[0].Attributes["href"].Value,
                Name = node.SelectNodes(".//h3/a")[0].InnerText,
                ImageLink = @"http://belchip.by/" + node.SelectNodes(".//a/img")?[1].Attributes["src"].Value,
                Price = node.SelectNodes(".//*[@class='denoPrice']")?[0].InnerText.Split(' ')[0] ?? "цена по запросу"
            };
            SetExistense(ref item);
            return item;
        }

        private void SetExistense(ref Item item)
        {
            if (item.Price == "цена по запросу") item.Existance = "по запросу";
            else { item.Existance = "на складе"; item.Price += "BYN"; }
        }

        private HtmlNodeCollection GetNodeCollection(string query)
        {
            string request = @"http://belchip.by/search/?query=" + query;
            HtmlDocument html = (new HtmlWeb()).Load(request);
            return html.DocumentNode.SelectNodes("//*[@class='cat-item']");
        }
    }
}