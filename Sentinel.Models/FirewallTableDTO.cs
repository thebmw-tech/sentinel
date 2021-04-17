namespace Sentinel.Models
{
    public class FirewallTableDTO
    {
        public string DefaultAction { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultLog { get; set; }
    }
}