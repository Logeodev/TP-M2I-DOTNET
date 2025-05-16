using MAUI_Sample.ViewModels;

namespace MAUI_Sample.Views
{
    public partial class TasksPage : ContentPage
    {
        private TasksViewModel _viewModel;

        public TasksPage(TasksViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTasksAsync();
        }
    }
}
