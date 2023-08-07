using DnsWebApi.Models;

namespace DnsWebApi.Services.Interfaces
{
    public interface INoteService
    {
        Task<Note> Create(Note note);
        Task<bool> Delete(int id);
        Task<Note> Update(int id, string text);
        IAsyncEnumerable<Note> GetUnread();
        IAsyncEnumerable<Note> GetAll();
    }
}