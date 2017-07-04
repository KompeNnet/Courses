using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace _3rd_searching.Models
{
    public class Item
    {
        [Key]
        public string Name { get; set; }
        public string ItemLink { get; set; }
        public string Price { get; set; }
        public string ImageLink { get; set; }
        public string Existance { get; set; }
    }
}