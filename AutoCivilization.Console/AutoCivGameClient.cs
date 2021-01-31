using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.StateInitialisers;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{   
    public interface IAutoCivGameClient 
    {
        Task<BotGameState> InitialiseNewGame();
    }

    public class AutoCivGameClient : IAutoCivGameClient
    {
        private const int PlayerCount = 2;

        private readonly IFocusCardDeckInitialiser _focusCardDeckInitialiser;
        private readonly IWonderCardsDeckInitialiser _wonderCardDeckInitialiser;
        private readonly ILeaderCardInitialiser _leaderCardInitialiser;
        private readonly IFocusBarInitialiser _focusBarInitialiser;
        private readonly IWonderCardDecksInitialiser _wonderCardDecksInitialiser;
        private readonly ICityStatesInitialiser _cityStateInitialiser;
        private readonly IGlobalGameCache _globalGameCache;

        public AutoCivGameClient(IGlobalGameCache globalGameCache,
                                 IFocusCardDeckInitialiser focusCardDeckInitialiser,
                                 IWonderCardsDeckInitialiser wonderCardDeckInitialiser,
                                 ILeaderCardInitialiser leaderCardInitialiser,
                                 IFocusBarInitialiser focusBarInitialiser,
                                 IWonderCardDecksInitialiser wonderCardDecksInitialiser,
                                 ICityStatesInitialiser cityStateInitialiser)
        {
            _globalGameCache = globalGameCache;
            _cityStateInitialiser = cityStateInitialiser;
            _focusCardDeckInitialiser = focusCardDeckInitialiser;
            _wonderCardDeckInitialiser = wonderCardDeckInitialiser;
            _leaderCardInitialiser = leaderCardInitialiser;
            _focusBarInitialiser = focusBarInitialiser;
            _wonderCardDecksInitialiser = wonderCardDecksInitialiser;
        }

        public async Task<BotGameState> InitialiseNewGame()
        {
            // TODO: hardwired for two player game

            WriteConsoleHeader();

            var initialFocusCards = await _focusCardDeckInitialiser.InitialiseFocusCardsDeck();
            _globalGameCache.FocusCardsDeck = initialFocusCards;

            var initialWonderCards = await _wonderCardDeckInitialiser.InitialiseWonderCardsDeck();
            _globalGameCache.WonderCardsDeck = initialWonderCards;

            var initialCityStates = await _cityStateInitialiser.InitialiseCityStates();
            _globalGameCache.CityStates = initialCityStates;

            var focusBar = _focusBarInitialiser.InitialiseFocusBarForBot();
            var wonderCards = _wonderCardDecksInitialiser.InitialiseDecksForBot(PlayerCount);
            var chosenLeader = await _leaderCardInitialiser.InitialiseRandomLeaderForBot();
            var gameState = new BotGameState(focusBar, chosenLeader, wonderCards);

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


        private void WriteConsoleGameStart(BotGameState gameState)
        {
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Leader Selected: {gameState.ChosenLeaderCard.Name} : {gameState.ChosenLeaderCard.Nation}");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Focus Bar Slot 1: {gameState.ActiveFocusBar.FocusSlot1.Name}");
            System.Console.WriteLine($"Focus Bar Slot 2: {gameState.ActiveFocusBar.FocusSlot2.Name}");
            System.Console.WriteLine($"Focus Bar Slot 3: {gameState.ActiveFocusBar.FocusSlot3.Name}");
            System.Console.WriteLine($"Focus Bar Slot 4: {gameState.ActiveFocusBar.FocusSlot4.Name}");
            System.Console.WriteLine($"Focus Bar Slot 5: {gameState.ActiveFocusBar.FocusSlot5.Name}");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Unlocked Culture Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Culture]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Culture]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Culture]?.Cost}");
            System.Console.WriteLine($"Unlocked Economy Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Economy]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Economy]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Economy]?.Cost}");
            System.Console.WriteLine($"Unlocked Science Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Science]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Science]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Science]?.Cost}");
            System.Console.WriteLine($"Unlocked Military Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Military]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Military]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Military]?.Cost}");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine();
            System.Console.WriteLine("Please go ahead and select leaders for all human players (remove the leader I have chosen from the leader deck first) and setup the physical game board as normal.");
            System.Console.WriteLine("No need to deal me in, I will manage my own focus cards, focus bar, technology upgrades, trade tokens and resources.");
            System.Console.WriteLine("");
            System.Console.WriteLine("In order to keep our wonder card decks in sync, please allow me to manage the decks setup and ongoing evolution.");
            System.Console.WriteLine("I have already shuffled the decks for each focus type, removed cards as per the rules and turned over the unlocked wonders available for purchase which can be seen above. You can replicate this setup by the side of the board.");
            System.Console.WriteLine("Whenever I purchase an unlocked wonder during my turn, I will inform you as to how to update the unlocked wonder decks by the side of the board for easy reference.");
            System.Console.WriteLine("During each round, if a human player purchases a wonder they can inform me and I will update the unlocked wonder decks which you can replicate by the side of the board.");
            System.Console.WriteLine("");
            System.Console.WriteLine("For this initial version of the software, I am configured for a two player game using the prologue board setup.");
            System.Console.WriteLine("If I need any physical interaction with the board, I will ask you to do this for me.");
            System.Console.WriteLine("If I need any information about moves that were made I will ask some simple questions.");
            System.Console.WriteLine("All you need to do just now is pick a color for me, place my captial city on the board and set aside my other peices.");
            System.Console.WriteLine("When everything is setup and you are happy to start the game, press any key and I will make the first move.");
            System.Console.ReadKey();
            System.Console.Clear();
        }
    }
}
