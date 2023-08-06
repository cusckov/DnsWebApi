namespace DnsWebApi.Models
{
    public class Note
    {
        public string? Text { get; set; }
        public DateTime? CreationDateTime { get; set; } = DateTime.Now;
        public DateTime? ModifyDateTime { get; set; } = DateTime.Now;
        public bool? IsRead { get; set; } = false;
    }
}
