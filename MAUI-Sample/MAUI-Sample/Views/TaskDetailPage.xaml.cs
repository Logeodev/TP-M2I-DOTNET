using MAUI_Sample.ViewModels;

namespace MAUI_Sample.Views
{
    public partial class TaskDetailPage : ContentPage
    {
        private TaskDetailViewModel _viewModel;

        public TaskDetailPage(TaskDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTaskAsync();
        }
    }
}
