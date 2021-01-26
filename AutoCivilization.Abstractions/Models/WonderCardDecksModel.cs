using System;
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

        public ReadOnlyDictionary<FocusType, List<WonderCardModel>> ActiveWonderCardDecks { get; }

        public List<WonderCardModel> CultureWonders { get { return ActiveWonderCardDecks[FocusType.Culture]; } }
        public List<WonderCardModel> EconomyWonders { get { return ActiveWonderCardDecks[FocusType.Economy]; } }
        public List<WonderCardModel> ScienceWonders { get { return ActiveWonderCardDecks[FocusType.Science]; } }
        public List<WonderCardModel> MilitaryWonders { get { return ActiveWonderCardDecks[FocusType.Military]; } }

        public List<WonderCardModel> ActiveWonders { get { return new List<WonderCardModel>() { ActiveCultureWonder, ActiveEconomyWonder, ActiveMilitaryWonder, ActiveScienceWonder }; } }
        public WonderCardModel ActiveCultureWonder { get { return GetActiveWonderForFocusType(FocusType.Culture); } }
        public WonderCardModel ActiveEconomyWonder { get { return GetActiveWonderForFocusType(FocusType.Economy); } }
        public WonderCardModel ActiveScienceWonder { get { return GetActiveWonderForFocusType(FocusType.Science); } }
        public WonderCardModel ActiveMilitaryWonder { get { return GetActiveWonderForFocusType(FocusType.Military); } }

        public WonderCardDecksModel(ReadOnlyDictionary<FocusType, List<WonderCardModel>> initialState)
        {
            ActiveWonderCardDecks = initialState;
        }

        private WonderCardModel GetActiveWonderForFocusType(FocusType type)
        {
            var cardsForType = ActiveWonderCardDecks[type];
            if (cardsForType.Count == 0) return null;

            var earliestEra = cardsForType.Min(x => x.Era);
            var cardsForTypeAndEra = cardsForType.Where(x => x.Era == earliestEra).ToList();

            var randomCardOrdinal = _randomService.Next(cardsForTypeAndEra.Count - 1);
            return cardsForTypeAndEra.ElementAt(randomCardOrdinal);
        }
    }
}
