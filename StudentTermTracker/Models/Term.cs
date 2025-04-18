using SQLite;

namespace StudentTermTracker.Models
{
    public class Term : INotifiable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public bool StartNotification { get; set; }
        public bool EndNotification { get; set; }
    }
}
