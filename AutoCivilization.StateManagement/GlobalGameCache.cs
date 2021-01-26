using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised once and should NEVER change its internal state
    //      due to this being an injected service - we cant init on constructor ala our immutable convention
    //      this needs to be manually primed for now (hence get/set)

    public class GlobalGameCache : IGlobalGameCache
    {
        public IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
        public IReadOnlyCollection<CityStateModel> CityStates { get; set; }
        public IReadOnlyCollection<WonderCardModel> WonderCardsDeck { get; set; }
    }
}
