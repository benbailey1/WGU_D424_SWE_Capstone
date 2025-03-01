using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTermTracker.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AssessmentType Type { get; set; }
        public bool StartNotification { get; set; }
        public bool EndNotification { get; set; }

        public enum AssessmentType
        {
            None = 0,
            Objective = 1,
            Performance = 2,
        }
    }
}
