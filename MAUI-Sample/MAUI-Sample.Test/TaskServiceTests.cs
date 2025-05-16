using MAUI_Sample.Models;
using MAUI_Sample.Services;
using WireMock.Server;
using TaskStatus = MAUI_Sample.Models.TaskStatus;
using Task = System.Threading.Tasks.Task;

namespace MAUI_Sample.Tests.Services
{
    public class MockTaskServiceTests : IDisposable
    {
        private readonly WireMockServer _mockServer;
        private readonly HttpClient _httpClient;
        private readonly MockTaskService _taskService;

        public MockTaskServiceTests()
        {
            // Configurer le serveur mock
            _mockServer = ApiMockSetup.ConfigureApiMock();

            // Créer un HttpClient pointant vers le serveur mock
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_mockServer.Urls[0])
            };

            // Initialiser le service avec le client HTTP réel
            _taskService = new MockTaskService(_httpClient);
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsAllTasks_WhenNoStatusProvided()
        {
            // Act
            var result = await _taskService.GetTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsFilteredTasks_WhenStatusProvided()
        {
            // Arrange
            var status = TaskStatus.todo;

            // Act
            var result = await _taskService.GetTasksAsync(status);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, task => Assert.Equal(status, task.Status));
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenTaskExists()
        {
            // Arrange
            int taskId = 1; // ID qui existe dans les données de test

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
        }

        [Fact]
        public async Task CreateTaskAsync_ReturnsCreatedTask_WhenSuccessful()
        {
            // Arrange
            var taskInput = new TaskInput
            {
                Title = "Nouveau test",
                Description = "Description du test",
                Priority = TaskPriority.medium,
                DueDate = DateTime.Now.AddDays(7)
            };

            // Act
            var result = await _taskService.CreateTaskAsync(taskInput);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskInput.Title, result.Title);
            Assert.Equal(taskInput.Description, result.Description);
            Assert.Equal(taskInput.Priority, result.Priority);
            Assert.Equal(TaskStatus.todo, result.Status); // Les nouvelles tâches sont en statut 'todo'
        }

        [Fact]
        public async Task UpdateTaskAsync_ReturnsUpdatedTask_WhenSuccessful()
        {
            // Arrange
            int taskId = 1; // ID qui existe dans les données de test
            var taskInput = new TaskInput
            {
                Title = "Tâche mise à jour",
                Description = "Description mise à jour",
                Priority = TaskPriority.high,
                DueDate = DateTime.Now.AddDays(3)
            };

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, taskInput);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal(taskInput.Title, result.Title);
            Assert.Equal(taskInput.Description, result.Description);
            Assert.Equal(taskInput.Priority, result.Priority);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ReturnsUpdatedTask_WhenSuccessful()
        {
            // Arrange
            int taskId = 2; // ID qui existe dans les données de test
            var newStatus = TaskStatus.done;

            // Act
            var result = await _taskService.UpdateTaskStatusAsync(taskId, newStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal(newStatus, result.Status);
        }

        [Fact]
        public async Task DeleteTaskAsync_DoesNotThrow_WhenSuccessful()
        {
            // Arrange
            int taskId = 3; // ID qui existe dans les données de test

            // Act & Assert
            await _taskService.DeleteTaskAsync(taskId); // Si aucune exception n'est levée, le test réussit
        }

        [Fact]
        public async Task GetTaskByIdAsync_ThrowsException_WhenTaskNotFound()
        {
            // Arrange
            int nonExistentTaskId = 999;

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(() => _taskService.GetTaskByIdAsync(nonExistentTaskId));
        }

        public void Dispose()
        {
            // Nettoyage des ressources
            _httpClient.Dispose();
            _mockServer.Stop();
        }
    }
}
