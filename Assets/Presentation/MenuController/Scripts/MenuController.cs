using Presentation.FileSelector.Scripts;
using Presentation.LayerOrganizer;
using Presentation.Visualizer;
using UnityEngine;

namespace Presentation.MenuController.Scripts {
    public class MenuController : MonoBehaviour {
        [Header("File selector")] 
        [SerializeField] FileSelectorHandler _fileSelector;
        [SerializeField] GameObject _files;

        [Header("Layer organizer")] 
        [SerializeField] LayerOrganizerManager _layerOrganizer;
        [SerializeField] GameObject _layers;

        [Header("Data Visualizer")] 
        [SerializeField] DataVisualizer _dataVisualizer;
        [SerializeField] GameObject _visualizer;

        void OnEnable() {
            _fileSelector.OnFileSelected += FileSelected;
            _layerOrganizer.OnOrderConfirmed += OnOrderConfirmed;
        }

        void OnDisable() {
            _fileSelector.OnFileSelected -= FileSelected;
            _layerOrganizer.OnOrderConfirmed -= OnOrderConfirmed;
        }

        void FileSelected(string obj) {
            _files.SetActive(false);
            _layers.SetActive(true);
        }

        void OnOrderConfirmed(int[] arg1, string[,] arg2) {
            _layers.SetActive(false);
            _visualizer.SetActive(true);
        }
    }
}