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

        public WonderCardDecksModel InitialiseWonderCardDecksForBot(int playerCount)
        {
            return InitialiseWonderCardDecks(playerCount);
        }

        private WonderCardDecksModel InitialiseWonderCardDecks(int playerCount)
        {
            var wonderDecksByType = _globalGameCache.WonderCardsDeck.GroupBy(x => x.Type);
            var wonderCardDecks = new Dictionary<FocusType, List<WonderCardModel>>();
            foreach(var wcg in wonderDecksByType)
            {
                var type = wcg.Key;
                var typeDeck = new List<WonderCardModel>();
                typeDeck.AddRange(wcg);
                RemoveWonderCardsForPlayerCount(typeDeck, playerCount);
                wonderCardDecks.Add(type, typeDeck);
            }
            return new WonderCardDecksModel(new ReadOnlyDictionary<FocusType, List<WonderCardModel>>(wonderCardDecks));
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
