using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.MiscResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoCivilization.TechnologyResolvers
{
    public class FocusBarResetResolver : IFocusBarResetResolver
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

        public FocusBarModel ResetFocusBarForSubMove(FocusBarModel activeFocusBar, FocusType focusType)
        {
            var slotIndexToReset = activeFocusBar.ActiveFocusSlots.First(x => x.Value.Type == focusType).Key;
            if (slotIndexToReset == 0) return activeFocusBar;

            var newFocusBar = new Dictionary<int, FocusCardModel>();
            if (slotIndexToReset == 1)
            {
                var tmpSlot2 = activeFocusBar.FocusSlot1;
                newFocusBar.Add(0, activeFocusBar.ActiveFocusSlots[slotIndexToReset]);
                newFocusBar.Add(1, tmpSlot2);
                newFocusBar.Add(2, activeFocusBar.ActiveFocusSlots[2]);
                newFocusBar.Add(3, activeFocusBar.ActiveFocusSlots[3]);
                newFocusBar.Add(4, activeFocusBar.ActiveFocusSlots[4]);
            }
            else
            {
                var tmpSlot2 = activeFocusBar.FocusSlot1;
                newFocusBar.Add(0, activeFocusBar.ActiveFocusSlots[slotIndexToReset]);

                var slotPointer = 0;
                for (int i = slotIndexToReset - 1; i > 1; i--)
                {
                    newFocusBar.Add(i, activeFocusBar.ActiveFocusSlots[i - 1]);
                    slotPointer++;
                }
                newFocusBar.Add(1, tmpSlot2);

                for (int i = slotIndexToReset; i < 5; i++)
                {
                    newFocusBar.Add(i, activeFocusBar.ActiveFocusSlots[i]);
                    slotPointer++;
                }

                // TODO: we need to add the unaffected cards after slot index
            }
            return new FocusBarModel(new ReadOnlyDictionary<int, FocusCardModel>(newFocusBar));
        }
    }
}
