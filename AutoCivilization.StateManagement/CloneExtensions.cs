using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.StateManagement
{
    public static class CloneExtensions
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
