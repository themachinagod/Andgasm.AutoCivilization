using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardActionsResolver
    {
        IReadOnlyCollection<FocusCardActionModel> GetResolverForFocusCard(FocusCardModel focusCardModel);
    }
}
