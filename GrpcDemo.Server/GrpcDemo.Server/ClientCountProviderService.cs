using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcClientCount;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace GrpcDemo.Server
{
    [Authorize]
    public class ClientCountProviderService : ClientCountProvider.ClientCountProviderBase
    {
        public override async Task GetClientCount(Empty request, IServerStreamWriter<ClientCount> responseStream, ServerCallContext context)
        {
            var count = 0;

            Console.WriteLine($"User name from JWT Token {context.GetHttpContext().User.Identity.Name}");

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new ClientCount { Count = count });
                count++;
            }
        }
    }
}
