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
        private readonly ISubject<string> _distinctInput;
        private readonly IObservable<string> _distinctObservable;

        private readonly ISubject<string> _oneInput;
        private readonly IObservable<string> _oneObservable;

        private readonly ISubject<string> _twoInput;
        private readonly IObservable<string> _twoObservable;

        private readonly IObservable<string> _mergeObservable;

        private readonly ISubject<string> _debounceInput;
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
                .DistinctUntilChanged();
            _distinctObservable.Subscribe(s => Console.WriteLine($"got distinct {s}"));

            _oneInput = new BehaviorSubject<string>("");
            _oneObservable = _oneInput
                .Where(s => s.Equals("1", StringComparison.InvariantCultureIgnoreCase));
            _oneObservable.Subscribe(s => Console.WriteLine($"got 1"));

            _twoInput = new BehaviorSubject<string>("");
            _twoObservable = _twoInput
                .Where(s => s.Equals("2", StringComparison.InvariantCultureIgnoreCase));
            _twoObservable.Subscribe(s => Console.WriteLine("got 2"));

            _mergeObservable = Observable.CombineLatest(_oneObservable, _twoObservable)
                .Select<IList<string>, string>(l => $"got both {l[0]} and {l[1]}");
            _mergeObservable.Subscribe(s => Console.WriteLine(s));

            _debounceInput = new BehaviorSubject<string>("");
            _debounceObservable = _debounceInput
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