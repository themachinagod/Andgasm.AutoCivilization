using System.Collections.ObjectModel;

namespace AutoCivilization.Abstractions
{
    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised at construction and should NEVER change its internal state

    public class FocusBarModel
    {
        public ReadOnlyDictionary<int, FocusCardModel> ActiveFocusSlots { get; }
        public FocusCardModel ActiveFocusSlot { get { return ActiveFocusSlots[4]; } }
        public FocusCardModel FocusSlot1 { get { return ActiveFocusSlots[0]; } }
        public FocusCardModel FocusSlot2 { get { return ActiveFocusSlots[1]; } }
        public FocusCardModel FocusSlot3 { get { return ActiveFocusSlots[2]; } }
        public FocusCardModel FocusSlot4 { get { return ActiveFocusSlots[3]; } }
        public FocusCardModel FocusSlot5 { get { return ActiveFocusSlots[4]; } }

        public FocusBarModel(ReadOnlyDictionary<int, FocusCardModel> initialState)
        {
            ActiveFocusSlots = initialState;
        }
    }
}
