#if UNITY_EDITOR

using UnityEditor;

namespace Core.Extensions {
    public static class EditorExtensions {
        public const string ColumnOrderKey = "Order";

        const char Divider = ';';

        public static void SaveStringArray(string[] array, string key = ColumnOrderKey) {
            var combinedData = string.Join(Divider, array);
            EditorPrefs.SetString(key, combinedData);
        }

        public static string[] GetStringArray(string key) {
            if (!EditorPrefs.HasKey(key))
                return null;
            var orderString = EditorPrefs.GetString(key);
            return orderString.Split(Divider);
        }

        public static void DeleteKey(string key) {
            if (EditorPrefs.HasKey(key))
                EditorPrefs.DeleteKey(key);
        }
    }
}

#endif