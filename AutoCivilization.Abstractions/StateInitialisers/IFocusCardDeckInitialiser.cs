using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardDeckInitialiser
    {
        Task<IReadOnlyCollection<FocusCardModel>> InitialiseFocusCardsDeckForBot();
    }
}
