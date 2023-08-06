using DnsWebApi.Models;
using DnsWebApi.Services.Interfaces;

namespace DnsWebApi.Services
{
    public class NoteService : INoteService
    {
        public NoteService()
        {
        }

        public Task<Note> Create(Note note)
        {
            throw new NotImplementedException();
        }

        public Task<Note> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Note> Edit(int id, Note note)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
