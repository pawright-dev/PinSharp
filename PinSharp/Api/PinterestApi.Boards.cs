using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PinSharp.Api.Responses;
using PinSharp.Models;

namespace PinSharp.Api
{
    internal partial class PinterestApi : IBoardsApi
    {
        public Task<IBoardSections> GetBoardSectionsAsync(string board)
        {
            return GetBoardSectionsAsync<IBoardSections>(board, Enumerable.Empty<string>());
        }

        public Task<T> GetBoardSectionsAsync<T>(string board, IEnumerable<string> fields)
        {
            return GetAsync<T>($"boards/{board}/sections", new RequestOptions(fields));
        }

        public Task<IDetailedBoard> GetBoardAsync(string board)
        {
            return GetBoardAsync<IDetailedBoard>(board, BoardFields);
        }

        public Task<T> GetBoardAsync<T>(string board, IEnumerable<string> fields)
        {
            return GetAsync<T>($"boards/{board}", new RequestOptions(fields));
        }

        public Task<PagedResponse<IPin>> GetPinsOnSectionAsync(string board, string section)
        {
            var responseTask = GetPagedAsync<IPin>($"boards/{board}/sections/{section}/pins");
            return PagedResponse<IPin>.FromTask(responseTask);
        }

        public Task<PagedResponse<IPin>> GetPinsAsync(string board)
        {
            return GetPinsAsync<IPin>(board, PinFields, null, 0);
        }

        public Task<PagedResponse<IPin>> GetPinsAsync(string board, int limit)
        {
            return GetPinsAsync<IPin>(board, PinFields, null, limit);
        }

        public Task<PagedResponse<IPin>> GetPinsAsync(string board, string cursor)
        {
            return GetPinsAsync<IPin>(board, PinFields, cursor, 0);
        }

        public Task<PagedResponse<IPin>> GetPinsAsync(string board, string cursor, int limit)
        {
            return GetPinsAsync<IPin>(board, PinFields, cursor, limit);
        }

        public Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields)
        {
            return GetPinsAsync<T>(board, fields, null, 0);
        }

        public Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields, int limit)
        {
            return GetPinsAsync<T>(board, fields, null, limit);
        }

        public Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields, string cursor)
        {
            return GetPinsAsync<T>(board, fields, cursor, 0);
        }

        public Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields, string cursor, int limit)
        {
            var responseTask = GetPagedAsync<T>($"boards/{board}/pins", new RequestOptions(fields, cursor, limit));
            return PagedResponse<T>.FromTask(responseTask);
        }

        public Task<IDetailedBoard> CreateBoardAsync(string name, string description = null)
        {
            return PostAsync<IDetailedBoard>("boards", new {name, description});
        }

        public Task<IDetailedBoard> CreateBoardSectionAsync(string parentId, string name, string description = null)
        {
            return PostAsync<IDetailedBoard>($"boards/{parentId}/sections", new { name, description });
        }

        public Task<IDetailedBoard> UpdateBoardAsync(string board, string name, string description = null)
        {
            return PatchAsync<IDetailedBoard>($"boards/{board}", new {board, name, description}, new RequestOptions(BoardFields));
        }

        public Task DeleteBoardAsync(string board)
        {
            return DeleteAsync($"boards/{board}");
        }
    }
}
