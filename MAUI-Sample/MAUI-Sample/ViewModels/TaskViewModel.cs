using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Sample.Models;
using MAUI_Sample.Services;
using TaskStatus = MAUI_Sample.Models.TaskStatus;
using Task = System.Threading.Tasks.Task;


namespace MAUI_Sample.ViewModels
{
    public partial class TasksViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        private ObservableCollection<Models.Task> _tasks;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
        private TaskInput _newTask;

        [ObservableProperty]
        private TaskStatus? _selectedStatusFilter;

        public TasksViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Title = "Tâches";
            Tasks = new ObservableCollection<Models.Task>();
            NewTask = new TaskInput
            {
                Title = "",
                Description = "",
                Priority = TaskPriority.medium
            };
        }

        [RelayCommand]
        public async Task LoadTasksAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var tasks = await _taskService.GetTasksAsync(SelectedStatusFilter);
                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanAddTask))]
        private async Task AddTaskAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(NewTask);
                Tasks.Add(createdTask);
                NewTask = new TaskInput
                {
                    Title = "",
                    Description = "",
                    Priority = TaskPriority.medium
                };
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanAddTask()
        {
            return !string.IsNullOrWhiteSpace(NewTask?.Title);
        }

        [RelayCommand]
        private async Task DeleteTaskAsync(int id)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                await _taskService.DeleteTaskAsync(id);
                var taskToRemove = Tasks.FirstOrDefault(t => t.Id == id);
                if (taskToRemove != null)
                {
                    Tasks.Remove(taskToRemove);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateStatusAsync(Models.Task task)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                // Cycle through statuses
                TaskStatus newStatus;
                switch (task.Status)
                {
                    case TaskStatus.todo:
                        newStatus = TaskStatus.in_progress;
                        break;
                    case TaskStatus.in_progress:
                        newStatus = TaskStatus.done;
                        break;
                    default:
                        newStatus = TaskStatus.todo;
                        break;
                }

                var updatedTask = await _taskService.UpdateTaskStatusAsync(task.Id, newStatus);

                // Update in collection
                var index = Tasks.IndexOf(task);
                if (index >= 0)
                {
                    Tasks[index] = updatedTask;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ApplyFilterAsync()
        {
            await LoadTasksAsync();
        }
    }
}
