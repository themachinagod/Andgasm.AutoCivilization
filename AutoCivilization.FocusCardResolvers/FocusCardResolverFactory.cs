using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Console
{
    public class FocusCardResolverFactory : IFocusCardResolverFactory
    {
        private readonly List<IFocusCardResolver> _resolvers;

        public FocusCardResolverFactory(ICultureLevel1FocusCardResolver cultureLevel1FocusCardResolver,
                                        ICultureLevel2FocusCardResolver cultureLevel2FocusCardResolver,
                                        ICultureLevel3FocusCardResolver cultureLevel3FocusCardResolver,
                                        ICultureLevel4FocusCardResolver cultureLevel4FocusCardResolver)
        {
            _resolvers = new List<IFocusCardResolver>();
            _resolvers.Add(cultureLevel1FocusCardResolver);
            _resolvers.Add(cultureLevel2FocusCardResolver);
            _resolvers.Add(cultureLevel3FocusCardResolver);
            _resolvers.Add(cultureLevel4FocusCardResolver);
        }

        public IFocusCardResolver GetFocusCardResolverForFocusCard(FocusCardModel activeFocusCard)
        {
            var applicableTypeResolvers = ResolveForFocusType(activeFocusCard.Type);
            var applicableResolver = ResolveForFocusLevel(applicableTypeResolvers, activeFocusCard.Level);
            return applicableResolver;
        }

        private IReadOnlyCollection<IFocusCardResolver> ResolveForFocusType(FocusType focusType)
        {
            return _resolvers.Where(x => x.FocusType == focusType).ToList();
        }

        private IFocusCardResolver ResolveForFocusLevel(IReadOnlyCollection<IFocusCardResolver> applicableTypeResolvers, FocusLevel focusLevel)
        {
            return applicableTypeResolvers.Single(x => x.FocusLevel == focusLevel);
        }
    }
}
