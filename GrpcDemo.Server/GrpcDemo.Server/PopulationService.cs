using Grpc.Core;
using GrpcPopulation;
using System.Threading.Tasks;

namespace GrpcDemo.Server
{
    public class PopulationService : PopulationProvider.PopulationProviderBase
    {
        private readonly IStatePopulationProvider statePopulationProvider;

        public PopulationService(IStatePopulationProvider statePopulationProvider)
        {
            this.statePopulationProvider = statePopulationProvider;
        }

        public override Task<PopulationResponse> GetPopulation(
            PopulationRequest populationRequest, 
            ServerCallContext serverCallContext)
        {
            var count = statePopulationProvider.Get(populationRequest.State);
            return Task.FromResult(new PopulationResponse { Count = count.ToString() });
        }
    }
}
