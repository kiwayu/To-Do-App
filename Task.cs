using System;

namespace ToDoApp
{
    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public class TodoTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Category { get; set; } = "General";
        
        public bool IsOverdue => DueDate.HasValue && DueDate.Value.Date < DateTime.Today && !IsCompleted;
        
        public string StatusText => IsCompleted ? "Completed" : IsOverdue ? "Overdue" : "Pending";
        
        public string PriorityText => Priority.ToString();
        
        public string DueDateText => DueDate?.ToString("MMM dd, yyyy") ?? "No due date";
    }
}
