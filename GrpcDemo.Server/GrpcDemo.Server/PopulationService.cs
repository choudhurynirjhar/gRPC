using Grpc.Core;
using GrpcPopulation;
using System.Collections.Generic;
using System.Linq;
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

        public override async Task<PopulationResponse> GetPopulation(IAsyncStreamReader<PopulationRequest> requestStream, ServerCallContext context)
        {
            var statePopulations = new List<long>();
            while (await requestStream.MoveNext())
            {
                var populationRequest = requestStream.Current;
                statePopulations.Add(statePopulationProvider.Get(populationRequest.State));
            }

            return new PopulationResponse { Count = statePopulations.Sum() };
        }
    }
}
