using AutoCivilization.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoCivilization.Console
{
    public class WonderCardDecksInitialiser : IWonderCardDecksInitialiser
    {
        private readonly Random _randomService = new Random();

        private readonly IGlobalGameCache _globalGameCache;

        public WonderCardDecksInitialiser(IGlobalGameCache globalGameCache)
        {
            _globalGameCache = globalGameCache;
        }

        public WonderCardDecksModel InitialiseDecksForBot(int playerCount)
        {
            // TODO: we should be suffling the order before grouping

            var rawWonders = new List<WonderCardModel>( _globalGameCache.WonderCardsDeck);
            RemoveWonderCardsForPlayerCount(rawWonders, 2);

            var cultureWonder = GetActiveWonderForFocusType(rawWonders, FocusType.Culture);
            var economyWonder = GetActiveWonderForFocusType(rawWonders, FocusType.Economy);
            var scienceWonder = GetActiveWonderForFocusType(rawWonders, FocusType.Science);
            var militaryWonder = GetActiveWonderForFocusType(rawWonders, FocusType.Military);

            var activeWonders = new Dictionary<FocusType, WonderCardModel>();
            activeWonders.Add(FocusType.Culture, cultureWonder);
            activeWonders.Add(FocusType.Economy, economyWonder);
            activeWonders.Add(FocusType.Science, scienceWonder);
            activeWonders.Add(FocusType.Military, militaryWonder);

            var wonderCardDecks = RegenerateDecks(rawWonders);
            return new WonderCardDecksModel(new ReadOnlyDictionary<FocusType, ReadOnlyCollection<WonderCardModel>>(wonderCardDecks),
                                            new ReadOnlyDictionary<FocusType, WonderCardModel>(activeWonders));
        }

        public WonderCardDecksModel RegenerateDeckForPurchasedWonder(IList<WonderCardModel> availableWonders, 
                                                                     IDictionary<FocusType, WonderCardModel> unlockedWonders, 
                                                                     WonderCardModel purchasedWonder)
        {
            var newActiveUnlockedWonder = GetActiveWonderForFocusType(availableWonders, purchasedWonder.Type);
            var regeneratedUnlockedWonders = new Dictionary<FocusType, WonderCardModel>(unlockedWonders);
            regeneratedUnlockedWonders.Remove(purchasedWonder.Type);
            regeneratedUnlockedWonders.Add(purchasedWonder.Type, newActiveUnlockedWonder);

            var regeneratedWonderDecks = RegenerateDecks(availableWonders);
            return new WonderCardDecksModel(new ReadOnlyDictionary<FocusType, ReadOnlyCollection<WonderCardModel>>(regeneratedWonderDecks),
                                            new ReadOnlyDictionary<FocusType, WonderCardModel>(regeneratedUnlockedWonders));
        }

        private static Dictionary<FocusType, ReadOnlyCollection<WonderCardModel>> RegenerateDecks(IList<WonderCardModel> currentWonders)
        {
            var wonderDecksByType = currentWonders.GroupBy(x => x.Type);
            var wonderCardDecks = new Dictionary<FocusType, ReadOnlyCollection<WonderCardModel>>();
            foreach (var wcg in wonderDecksByType)
            {
                var type = wcg.Key;
                var typeDeck = new List<WonderCardModel>();
                typeDeck.AddRange(wcg);
                wonderCardDecks.Add(type, new ReadOnlyCollection<WonderCardModel>(typeDeck));
            }
            return wonderCardDecks;
        }

        private WonderCardModel GetActiveWonderForFocusType(IList<WonderCardModel> activeWonderCards, FocusType type)
        {
            var cardsForType = activeWonderCards.Where(x => x.Type == type).ToList();
            if (cardsForType.Count == 0) return null;

            var earliestEra = cardsForType.Min(x => x.Era);
            var cardsForTypeAndEra = cardsForType.Where(x => x.Era == earliestEra).ToList();

            var randomCardOrdinal = _randomService.Next(cardsForTypeAndEra.Count - 1);
            return cardsForTypeAndEra.ElementAt(randomCardOrdinal);
        }


        private void RemoveWonderCardsForPlayerCount(List<WonderCardModel> typeDeck, int playerCount)
        {
            // rules state two player game should;
            //       remove one random ancient era card
            //       remove one random medievil card

            if (playerCount < 4)
            {
                RemoveRandomCard(typeDeck, EraType.Ancient);
            }
            if (playerCount < 3)
            {
                RemoveRandomCard(typeDeck, EraType.Medievil);
            }
        }

        private void RemoveRandomCard(List<WonderCardModel> typeDeck, EraType era)
        {
            var deck = typeDeck.Where(x => x.Era == era).ToList();
            var removeIndex = _randomService.Next(deck.Count - 1);
            typeDeck.RemoveAt(removeIndex);
        }
    }
}
