using AutoCivilization.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class WonderCardsDeckInitialiser : IWonderCardsDeckInitialiser
    {
        public async Task<IReadOnlyCollection<WonderCardModel>> InitialiseWonderCardsDeck()
        {
            var dataStream = ReadDataFromResource();
            return await InitialiseWonderCardsDeck(dataStream);
        }

        private Stream ReadDataFromResource()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AutoCivilization.StateManagement.Data.WonderCards.json";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private async Task<IReadOnlyCollection<WonderCardModel>> InitialiseWonderCardsDeck(Stream dataStream)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            return await JsonSerializer.DeserializeAsync<List<WonderCardModel>>(dataStream, options);
        }
    }
}
