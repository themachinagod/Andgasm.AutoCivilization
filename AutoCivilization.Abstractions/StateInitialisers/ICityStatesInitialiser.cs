using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions.StateInitialisers
{
    public interface ICityStatesInitialiser
    {
        Task<IReadOnlyCollection<CityStateModel>> InitialiseCityStates();
    }
}
