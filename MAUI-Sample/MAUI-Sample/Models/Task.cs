using System;

namespace MAUI_Sample.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public enum TaskStatus
    {
        todo,
        in_progress,
        done
    }

    public enum TaskPriority
    {
        low,
        medium,
        high
    }
}
