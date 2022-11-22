using System.Collections.Generic;

namespace PinSharp.Api.Responses
{
    internal class PagedApiResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public string Bookmark { get; set; }
    }
}
