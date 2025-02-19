using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DeckShuffle
{
    public class Array
    {
        static System.Random seed = new System.Random();

        public static void shuffle<T>(T[] data)
        {
            System.Random ran = new System.Random(seed.Next());

            for(int i = 0; i<data.Length; i++)
            {
                int randomValue = ran.Next(0, data.Length);
                T temp = data[i];
                data[i] = data[randomValue];
                data[randomValue] = temp;
            }
        }
    }
}
