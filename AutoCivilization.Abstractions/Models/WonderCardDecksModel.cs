using System    ;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoCivilization.Abstractions
{
    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised at construction and should NEVER change its internal state

    // DBr: this represent the state of the user wonder card deck 
    //      insofar as we will regenerate this whenever a wonder is built so that the next wonder of that type is available
    //      the active wonders for types are the wonders that can be purchased
    //      when one is purchased we will recreate the wonder card decks model based on the new set

    public class WonderCardDecksModel
    {
        private readonly Random _randomService = new Random();

        public ReadOnlyDictionary<FocusType, ReadOnlyCollection<WonderCardModel>> AvailableWonderCardDecks { get; }

        public ReadOnlyDictionary<FocusType, WonderCardModel> UnlockedWonderCards { get; }

        public ReadOnlyCollection<WonderCardModel> CultureWonders { get { return AvailableWonderCardDecks[FocusType.Culture]; } }
        public ReadOnlyCollection<WonderCardModel> EconomyWonders { get { return AvailableWonderCardDecks[FocusType.Economy]; } }
        public ReadOnlyCollection<WonderCardModel> ScienceWonders { get { return AvailableWonderCardDecks[FocusType.Science]; } }
        public ReadOnlyCollection<WonderCardModel> MilitaryWonders { get { return AvailableWonderCardDecks[FocusType.Military]; } }

        public WonderCardDecksModel(ReadOnlyDictionary<FocusType, ReadOnlyCollection<WonderCardModel>> availableWonders,
                                    ReadOnlyDictionary<FocusType, WonderCardModel> activeWonders)
        {
            AvailableWonderCardDecks = availableWonders;

            var awc = new Dictionary<FocusType, WonderCardModel>();
            awc.Add(FocusType.Culture, activeWonders[FocusType.Culture]);
            awc.Add(FocusType.Economy, activeWonders[FocusType.Economy]);
            awc.Add(FocusType.Science, activeWonders[FocusType.Science]);
            awc.Add(FocusType.Military, activeWonders[FocusType.Military]);
            UnlockedWonderCards = new ReadOnlyDictionary<FocusType, WonderCardModel>(awc);
        }
    }
}
