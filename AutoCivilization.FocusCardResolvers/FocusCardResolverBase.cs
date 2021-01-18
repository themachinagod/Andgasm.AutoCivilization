using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public abstract class FocusCardResolverBase : IFocusCardMoveResolver
    {
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

        public FocusCardResolverBase(IBotMoveStateService botMoveStateService)
        {
            _botMoveStateService = botMoveStateService;
            _currentStep = -1;
            _actionSteps = new Dictionary<int, IStepAction>();
        }

        public virtual void PrimeMoveState(IBotGameStateService botGameStateService)
        {
        }

        public (string Message, IReadOnlyCollection<string> ResponseOptions) ProcessMoveStepRequest()
        {
            var stepAction = GetNextStep();
            if (stepAction.ShouldExecuteAction())
            {
                return stepAction.ExecuteAction();
            }
            return (null, null);
        }

        public void ProcessMoveStepResponse(string response)
        {
            var stepAction = _actionSteps[_currentStep];
            stepAction.ProcessActionResponse(response);
        }

        public virtual string UpdateGameStateForMove(IBotGameStateService botGameStateService = null)
        {
            return string.Empty;
        }

        private IStepAction GetNextStep()
        {
            _currentStep++;
            return _actionSteps[_currentStep];
        }
    }
}
