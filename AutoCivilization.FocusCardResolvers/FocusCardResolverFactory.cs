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

        public FocusCardResolverFactory(ICultureLevel1FocusCardResolver cultureLevel1FocusCardResolver,
                                        ICultureLevel2FocusCardResolver cultureLevel2FocusCardResolver,
                                        ICultureLevel3FocusCardMoveResolver cultureLevel3FocusCardResolver,
                                        ICultureLevel4FocusCardResolver cultureLevel4FocusCardResolver,
                                        IScienceLevel1FocusCardResolver scienceLevel1FocusCardResolver,
                                        IScienceLevel2FocusCardResolver scienceLevel2FocusCardResolver,
                                        IScienceLevel3FocusCardResolver scienceLevel3FocusCardResolver,
                                        IScienceLevel4FocusCardResolver scienceLevel4FocusCardResolver)
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
            return applicableTypeResolvers.Single(x => x.FocusLevel == focusLevel);
        }
    }
}
