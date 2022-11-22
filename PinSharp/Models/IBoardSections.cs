using Newtonsoft.Json;
using PinSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinSharp.Models
{
    [JsonConverter(typeof(InterfaceConverter<BoardSections>))]
    public interface IBoardSections
    {
        List<IDetailedBoard> Items { get; set; }
        string Bookmark { get; set; }
    }
}
