using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions {
    public static class MatrixExtensions {
        public static HashSet<T>
            GroupDataByColumnKey<T>(T[,] data, T key, int[] order, int column, int startingRow = 1) {
            if (GuardMatrix(data))
                return null;

            if (key is null) {
                Debug.LogError("No key was provided.");
                return null;
            }

            var rowsLength = data.GetLength(0);
            var columnsLength = data.GetLength(1);

            if (order.Length != columnsLength) {
                Debug.LogError($"Order of columns doesn't match columns length: {order.Length} != {columnsLength}.");
                return null;
            }

            if (GuardIndex(column, columnsLength))
                return null;

            if (GuardIndex(startingRow, rowsLength))
                return null;

            var entries = new HashSet<T>();

            for (var i = startingRow; i < rowsLength; i++) {
                var group = data[i, order[column - 1]];
                if (!group?.Equals(key) ?? true)
                    continue;

                var entry = data[i, order[column]];
                if (entry is null)
                    return null;

                entries.Add(entry);
            }

            return entries;
        }

        public static HashSet<T> GetColumnUniqueEntries<T>(T[,] data, int column, int startingRow = 1) {
            if (GuardMatrix(data))
                return null;

            if (GuardIndex(column, data.GetLength(1)))
                return null;

            if (GuardIndex(startingRow, data.GetLength(0)))
                return null;

            var uniqueEntries = new HashSet<T>();

            for (var row = startingRow; row < data.GetLength(0); row++)
                uniqueEntries.Add(data[row, column]);

            return uniqueEntries;
        }

        static bool GuardMatrix<T>(T[,] data) {
            if (data is not null) return false;

            Debug.LogError("Data is null.");
            return true;
        }

        static bool GuardIndex(int index, int top, int bottom = 0) {
            if (index >= bottom && index < top) return false;

            Debug.LogError($"Selected index: {index} is out of bounds.");
            return true;
        }
    }
}