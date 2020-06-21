using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.Extensions {
    public static class ListExtension {
        public static List<T> Randomize<T>(this List<T> list) {
            var result = new List<T>(list);
            var n = result.Count;
            
            while ( n > 1 ) {
                var rand = Random.Range(0, n--);
                var temp = result[n];
                result[n] = result[rand];
                result[rand] = temp;
            }

            return result;
        }

        public static T GetRandomElement<T>(this List<T> list) {
            var rand = Random.Range(0, list.Count);
            return list[rand];
        }

        public static void AddIfNotExists<T>(this List<T> list, T value) {
            if ( list.IndexOf(value) != -1 ) {
                return;
            }
            list.Add(value);
        }
    }
}