using UnityEngine;

namespace Core.DataBaseType {
    [CreateAssetMenu(fileName = "new DataBaseType", menuName = "Types/DataBase")]
    public class DataBaseTypeSO : ScriptableObject {
        [SerializeField] string _name;

        const string DefaultDatabase = "DataBase";

        void OnValidate() {
            if (string.IsNullOrWhiteSpace(_name))
                Debug.LogWarning("DataBase type isn't assigned.");
        }

        public virtual string GetName() {
            return string.IsNullOrWhiteSpace(_name) ? DefaultDatabase : _name;
        }
    }
}