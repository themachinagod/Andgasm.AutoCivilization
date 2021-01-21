using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.StateInitialisers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class CityStatesInitialiser : ICityStatesInitialiser
    {
        public async Task<IReadOnlyCollection<CityStateModel>> InitialiseCityStates()
        {
            var dataStream = ReadDataFromResource();
            return await InitialiseCityStates(dataStream);
        }

        private Stream ReadDataFromResource()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AutoCivilization.StateManagement.Data.CityStates.json";
            return assembly.GetManifestResourceStream(resourceName);
        }

        private async Task<IReadOnlyCollection<CityStateModel>> InitialiseCityStates(Stream dataStream)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            return await JsonSerializer.DeserializeAsync<List<CityStateModel>>(dataStream, options);
        }
    }
}
