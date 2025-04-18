using System;

namespace StudentTermTracker.Models
{
    public interface INotifiable
    {
        bool StartNotification { get; set; }
        bool EndNotification { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
} 