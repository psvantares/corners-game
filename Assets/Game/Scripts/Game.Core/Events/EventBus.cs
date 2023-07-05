using System;
using UniRx;

namespace Game.Core
{
    public class EventBus
    {
        public static readonly ISubject<string> RemainingTime = new Subject<string>();
        public static readonly ISubject<string> GameStateEnding = new Subject<string>();

        public static IObservable<string> RemainingTimeEvent => RemainingTime;
        public static IObservable<string> GameStateEndingEvent => GameStateEnding;
    }
}