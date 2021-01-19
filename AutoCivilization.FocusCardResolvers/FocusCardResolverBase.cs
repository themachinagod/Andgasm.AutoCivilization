using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public abstract class FocusCardMoveResolverBase : IFocusCardMoveResolver
    {
        internal readonly IBotMoveStateCache _botMoveStateService;

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

        public FocusCardMoveResolverBase(IBotMoveStateCache botMoveStateService)
        {
            _botMoveStateService = botMoveStateService;
            _currentStep = -1;
            _actionSteps = new Dictionary<int, IStepAction>();
        }

        public virtual void PrimeMoveState(BotGameStateCache botGameStateService)
        {
        }

        public MoveStepActionData ProcessMoveStepRequest()
        {
            var stepAction = GetNextStep();
            if (stepAction.ShouldExecuteAction())
            {
                return stepAction.ExecuteAction();
            }
            return null;
        }

        public void ProcessMoveStepResponse(string response)
        {
            var stepAction = _actionSteps[_currentStep];
            stepAction.ProcessActionResponse(response);
        }

        public virtual string UpdateGameStateForMove(BotGameStateCache botGameStateService)
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
