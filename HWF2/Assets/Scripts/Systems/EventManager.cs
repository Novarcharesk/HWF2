using System;
using UnityEngine;

/// <summary>
/// The class that holds all static events/actions/funcs that may be invoked or broadcast throughout the game's runtime.
/// </summary>
public class EventManager
{
    public static Action OnScored;
    public static Action OnTimerEnd;
}
