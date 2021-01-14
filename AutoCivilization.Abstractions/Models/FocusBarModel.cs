using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Abstractions
{
    public class FocusBarModel
    {
        Dictionary<int, FocusCardModel> _activeFocusBar { get; set; }
        public FocusCardModel FocusSlot1 { get { return _activeFocusBar[0]; } }
        public FocusCardModel FocusSlot2 { get { return _activeFocusBar[1]; } }
        public FocusCardModel FocusSlot3 { get { return _activeFocusBar[2]; } }
        public FocusCardModel FocusSlot4 { get { return _activeFocusBar[3]; } }
        public FocusCardModel FocusSlot5 { get { return _activeFocusBar[4]; } }

        public FocusBarModel(Dictionary<int, FocusCardModel> initialState)
        {
            _activeFocusBar = initialState;
        }
    }
}
