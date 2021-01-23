using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.MiscResolvers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoCivilization.TechnologyResolvers
{
    public class FocusBarEndOfMoveResolver : IFocusBarEndOfRoundResolver
    {
        public FocusBarModel ResetFocusBarForNextMove(FocusBarModel activeFocusBar)
        {
            // shift all cards up by 1 with slot resolved this turn being reset to first slot

            var tmpSlot2 = activeFocusBar.FocusSlot1;
            var newFocusBar = new Dictionary<int, FocusCardModel>();
            newFocusBar.Add(0, activeFocusBar.FocusSlot5);
            newFocusBar.Add(4, activeFocusBar.FocusSlot4);
            newFocusBar.Add(3, activeFocusBar.FocusSlot3);
            newFocusBar.Add(2, activeFocusBar.FocusSlot2);
            newFocusBar.Add(1, tmpSlot2);
            return new FocusBarModel(new ReadOnlyDictionary<int, FocusCardModel>(newFocusBar));
        }
    }
}
