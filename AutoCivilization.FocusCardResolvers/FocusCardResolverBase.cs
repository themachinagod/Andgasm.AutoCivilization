using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public abstract class FocusCardResolverBase : IFocusCardResolver
    {
        internal readonly IBotGameStateService _botGameStateService;
        internal readonly IBotMoveStateService _botMoveStateService;

        internal Dictionary<int, IStepAction> _actionSteps { get; set; }
        internal int _currentStep { get; set; }

        public FocusType FocusType { get; set; }
        public FocusLevel FocusLevel { get; set; }
        public bool HasMoreSteps
        {
            get
            {
                return _actionSteps.ContainsKey(_currentStep + 1);
            }
        }

        public FocusCardResolverBase(IBotGameStateService botGameStateService,
                                     IBotMoveStateService botMoveStateService)
        {
            _botGameStateService = botGameStateService;
            _botMoveStateService = botMoveStateService;
            _currentStep = -1;
            _actionSteps = new Dictionary<int, IStepAction>();
        }

        public virtual void InitialiseMoveState()
        {
        }

        public virtual IStepAction GetNextStep()
        {
            _currentStep++;
            return _actionSteps[_currentStep];
        }

        public virtual void Resolve()
        {
        }
    }
}
