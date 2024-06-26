﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinSharp.Api.Responses
{
    public class PagedResponse<T> : IReadOnlyList<T>
    {
        private IReadOnlyList<T> Items { get; }

        public PagedResponse(IEnumerable<T> pins) : this(pins, null)
        {
        }

        public PagedResponse(IEnumerable<T> pins, string cursor)
        {
            Items = new List<T>(pins ?? Enumerable.Empty<T>());
            NextPageCursor = cursor;
        }

        internal static async Task<PagedResponse<T>> FromTask(Task<PagedApiResponse<T>> task)
        {
            var response = await task.ConfigureAwait(false);
            if (response == null)
                return null;
            return new PagedResponse<T>(response.Items, response.Bookmark);
        }

        public string NextPageCursor { get; set; }

        public int? Ratelimit { get; set; }
        public int? RatelimitRemaining { get; set; }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => Items.Count;

        public T this[int index] => Items[index];
    }
}
