using System;

namespace ThompsonSamplingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Based on MSDN article: https://msdn.microsoft.com/en-us/magazine/mt829274.aspx
            ConsoleWriteLineColor("############################", ConsoleColor.DarkBlue);
            ConsoleWriteLineColor("Begin Thompson Sampling Demo", ConsoleColor.DarkBlue);
            ConsoleWriteLineColor("############################", ConsoleColor.DarkBlue);

            // Define the number of of machines and their associated "hidden" success probabilities (payout)
            // The "hidden" probabilities are not known to us (in a real scenario) and we are trying to find the best one
            int N = 3;
            // probabilities
            double[] means = { 0.200, 0.250, 0.375 };
            double[] probs = new double[N];
            int[] successes = new int[N];
            int[] failures = new int[N];

            // Initialize the BetaSampler with a random number
            // Note: Use a hardcoded seed for reproducability
            Random rnd = new Random(DateTime.Now.Millisecond);
            var rndInt = rnd.Next();
            BetaSampler bs = new BetaSampler(rndInt);

            // Main Trials Loop
            for (int trial = 0; trial != 100; trial++)
            {
                ConsoleWriteLineColor("Trial " + (trial + 1), ConsoleColor.Magenta);
                // For each machine, sample new estimated probability from a beta distribution,
                // based on past successs/failures from trials.  First iteration starts at 0/0 for each machine
                for (int i = 0; i < N; ++i)
                {
                    // The probability results for each machine will be different each time the Sample method is run
                    // One specific "point" is selected from the entire Beta distribution
                    probs[i] = bs.Sample(successes[i] + 1.0, failures[i] + 1.0);
                }

                Console.Write("sampling probs = ");
                for (int i = 0; i < N; ++i)
                {
                    Console.Write(probs[i].ToString("F4") + " ");
                }

                Console.WriteLine(string.Empty);
                int machine = 0;
                double highProb = 0.0;

                // Select the machine to play with the highest probability from the Beta distribution (Beta Sampler)
                for (int i = 0; i < N; ++i)
                {
                    if (probs[i] > highProb)
                    {
                        highProb = probs[i];
                        machine = i;
                    }
                }

                Console.Write("Playing machine " + (machine + 1));
                double p = rnd.NextDouble();

                if (p < means[machine])
                {
                    ConsoleWriteLineColor(" -- win", ConsoleColor.Green);
                    ++successes[machine];
                }
                else
                {
                    ConsoleWriteLineColor(" -- lose", ConsoleColor.Red);
                    ++failures[machine];
                }
            } // End of Trials Loop

            Console.WriteLine("Final estimates of means: ");
            for (int i = 0; i < N; ++i)
            {
                double u = (successes[i] * 1.0) / (successes[i] + failures[i]);
                Console.WriteLine(u.ToString("F4") + "  ");
            }

            Console.WriteLine("Number times machine played:");
            for (int i = 0; i < N; ++i)
            {
                int ct = successes[i] + failures[i];
                Console.WriteLine(ct + "  ");
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("End of demo ");
            Console.ReadLine();
        }

        static void ConsoleWriteLineColor(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
