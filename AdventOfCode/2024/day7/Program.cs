﻿using System.Numerics;
using System.Diagnostics;

class BridgeRepair
{
    private static BigInteger count = 0;

    public static void Main()
    {
        CallExpr();
    }

    public static void CallExpr()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        EvaluateExpression();
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        Console.WriteLine($"Execution Time: {ts.TotalMilliseconds / 1000}s");
    }

    private static void EvaluateExpression()
    {
        var input = File.ReadAllLines("numbers.txt");

        foreach (var line in input)
        {
            var arr = line.Split(':');

            var calibration = new
            {
                Result = BigInteger.Parse(arr[0].Trim()),
                Operands = arr[1].Trim().Split(' ')
                    .Select(s => BigInteger.Parse(s.Trim()))
                    .Reverse()
                    .ToList()
            };

            var possibleResults = Evaluate(calibration.Operands, []);

            if (possibleResults.Contains(calibration.Result))
                count += calibration.Result;
        }

        Console.WriteLine(count);
    }

    private static List<BigInteger> Evaluate(List<BigInteger> operands, Dictionary<string, List<BigInteger>> cache)
    {
        string key = string.Join(".", operands);

        if (cache.ContainsKey(key)) return cache[key];

        if (operands.Count < 2) throw new IndexOutOfRangeException("A minimum of 2 elements is expected.");

        List<BigInteger> result = [];

        if (operands.Count == 2)
        {
            result.Add(operands[0] + operands[1]);
            result.Add(operands[0] * operands[1]);
            result.Add(BigInteger.Parse(operands[1] + "" + operands[0])); // for part 1 remove this line
        }
        else
        {
            BigInteger left = operands[0];
            List<BigInteger> rights = Evaluate(operands[1..], cache);

            foreach (var right in rights)
            {
                foreach (var n in Evaluate([left, right], cache))
                {
                    result.Add(n);
                }
            }
        }

        cache.Add(key, result);

        return result;
    }
}