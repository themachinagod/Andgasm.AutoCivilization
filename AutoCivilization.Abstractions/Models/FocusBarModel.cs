using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Abstractions
{
    public class FocusBarModel
    {
        public Dictionary<int, FocusCardModel> ActiveFocusSlots { get; set; }
        public FocusCardModel ActiveFocusSlot { get { return ActiveFocusSlots[4]; } }
        public FocusCardModel FocusSlot1 { get { return ActiveFocusSlots[0]; } }
        public FocusCardModel FocusSlot2 { get { return ActiveFocusSlots[1]; } }
        public FocusCardModel FocusSlot3 { get { return ActiveFocusSlots[2]; } }
        public FocusCardModel FocusSlot4 { get { return ActiveFocusSlots[3]; } }
        public FocusCardModel FocusSlot5 { get { return ActiveFocusSlots[4]; } }

        public FocusBarModel(Dictionary<int, FocusCardModel> initialState)
        {
            ActiveFocusSlots = initialState;
        }
    }
}
