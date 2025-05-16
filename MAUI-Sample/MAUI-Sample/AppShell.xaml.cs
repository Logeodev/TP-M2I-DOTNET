namespace MAUI_Sample
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.TaskDetailPage), typeof(Views.TaskDetailPage));
        }
    }
}