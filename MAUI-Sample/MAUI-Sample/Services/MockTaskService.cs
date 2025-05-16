using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using MAUI_Sample.Models;
using NJsonSchema.Extensions;
using TaskStatus = MAUI_Sample.Models.TaskStatus;

namespace MAUI_Sample.Services
{
    public class MockTaskService : ITaskService
    {
        private readonly HttpClient _httpClient;

        public MockTaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Models.Task>> GetTasksAsync(TaskStatus? status = null)
        {
            string url = "/api/tasks";
            if (status.HasValue)
            {
                url += $"?status={status.Value}";
            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<Models.Task>>(url);
        }

        public async Task<Models.Task> GetTaskByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Models.Task>($"/api/tasks/{id}");
        }

        public async Task<Models.Task> CreateTaskAsync(TaskInput taskInput)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/tasks", taskInput);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Models.Task>();
        }

        public async Task<Models.Task> UpdateTaskAsync(int id, TaskInput taskInput)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/tasks/{id}", taskInput);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Models.Task>();
        }

        public async Task<Models.Task> UpdateTaskStatusAsync(int id, TaskStatus status)
        {
            var statusUpdate = new { status = status.ToString() };
            var response = await _httpClient.PutAsJsonAsync($"/api/tasks/{id}/status", statusUpdate);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Models.Task>();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/tasks/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
