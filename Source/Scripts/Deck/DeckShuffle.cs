using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DeckShuffle
{
    public static class ArrayUtils
    {
        private static Random rand = new Random(); // 전역적으로 하나의 Random 객체 유지
    
        public static void Shuffle<T>(T[] data)
        {
            for (int i = data.Length - 1; i > 0; i--) // Fisher-Yates 알고리즘 사용
            {
                int randomIndex = rand.Next(0, i + 1); // 0 ~ i 사이의 랜덤 인덱스 선택
                (data[i], data[randomIndex]) = (data[randomIndex], data[i]); // Swap 문법
            }
        }
    }
}
