using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardResolverFactory
    {
        IFocusCardResolver GetFocusCardResolverForFocusCard(FocusCardModel activeFocusCard);
    }
}
