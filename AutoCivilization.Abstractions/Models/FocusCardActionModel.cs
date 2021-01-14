using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Abstractions
{
    public enum OperationType
    { 
        Instruction,
        InformationRequest
    }

    public class FocusCardActionModel
    {
        public OperationType OperationType { get; set; }
        public string Message { get; set; }
        public IReadOnlyCollection<string> ResponseOptions { get; set; }
    }
}
