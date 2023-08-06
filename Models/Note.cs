namespace DnsWebApi.Models
{
    public class Note
    {
        public string? Text { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
