using SQLite;
using System.ComponentModel.DataAnnotations;

namespace StudentTermTracker.Models
{
    public class Course : INotifiable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public bool StartNotification { get; set; }
        public bool EndNotification { get; set; }
        public StatusType Status { get; set; }
        public String Notes { get; set; }

        // POLYMORPHISM Example 
        public enum StatusType
        {
            [Display(Name = "Not Selected")]
            None,
            [Display(Name = "In Progress")]
            InProgress,
            [Display(Name = "Completed")]
            Completed,
            [Display(Name = "Dropped")]
            Dropped,
            [Display(Name = "Planned")]
            Planned
        }
    }
}
