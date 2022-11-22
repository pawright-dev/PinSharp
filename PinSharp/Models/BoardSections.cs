using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PinSharp.Models.Counts;
using PinSharp.Models.Images;

namespace PinSharp.Models
{
    public class BoardSections : IBoardSections
    {
        public List<IDetailedBoard> Items { get; set; }
        public string Bookmark { get; set; }
    }
}
