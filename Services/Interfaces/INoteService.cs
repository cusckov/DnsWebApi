using DnsWebApi.Models;

namespace DnsWebApi.Services.Interfaces
{
    public interface INoteService
    {
        Task<Note> Create(Note note);
        Task<Note> Delete(int id);
        Task<Note> Edit(int id, Note note);
        IEnumerable<Note> GetAll();
    }
}