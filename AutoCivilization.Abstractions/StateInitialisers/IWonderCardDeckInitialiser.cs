using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IWonderCardsDeckInitialiser
    {
        Task<IReadOnlyCollection<WonderCardModel>> InitialiseWonderCardsDeck();
    }
}
