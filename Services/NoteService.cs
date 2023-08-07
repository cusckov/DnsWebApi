using DnsWebApi.Models;
using DnsWebApi.Services.DatabaseStrategy;
using DnsWebApi.Services.DatabaseStrategy.Interfaces;
using DnsWebApi.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DnsWebApi.Services
{
    public class NoteService : INoteService
    {
        IDatabaseStrategy databaseStrategy;

        public NoteService(DatabaseStrategyContext strategyContext)
        {
            databaseStrategy = strategyContext.GetStrategy();
        }

        public async Task<Note> Create(Note note)
        {
            if (note is null)
                throw new ArgumentNullException(nameof(note));

            var parameters = new Dictionary<string, object>();

            parameters.Add("@Text", note.Text);
            parameters.Add("@IsRead", note.IsRead);
            parameters.Add("@CreationDateTime", note.CreationDateTime);
            parameters.Add("@ModifyDateTime", note.ModifyDateTime);

            var result = await databaseStrategy.SelectDataFromProcedure("[dbo].[AddNote]", parameters);

            if (result.ErrorMessage != null || result.Result?.Count == 0)
                throw new Exception(result.ErrorMessage);

            note = new Note();

            var properties = typeof(Note).GetProperties();

            foreach (var property in properties)
            {
                property.SetValue(note, result.Result?[0][property.Name], null);
            }

            return note;
        }

        public async Task<bool> Delete(int id)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("@Id", id);

            var result = await databaseStrategy.ExecuteProcedureWithReturnParameter("[dbo].[DeleteNote]", parameters);

            if (result.ErrorMessage != null)
                throw new Exception(result.ErrorMessage);

            return Convert.ToBoolean(result.Result);
        }

        public async Task<Note> Update(int id, string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            var parameters = new Dictionary<string, object>();

            parameters.Add("@Id", id);
            parameters.Add("@Text", text);

            var result = await databaseStrategy.SelectDataFromProcedure("[dbo].[UpdateNote]", parameters);

            if (result.ErrorMessage != null || result.Result?.Count == 0)
                throw new Exception(result.ErrorMessage);

            var returnNote = new Note();

            var properties = typeof(Note).GetProperties();

            foreach (var property in properties)
            {
                property.SetValue(returnNote, result.Result?[0][property.Name], null);
            }

            return returnNote;
        }

        public async IAsyncEnumerable<Note> GetAll()
        {
            var result = await databaseStrategy.SelectDataFromProcedure("[dbo].[GetAllNotes]", null);

            if (result.ErrorMessage != null)
                throw new Exception(result.ErrorMessage);

            foreach (var items in result.Result)
            {
                var note = new Note();

                var properties = typeof(Note).GetProperties();

                foreach (var property in properties)
                {
                    property.SetValue(note, items.Value[property.Name], null);
                }

                yield return note;
            }
        }

        public async IAsyncEnumerable<Note> GetUnread()
        {
            var result = await databaseStrategy.SelectDataFromProcedure("[dbo].[GetUnreadNotes]", null);

            if (result.ErrorMessage != null)
                throw new Exception(result.ErrorMessage);

            foreach (var items in result.Result)
            {
                var note = new Note();

                var properties = typeof(Note).GetProperties();

                foreach (var property in properties)
                {
                    property.SetValue(note, items.Value[property.Name], null);
                }

                yield return note;
            }
        }
    }
}
