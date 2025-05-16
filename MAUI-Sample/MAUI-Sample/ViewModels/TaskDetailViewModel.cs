// ViewModels/TaskDetailViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Sample.Models;
using MAUI_Sample.Services;
using System.Threading.Tasks;
using TaskStatus = MAUI_Sample.Models.TaskStatus;
using Task = System.Threading.Tasks.Task;

namespace MAUI_Sample.ViewModels
{
    [QueryProperty(nameof(TaskId), "id")]
    public partial class TaskDetailViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        private int _taskId;

        [ObservableProperty]
        private Models.Task _task;

        [ObservableProperty]
        private TaskInput _taskEdit;

        public TaskDetailViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Title = "Détails de la tâche";
        }

        [RelayCommand]
        public async Task LoadTaskAsync()
        {
            if (TaskId <= 0) return;

            IsBusy = true;
            try
            {
                Task = await _taskService.GetTaskByIdAsync(TaskId);
                TaskEdit = new TaskInput
                {
                    Title = Task.Title,
                    Description = Task.Description,
                    Priority = Task.Priority,
                    DueDate = Task.DueDate
                };
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateTaskAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Task = await _taskService.UpdateTaskAsync(TaskId, TaskEdit);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateStatusAsync(TaskStatus newStatus)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Task = await _taskService.UpdateTaskStatusAsync(TaskId, newStatus);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
