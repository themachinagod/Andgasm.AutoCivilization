using AutoCivilization.Abstractions;
using System;
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
        public async Task<IReadOnlyCollection<FocusCardModel>> InitialiseFocusCardsDeckForBot()
        {
            var dataStream = ReadDataFromResource();
            return await InitialiseFocusCardsDeck(dataStream);
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
