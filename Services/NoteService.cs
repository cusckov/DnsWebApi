using DnsWebApi.Models;
using DnsWebApi.Services.DatabaseStrategy;
using DnsWebApi.Services.DatabaseStrategy.Interfaces;
using DnsWebApi.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Xml.Linq;

namespace DnsWebApi.Services
{
    public class NoteService : INoteService
    {
        IDatabaseStrategy databaseStrategy;

        public NoteService(DatabaseStrategyContext strategyContext)
        {
            databaseStrategy = strategyContext.GetStrategy();
        }

        public async Task<bool> Create(Note note)
        {
            if (note is null)
                throw new ArgumentNullException(nameof(note));

            var parameters = new Dictionary<string, object>();

            parameters.Add("@Text", note.Text);

            var result = await databaseStrategy.ExecuteProcedure("[dbo].[AddNote]", parameters);

            if(result.ErrorMessage != null)
                throw new Exception(result.ErrorMessage);

            return result.Result;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(int id, Note note)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
