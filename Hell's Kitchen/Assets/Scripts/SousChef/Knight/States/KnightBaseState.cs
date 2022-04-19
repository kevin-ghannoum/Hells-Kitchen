using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KnightBaseState
{
    public abstract void EnterState(KnightStateManager knight);
    public abstract void UpdateState(KnightStateManager knight);
}
