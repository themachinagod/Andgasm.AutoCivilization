using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Abstractions
{
    public enum OperationType
    { 
        ActionRequest,
        InformationRequest
    }

    public class FocusCardActionModel
    {
        public FocusType FocusType { get; set; }
        public FocusLevel FocusLevel { get; set; }
        public int StepIndex { get; set; }
        public OperationType OperationType { get; set; }
        public string Message { get; set; }
        public IReadOnlyCollection<string> ResponseOptions { get; set; }
        public string ResponseResult { get; set; }
    }
}
