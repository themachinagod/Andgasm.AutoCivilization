using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public abstract class FocusCardMoveResolverBase : IFocusCardMoveResolver
    {
        #region Fields
        internal Dictionary<int, IStepAction> _actionSteps { get; set; }
        internal int _currentStep { get; set; }
        internal BotMoveStateCache _moveState { get; set; }

        #endregion

        #region Properties
        public FocusType FocusType { get; set; }
        public FocusLevel FocusLevel { get; set; }
        public bool HasMoreSteps
        {
            get
            {
                return _actionSteps.ContainsKey(_currentStep + 1);
            }
        }
        #endregion

        #region Ctor
        public FocusCardMoveResolverBase()
        {
            _currentStep = -1;
            _actionSteps = new Dictionary<int, IStepAction>();
        }
        #endregion

        public virtual void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _moveState = new BotMoveStateCache();
        }

        public MoveStepActionData ProcessMoveStepRequest()
        {
            var stepAction = GetNextStep();
            if (stepAction.ShouldExecuteAction(_moveState))
            {
                return stepAction.ExecuteAction(_moveState);
            }
            return null;
        }

        public void ProcessMoveStepResponse(string response)
        {
            var stepAction = _actionSteps[_currentStep];
            _moveState = stepAction.ProcessActionResponse(response, _moveState);
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
