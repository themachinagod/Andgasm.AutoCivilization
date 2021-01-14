using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.Console
{
    public class EarlyEmpireFocusCardResolver : ICultureLevel1FocusCardResolver
    {
        public FocusType FocusType { get; } = FocusType.Culture;
        public FocusLevel FocusLevel { get; } = FocusLevel.Lvl1;

        public void Resolve()
        {
        }
    }
}
