using System;

namespace ThompsonSamplingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Based on MSDN article: https://msdn.microsoft.com/en-us/magazine/mt829274.aspx
            Console.WriteLine("Begin Thompson sampling demo");

            int N = 3;
            double[] means = new double[] { 0.2, 0.25, 0.35 };
            double[] probs = new double[N];
            int[] S = new int[N];
            int[] F = new int[N];

            Random rnd = new Random(4);
            var rndInt = rnd.Next();

            BetaSampler bs = new BetaSampler(rndInt);

            for (int trial = 0; trial < 25000; ++trial)
            {
                Console.WriteLine("Trial " + trial);
                for (int i = 0; i < N; ++i)
                {
                    probs[i] = bs.Sample(S[i] + 1.0, F[i] + 1.0);
                }

                Console.Write("sampling probs = ");
                for (int i = 0; i < N; ++i)
                {
                    Console.Write(probs[i].ToString("F4") + " ");
                }

                Console.WriteLine(string.Empty);
                int machine = 0;
                double highProb = 0.0;
                for (int i = 0; i < N; ++i)
                {
                    if (probs[i] > highProb)
                    {
                        highProb = probs[i];
                        machine = i;
                    }
                }

                Console.Write("Playing machine " + machine);
                double p = rnd.NextDouble();

                if (p < means[machine])
                {
                    Console.WriteLine(" -- win");
                    ++S[machine];
                }
                else
                {
                    Console.WriteLine(" -- lose");
                    ++F[machine];
                }
            } // End of Trials Loop

            Console.WriteLine("Final estimates of means: ");
            for (int i = 0; i < N; ++i)
            {
                double u = (S[i] * 1.0) / (S[i] + F[i]);
                Console.WriteLine(u.ToString("F4") + "  ");
            }

            Console.WriteLine("Number times machine played:");
            for (int i = 0; i < N; ++i)
            {
                int ct = S[i] + F[i];
                Console.WriteLine(ct + "  ");
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("End of demo ");
            Console.ReadLine();
        }
    }
}
