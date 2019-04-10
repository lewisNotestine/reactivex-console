using System;

namespace ReactiveXConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var reactor = new Reactor();
            var finished = false;

            Console.WriteLine(
                @"Press a key:
                1: send a 1
                2: send a 2
                b: send a b to be debounced
                q,w,e,r,t,y: send input to distinct-until-changed
                x: quit");

            do
            {
                string kinput = ((char)Console.Read()).ToString();
                switch (kinput)
                {
                    case "1":
                        reactor.HandleOne(kinput);
                        break;

                    case "2":
                        reactor.HandleTwo(kinput);
                        break;

                    case "b":
                        reactor.HandleDebounce(kinput);
                        break;

                    case "q":
                    case "w":
                    case "e":
                    case "r":
                    case "t":
                    case "y":
                        reactor.HandleDistinctUntilChanged(kinput);
                        break;

                    case "x":
                        finished = true;
                        break;

                    default:
                        break;

                }
            }
            while (!finished);
        }
    }
}
