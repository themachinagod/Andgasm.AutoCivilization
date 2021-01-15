using AutoCivilization.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class LeaderCardInitialiser : ILeaderCardInitialiser
    {
        private readonly Random _randomService = new Random();

        private readonly IBotGameStateService _botGameStateService;

        private IReadOnlyCollection<LeaderCardModel> _leaderCardsDeck;

        public LeaderCardInitialiser(IBotGameStateService botGameStateService)
        {
            _botGameStateService = botGameStateService;
        }

        public async Task InitialiseRandomLeaderForBot()
        {
            var dataStream = ReadDataFromResource();
            _leaderCardsDeck = await InitialiseLeaderCardsDeck(dataStream);
            _botGameStateService.ChosenLeaderCard = ChooseRandomLeader();
        }

        private Stream ReadDataFromResource()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AutoCivilization.StateManagement.Data.LeaderCards.json";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private async Task<IReadOnlyCollection<LeaderCardModel>> InitialiseLeaderCardsDeck(Stream dataStream)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            return await JsonSerializer.DeserializeAsync<List<LeaderCardModel>>(dataStream, options);
        }

        private LeaderCardModel ChooseRandomLeader()
        {
            var randomOrdinal = _randomService.Next(_leaderCardsDeck.Count);
            return _leaderCardsDeck.ElementAt(randomOrdinal);
        }
    }
}
