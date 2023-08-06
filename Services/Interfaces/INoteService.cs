using DnsWebApi.Models;

namespace DnsWebApi.Services.Interfaces
{
    public interface INoteService
    {
        Task<bool> Create(Note note);
        Task<bool> Delete(int id);
        Task<bool> Edit(int id, Note note);
        IEnumerable<Note> GetAll();
    }
}