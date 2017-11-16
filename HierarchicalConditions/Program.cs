///
/// 

namespace HierarchicalConditions
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            bool[] flags = new bool[] { true, false, true, true, false, false };
            ConditionalEvaluator ce = new ConditionalEvaluator
                (flags[0], GetFunc("0 true"), GetFunc("0 false"));
            ce
                .And(flags[1], GetFunc("0 Or 1 true"), GetFunc("0 Or 1 false"))
                    .Untrue(flags[2], GetFunc("True only on (0 Or 1) == false"));
            ce
                .Or(flags[3] || flags[4], GetFunc("0 AND 3 true"), GetFunc("0 AND 3 false"))
                    .Untrue(GetFunc("True only on (0 AND 3) == false"));

            // The above code replaces
            //if (flags[0])
            //{ Console.WriteLine("0 true"); }
            //else { Console.WriteLine("0 false"); }

            //if (flags[0] || flags [1])
            //{ Console.WriteLine("0 Or 1 true"); }
            //else { Console.WriteLine("0 Or 1 false"); }

            //if ((!(flags[0] || flags [1])) && (flags[2]))
            //{ Console.WriteLine("True only on (0 Or 1) == false"); }

            //if (flags[0] && (flags[3] || flags[4]))
            //{ Console.WriteLine("0 AND 3 true"); }
            //else { Console.WriteLine("0 AND 3 false"); }

            //if (!(flags[0] && (flags[3] || flags[4])))
            //{ Console.WriteLine("True only on(0 AND 3) == false"); }

            Console.WriteLine("Press ESC key to quit, any other key to see demo");
            ConditionalEvaluator ce2 = new ConditionalEvaluator(
                Console.ReadKey().Key != ConsoleKey.Escape,             // Condition
                () => { ce.Evaluate(); Console.ReadKey(); return 0; },  // If
                GetFunc("Non-Enter pressed"));                          // Else
            ce2.Evaluate();

            // The above block replaces
            if (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                ce.Evaluate();
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Non-Enter pressed");
            }
        }

        static Func<int> GetFunc(string msg, Func<int> postAction = null)
        {
            Func<int> FuncWithMessage = () =>
            {
                Console.WriteLine(msg);
                postAction?.Invoke();
                return 0;
            };
            return FuncWithMessage;
        }
    }
}
