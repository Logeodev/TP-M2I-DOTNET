using System.Net;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using MAUI_Sample.Mock;
using IResponseProvider = WireMock.ResponseProviders.IResponseProvider;
using TaskStatus = MAUI_Sample.Models.TaskStatus;
using TaskInput = MAUI_Sample.Models.TaskInput;
using WireMock;

namespace MAUI_Sample.Services
{
    public static class ApiMockSetup
    {
        public static WireMockServer ConfigureApiMock()
        {
            var mockServer = WireMockServer.Start();

            // GET all tasks
            mockServer
                .Given(Request.Create().WithPath("/api/tasks").UsingGet())
                .ThenRespondWith((req) => req
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson((req) => (TaskApiMockResponses.GetSampleTasks())));

            // GET task by ID
            mockServer
                .Given(Request.Create().WithPath("/api/tasks/*").UsingGet())
                .ThenRespondWith((builder) => builder
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(req => (TaskApiMockResponses.GetTaskById(int.Parse(req.PathSegments[2])))));

            // POST create task
            mockServer
                .Given(Request.Create().WithPath("/api/tasks").UsingPost())
                .ThenRespondWith((builder) => builder
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(req => (CreateTaskResponse(req))));

            // PUT update task
            mockServer
                .Given(Request.Create().WithPath("/api/tasks/*").UsingPut())
                .ThenRespondWith((builder) => builder
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(req => (UpdateTaskResponse(req)))
                );

            // PUT update task status
            mockServer
                .Given(Request.Create().WithPath("/api/tasks/*/status").UsingPut())
                .ThenRespondWith((builder) => builder
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(req => (UpdateTaskStatusResponse(req)))
                );

            // DELETE task
            mockServer
                .Given(Request.Create().WithPath("/api/tasks/*").UsingDelete())
                .ThenRespondWith((builder) => builder
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(req => (DeleteTaskResponse(req)))
                );

            return mockServer;
        }

        private static object CreateTaskResponse(IRequestMessage req)
        {
            var body = req.Body;
            var taskInput = JsonSerializer.Deserialize<TaskInput>(body);
            return TaskApiMockResponses.CreateTask(taskInput);
        }

        private static IResponseProvider UpdateTaskResponse(IRequestMessage req)
        {
            var id = int.Parse(req.PathSegments[2]);
            var taskInput = JsonSerializer.Deserialize<TaskInput>(req.Body);
            var task = TaskApiMockResponses.UpdateTask(id, taskInput);

            if (task != null)
            {
                return Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(task);
            }
            else
            {
                return Response.Create()
                    .WithStatusCode(HttpStatusCode.NotFound);
            }
        }

        private static IResponseProvider UpdateTaskStatusResponse(IRequestMessage req)
        {
            var id = int.Parse(req.PathSegments[2]);
            var statusObj = JsonSerializer.Deserialize<JsonElement>(req.Body);
            var status = Enum.Parse<TaskStatus>(statusObj.GetProperty("status").GetString());
            var task = TaskApiMockResponses.UpdateTaskStatus(id, status);

            if (task != null)
            {
                return Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(task);
            }
            else
            {
                return Response.Create()
                    .WithStatusCode(HttpStatusCode.NotFound);
            }
        }

        private static IResponseProvider DeleteTaskResponse(IRequestMessage req)
        {
            var id = int.Parse(req.PathSegments[2]);
            var success = TaskApiMockResponses.DeleteTask(id);

            if (success)
            {
                return Response.Create()
                    .WithStatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Response.Create()
                    .WithStatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
