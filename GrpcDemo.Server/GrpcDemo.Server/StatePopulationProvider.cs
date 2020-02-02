using System.Collections.Generic;

namespace GrpcDemo.Server
{
    public class StatePopulationProvider : IStatePopulationProvider
    {
        private readonly IDictionary<string, long> states = new Dictionary<string, long> {
            { "NJ", 10000 },
            { "NY", 20000 },
            { "MD", 30000 },
            { "KY", 40000 },
        };

        public long Get(string state)
        {
            if (states.ContainsKey(state))
            {
                return states[state];
            }
            return 0;
        }
    }
}
