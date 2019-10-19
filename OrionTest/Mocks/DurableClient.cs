using DurableTask.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrionTest.Mocks
{
    class DurableClient : IDurableClient
    {
        #region "Methods used in mocks"
        public virtual Task<string> StartNewAsync(string functionName)
        {
            return Task.FromResult($"{functionName}");
        }

        public Task<string> StartNewAsync(string orchestratorFunctionName, string instanceId, object input)
        {
            return Task.FromResult($"{orchestratorFunctionName}");
        }
        #endregion

        #region Methods not used for tesitng"
        public string TaskHubName => throw new NotImplementedException();

        public HttpResponseMessage CreateCheckStatusResponse(HttpRequestMessage request, string instanceId)
        {
            throw new NotImplementedException();
        }

        public IActionResult CreateCheckStatusResponse(HttpRequest request, string instanceId)
        {
            throw new NotImplementedException();
        }

        public HttpManagementPayload CreateHttpManagementPayload(string instanceId)
        {
            throw new NotImplementedException();
        }

        public Task<DurableOrchestrationStatus> GetStatusAsync(string instanceId, bool showHistory, bool showHistoryOutput, bool showInput = true)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DurableOrchestrationStatus>> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DurableOrchestrationStatus>> GetStatusAsync(DateTime createdTimeFrom, DateTime? createdTimeTo, IEnumerable<OrchestrationRuntimeStatus> runtimeStatus, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<OrchestrationStatusQueryResult> GetStatusAsync(OrchestrationStatusQueryCondition condition, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PurgeHistoryResult> PurgeInstanceHistoryAsync(string instanceId)
        {
            throw new NotImplementedException();
        }

        public Task<PurgeHistoryResult> PurgeInstanceHistoryAsync(DateTime createdTimeFrom, DateTime? createdTimeTo, IEnumerable<OrchestrationStatus> runtimeStatus)
        {
            throw new NotImplementedException();
        }

        public Task RaiseEventAsync(string instanceId, string eventName, object eventData)
        {
            throw new NotImplementedException();
        }

        public Task RaiseEventAsync(string taskHubName, string instanceId, string eventName, object eventData, string connectionName = null)
        {
            throw new NotImplementedException();
        }

        public Task<EntityStateResponse<T>> ReadEntityStateAsync<T>(EntityId entityId, string taskHubName = null, string connectionName = null)
        {
            throw new NotImplementedException();
        }

        public Task RewindAsync(string instanceId, string reason)
        {
            throw new NotImplementedException();
        }

        public Task SignalEntityAsync(EntityId entityId, string operationName, object operationInput = null, string taskHubName = null, string connectionName = null)
        {
            throw new NotImplementedException();
        }

        public Task TerminateAsync(string instanceId, string reason)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> WaitForCompletionOrCreateCheckStatusResponseAsync(HttpRequestMessage request, string instanceId, TimeSpan timeout, TimeSpan retryInterval)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> WaitForCompletionOrCreateCheckStatusResponseAsync(HttpRequest request, string instanceId, TimeSpan timeout, TimeSpan retryInterval)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
