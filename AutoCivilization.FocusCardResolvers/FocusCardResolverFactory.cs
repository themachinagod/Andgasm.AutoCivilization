using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Console
{
    public class FocusCardResolverFactory : IFocusCardResolverFactory
    {
        private readonly List<IFocusCardMoveResolver> _resolvers;

        public FocusCardResolverFactory(ICultureLevel1FocusCardMoveResolver cultureLevel1FocusCardResolver,
                                        ICultureLevel2FocusCardMoveResolver cultureLevel2FocusCardResolver,
                                        ICultureLevel3FocusCardMoveResolver cultureLevel3FocusCardResolver,
                                        ICultureLevel4FocusCardMoveResolver cultureLevel4FocusCardResolver,
                                        IScienceLevel1FocusCardMoveResolver scienceLevel1FocusCardResolver,
                                        IScienceLevel2FocusCardMoveResolver scienceLevel2FocusCardResolver,
                                        IScienceLevel3FocusCardMoveResolver scienceLevel3FocusCardResolver,
                                        IScienceLevel4FocusCardMoveResolver scienceLevel4FocusCardResolver,
                                        IEconomyLevel1FocusCardMoveResolver economyLevel1FocusCardResolver,
                                        IEconomyLevel2FocusCardMoveResolver economyLevel2FocusCardResolver,
                                        IEconomyLevel3FocusCardMoveResolver economyLevel3FocusCardResolver,
                                        IEconomyLevel4FocusCardMoveResolver economyLevel4FocusCardResolver)
        {
            _resolvers = new List<IFocusCardMoveResolver>();
            _resolvers.Add(cultureLevel1FocusCardResolver);
            _resolvers.Add(cultureLevel2FocusCardResolver);
            _resolvers.Add(cultureLevel3FocusCardResolver);
            _resolvers.Add(cultureLevel4FocusCardResolver);
            _resolvers.Add(scienceLevel1FocusCardResolver);
            _resolvers.Add(scienceLevel2FocusCardResolver);
            _resolvers.Add(scienceLevel3FocusCardResolver);
            _resolvers.Add(scienceLevel4FocusCardResolver);
            _resolvers.Add(economyLevel1FocusCardResolver);
            _resolvers.Add(economyLevel2FocusCardResolver);
            _resolvers.Add(economyLevel3FocusCardResolver);
            _resolvers.Add(economyLevel4FocusCardResolver);
        }

        public IFocusCardMoveResolver GetFocusCardMoveResolver(FocusCardModel activeFocusCard)
        {
            var applicableTypeResolvers = ResolveForFocusType(FocusType.Culture); // activeFocusCard.Type);
            var applicableResolver = ResolveForFocusLevel(applicableTypeResolvers, FocusLevel.Lvl3); // activeFocusCard.Level);
            return applicableResolver;
        }

        private IReadOnlyCollection<IFocusCardMoveResolver> ResolveForFocusType(FocusType focusType)
        {
            return _resolvers.Where(x => x.FocusType == focusType).ToList();
        }

        private IFocusCardMoveResolver ResolveForFocusLevel(IReadOnlyCollection<IFocusCardMoveResolver> applicableTypeResolvers, FocusLevel focusLevel)
        {
            var resolver = applicableTypeResolvers.FirstOrDefault(x => x.FocusLevel == focusLevel);
            if (resolver == null) return _resolvers.First(); // DBr: TESTING HACK: this is just to cover no impls for some cards - remove!
            return resolver;
        }
    }
}
