using AutoCivilization.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class FocusCardDeckInitialiser : IFocusCardDeckInitialiser
    {
        private readonly IBotGameStateService _botGameStateService;

        public FocusCardDeckInitialiser(IBotGameStateService botGameStateService)
        {
            _botGameStateService = botGameStateService;
        }

        public async Task InitialiseFocusCardsDeckForBot()
        {
            var dataStream = ReadDataFromResource();
            _botGameStateService.FocusCardsDeck = await InitialiseFocusCardsDeck(dataStream);
        }

        private Stream ReadDataFromResource()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AutoCivilization.StateManagement.Data.FocusCards.json";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private async Task<IReadOnlyCollection<FocusCardModel>> InitialiseFocusCardsDeck(Stream dataStream)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            return await JsonSerializer.DeserializeAsync<List<FocusCardModel>>(dataStream, options);
        }
    }
}
