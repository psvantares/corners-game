using System;
using UniRx;

namespace Game.Core
{
    public class EventBus
    {
        public static readonly ISubject<float> RemainingTime = new Subject<float>();

        public static IObservable<float> RemainingTimeEvent => RemainingTime;
    }
}