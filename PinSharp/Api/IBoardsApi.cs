using System.Collections.Generic;
using System.Threading.Tasks;
using PinSharp.Api.Responses;
using PinSharp.Models;

namespace PinSharp.Api
{
    public interface IBoardsApi
    {
        Task<IBoardSections> GetBoardSectionsAsync(string board);
        Task<T> GetBoardSectionsAsync<T>(string board, IEnumerable<string> fields);
        Task<IDetailedBoard> GetBoardAsync(string board);
        Task<T> GetBoardAsync<T>(string board, IEnumerable<string> fields);

        Task<PagedResponse<IPin>> GetPinsOnSectionAsync(string board, string section);
        Task<PagedResponse<IPin>> GetPinsAsync(string board);
        Task<PagedResponse<IPin>> GetPinsAsync(string board, int limit);
        Task<PagedResponse<IPin>> GetPinsAsync(string board, string cursor);
        Task<PagedResponse<IPin>> GetPinsAsync(string board, string cursor, int limit);

        Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields);
        Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields, int limit);
        Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields, string cursor);
        Task<PagedResponse<T>> GetPinsAsync<T>(string board, IEnumerable<string> fields, string cursor, int limit);

        Task<IDetailedBoard> CreateBoardAsync(string name, string description = null);
        Task<IDetailedBoard> CreateBoardSectionAsync(string parentId, string name, string description = null);
        Task<IDetailedBoard> UpdateBoardAsync(string board, string name, string description = null);
        Task DeleteBoardAsync(string board);
    }
}