using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised once and should NEVER change its internal state

    public class GlobalGameCache : IGlobalGameCache
    {
        public IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
    }
}
