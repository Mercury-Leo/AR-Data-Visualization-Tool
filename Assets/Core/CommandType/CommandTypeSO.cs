using UnityEngine;

namespace Core.CommandType {
    [CreateAssetMenu(fileName = "new Command Type", menuName = "Types/Command")]
    public class CommandTypeSO : ScriptableObject {
        [SerializeField] string _command;

        const string DefaultCommand = "Command";

        void OnValidate() {
            if (string.IsNullOrWhiteSpace(_command))
                Debug.LogWarning("Command type isn't assigned.");
        }

        public virtual string GetCommand() {
            return string.IsNullOrWhiteSpace(_command) ? DefaultCommand : _command;
        }
    }
}