using System;

namespace MAUI_Sample.Models
{
    public class TaskInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
