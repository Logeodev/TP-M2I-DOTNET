using MAUI_Sample.Models;
using TaskStatus = MAUI_Sample.Models.TaskStatus;

namespace MAUI_Sample.Mock
{
    public static class TaskApiMockResponses
    {
        private static readonly List<Models.Task> _tasks = new List<Models.Task>
        {
            new Models.Task
            {
                Id = 1,
                Title = "Implémenter le module d'authentification",
                Description = "Créer les API endpoints pour l'authentification des utilisateurs avec JWT",
                Status = TaskStatus.in_progress,
                Priority = TaskPriority.high,
                CreatedAt = DateTime.Parse("2025-05-10T09:00:00Z"),
                UpdatedAt = DateTime.Parse("2025-05-12T14:30:00Z"),
                DueDate = DateTime.Parse("2025-06-15T14:00:00Z")
            },
            new Models.Task
            {
                Id = 2,
                Title = "Concevoir la base de données",
                Description = "Définir le schéma de la base de données pour le projet",
                Status = TaskStatus.done,
                Priority = TaskPriority.high,
                CreatedAt = DateTime.Parse("2025-05-05T10:00:00Z"),
                UpdatedAt = DateTime.Parse("2025-05-08T11:00:00Z"),
                DueDate = DateTime.Parse("2025-05-15T17:00:00Z")
            },
            new Models.Task
            {
                Id = 3,
                Title = "Ajouter des tests unitaires",
                Description = "Écrire des tests pour les fonctionnalités principales",
                Status = TaskStatus.todo,
                Priority = TaskPriority.medium,
                CreatedAt = DateTime.Parse("2025-05-14T13:00:00Z"),
                UpdatedAt = DateTime.Parse("2025-05-14T13:00:00Z"),
                DueDate = DateTime.Parse("2025-06-01T17:00:00Z")
            }
        };

        private static int _nextId = 4;

        public static IEnumerable<Models.Task> GetSampleTasks(TaskStatus? status = null)
        {
            if (status.HasValue)
            {
                return _tasks.Where(t => t.Status == status.Value).ToList();
            }
            return _tasks.ToList();
        }

        public static Models.Task GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public static Models.Task CreateTask(TaskInput taskInput)
        {
            var task = new Models.Task
            {
                Id = _nextId++,
                Title = taskInput.Title,
                Description = taskInput.Description,
                Status = TaskStatus.todo,
                Priority = taskInput.Priority,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DueDate = taskInput.DueDate
            };

            _tasks.Add(task);
            return task;
        }

        public static Models.Task UpdateTask(int id, TaskInput taskInput)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return null;

            task.Title = taskInput.Title;
            task.Description = taskInput.Description;
            task.Priority = taskInput.Priority;
            task.DueDate = taskInput.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            return task;
        }

        public static Models.Task UpdateTaskStatus(int id, TaskStatus status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return null;

            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;

            return task;
        }

        public static bool DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            _tasks.Remove(task);
            return true;
        }
    }
}
