using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ReactiveXConsole
{
    internal class Reactor
    {
        // Subjects are Both Observer and Observable
        // That is, they can generate both behavior and projections of behavior.
        private readonly ISubject<string> _distinctInput;
        private readonly ISubject<string> _oneInput;
        private readonly ISubject<string> _twoInput;
        private readonly ISubject<string> _debounceInput;

        private readonly IObservable<string> _distinctObservable;
        private readonly IObservable<string> _oneObservable;
        private readonly IObservable<string> _twoObservable;
        private readonly IObservable<string> _mergeObservable;
        private readonly IObservable<string> _debounceObservable;

#region what_about...
        // things to try:
        // 1. what about switching up numbers input (i.e., state as stream)
        // 2. what about merging?
#endregion

        public Reactor()
        {
            _distinctInput = new BehaviorSubject<string>("");
            _distinctObservable = _distinctInput
                .DistinctUntilChanged()
                .Select(s => $"got distinct {s}");
            _distinctObservable.Subscribe(s => Console.WriteLine(s));

            _oneInput = new BehaviorSubject<string>("");
            _oneObservable = _oneInput
                .Where(s => s.Equals("1", StringComparison.InvariantCultureIgnoreCase))
                .Select(s => $"got {s}");
            _oneObservable.Subscribe(s => Console.WriteLine(s));

            _twoInput = new BehaviorSubject<string>("");
            _twoObservable = _twoInput
                .Where(s => s.Equals("2", StringComparison.InvariantCultureIgnoreCase))
                .Select(s => $"got {s}");
            _twoObservable.Subscribe(s => Console.WriteLine(s));

            _mergeObservable = Observable.CombineLatest(_oneObservable, _twoObservable)
                .Select<IList<string>, string>(l => $"both {l[0]} and {l[1]}");
            _mergeObservable.Subscribe(s => Console.WriteLine(s));

            _debounceInput = new BehaviorSubject<string>("");
            _debounceObservable = _debounceInput
                .Where(s => !string.IsNullOrEmpty(s))
                .Throttle(TimeSpan.FromSeconds(3))
                .Select(s => $"bouncy bouncy {s}");
            _debounceObservable.Subscribe(s => Console.WriteLine(s));
        }

        public void HandleOne(string input)
        {
            _oneInput.OnNext(input);
        }

        public void HandleTwo(string input)
        {
            _twoInput.OnNext(input);
        }

        public void HandleDebounce(string input)
        {
            _debounceInput.OnNext(input);
        }

        public void HandleDistinctUntilChanged(string input)
        {
            _distinctInput.OnNext(input);
        }
    }
}