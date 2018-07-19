using System.Collections.Generic;
using System.Security.Cryptography;
using System;

public static class Helpers
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        var provider = new RNGCryptoServiceProvider();
        var n = list.Count;
        while(n > 1)
        {
            #if UNITY_STANDALONE_WIN
            
            var box = new byte[1];

            do provider.GetBytes(box);
            while (!(box[0] < n * (byte.MaxValue / n)));

            var k = (box[0] % n);
            n--;

            #else

            n--;
            var k = rng.Next(n + 1);

            #endif

            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
