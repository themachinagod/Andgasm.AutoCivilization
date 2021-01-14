using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardResolver
    {
        FocusType FocusType { get; }
        FocusLevel FocusLevel { get; }

        void Resolve();
    }
}
