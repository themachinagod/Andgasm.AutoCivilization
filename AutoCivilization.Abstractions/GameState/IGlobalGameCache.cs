using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    // DBr: this should only be initialised once and should NEVER change its internal state
    //      see impl for comments around immutability

    public interface IGlobalGameCache
    {
        IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
        IReadOnlyCollection<CityStateModel> CityStates { get; set; }
    }
}
