namespace DnsWebApi.Services.DatabaseStrategy.Strategies
{
    public class SqlResult<T>
    {
        public T? Result { get; set; }
        public string? ErrorMessage { get; set; } = "";
    }
}