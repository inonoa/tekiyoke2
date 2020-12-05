
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomUtil
{
    public static IEnumerable<T> Generate<T>(Func<T> generator, int length)
    {
        return Enumerable.Repeat(0, length).Select(_ => generator.Invoke());
    }
}

public static class RandomPickExtensions
{
    public static T RandomPick<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
