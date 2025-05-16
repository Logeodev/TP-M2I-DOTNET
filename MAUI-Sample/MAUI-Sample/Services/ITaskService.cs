using System.Collections.Generic;
using System.Threading.Tasks;
using MAUI_Sample.Models;
using TaskStatus = MAUI_Sample.Models.TaskStatus;

namespace MAUI_Sample.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetTasksAsync(TaskStatus? status = null);
        Task<Models.Task> GetTaskByIdAsync(int id);
        Task<Models.Task> CreateTaskAsync(TaskInput taskInput);
        Task<Models.Task> UpdateTaskAsync(int id, TaskInput taskInput);
        Task<Models.Task> UpdateTaskStatusAsync(int id, TaskStatus status);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
    }
}
