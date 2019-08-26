namespace NetR.Worker.Models
{
    public class ServiceConfig
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public string ResponsibleServer { get; set; }
        public string Status { get; set; }
        public bool Enabled { get; set; }
    }
}