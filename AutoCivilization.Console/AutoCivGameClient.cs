using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.StateInitialisers;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{   
    public interface IAutoCivGameClient 
    {
        Task<BotGameStateCache> InitialiseNewGame();
    }

    public class AutoCivGameClient : IAutoCivGameClient
    {
        private readonly IFocusCardDeckInitialiser _focusCardDeckInitialiser;
        private readonly ILeaderCardInitialiser _leaderCardInitialiser;
        private readonly IFocusBarInitialiser _focusBarInitialiser;
        private readonly ICityStatesInitialiser _cityStateInitialiser;
        private readonly IGlobalGameCache _globalGameCache;

        public AutoCivGameClient(IGlobalGameCache globalGameCache,
                                 IFocusCardDeckInitialiser focusCardDeckInitialiser,
                                 ILeaderCardInitialiser leaderCardInitialiser,
                                 IFocusBarInitialiser focusBarInitialiser,
                                 ICityStatesInitialiser cityStateInitialiser)
        {
            _globalGameCache = globalGameCache;
            _cityStateInitialiser = cityStateInitialiser;
            _focusCardDeckInitialiser = focusCardDeckInitialiser;
            _leaderCardInitialiser = leaderCardInitialiser;
            _focusBarInitialiser = focusBarInitialiser;
        }

        public async Task<BotGameStateCache> InitialiseNewGame()
        {
            // TODO: wonder card initialisation for bot

            WriteConsoleHeader();

            var initialFocustCards = await _focusCardDeckInitialiser.InitialiseFocusCardsDeck();
            _globalGameCache.FocusCardsDeck = initialFocustCards;

            var initialCityStates = await _cityStateInitialiser.InitialiseCityStates();
            _globalGameCache.CityStates = initialCityStates;

            var focusBar = _focusBarInitialiser.InitialiseFocusBarForBot();
            var chosenLeader = await _leaderCardInitialiser.InitialiseRandomLeaderForBot();
            var gameState = new BotGameStateCache(focusBar, chosenLeader);

            WriteConsoleGameStart(gameState);
            return gameState;
        }

        private void WriteConsoleHeader()
        {
            System.Console.WriteLine("#############################");
            System.Console.WriteLine("#   AutoCivilization v1.0   #");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine();
        }


        private void WriteConsoleGameStart(BotGameStateCache gameState)
        {
            System.Console.WriteLine($"Leader Selected: {gameState.ChosenLeaderCard.Name} : {gameState.ChosenLeaderCard.Nation}");
            System.Console.WriteLine($"Focus Bar Slot 1: {gameState.ActiveFocusBar.FocusSlot1.Name}");
            System.Console.WriteLine($"Focus Bar Slot 2: {gameState.ActiveFocusBar.FocusSlot2.Name}");
            System.Console.WriteLine($"Focus Bar Slot 3: {gameState.ActiveFocusBar.FocusSlot3.Name}");
            System.Console.WriteLine($"Focus Bar Slot 4: {gameState.ActiveFocusBar.FocusSlot4.Name}");
            System.Console.WriteLine($"Focus Bar Slot 5: {gameState.ActiveFocusBar.FocusSlot5.Name}");
            System.Console.WriteLine();
            System.Console.WriteLine("Please go ahead and select leaders for all human players and setup the physical game board as normal.");
            System.Console.WriteLine("No need to deal me in, I will manage my own focus cards, focus bar, wonder decks, technology upgrades and trade tokens.");
            System.Console.WriteLine("If I need any physical interaction with the board, I will ask you to do this for me.");
            System.Console.WriteLine("If I need any information about moves that were made I will ask some simple questions.");
            System.Console.WriteLine("All you need to do just now is pick a color for me, place my captial city on the board and set aside my other peices.");
            System.Console.WriteLine("When everything is setup and you are happy to start the game, press any key and I will make the first move.");
            System.Console.ReadKey();
            System.Console.Clear();
        }
    }
}
