using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rd_searching.Models
{
    public class Item
    {
        public string Name { get; set; }
        public string ItemLink { get; set; }
        public string Price { get; set; }
        public string PictureLink { get; set; }
        public bool IsExists { get; set; }
    }
}