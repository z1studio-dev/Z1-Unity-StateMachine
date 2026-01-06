

using System;

public class ConditionAlwaysFalse<Estate> : StateConditions<Estate> where Estate : Enum
{
    bool _isTrueOrFalse;
    public ConditionAlwaysFalse(Estate nextState) : base(nextState)
    {
        _isTrueOrFalse = false;
    }

    public override bool CheckCondition()
    {
        return _isTrueOrFalse;
    }
}

