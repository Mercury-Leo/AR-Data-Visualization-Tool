using TMPro;
using UnityEngine;

namespace Presentation.Components.Label.Scripts {
    public class LabelInitializer : MonoBehaviour {
        [SerializeField] TMP_Text _textComponent;

        public string Title {
            get => _textComponent.text;
            set => _textComponent.text = value;
        }
    }
}