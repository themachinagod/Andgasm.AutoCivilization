using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotRoundStateCache : IBotRoundStateCache
    {
        public List<SubMoveConfiguration> SubMoveConfigurations { get; set; } = new List<SubMoveConfiguration>();
    }
}
