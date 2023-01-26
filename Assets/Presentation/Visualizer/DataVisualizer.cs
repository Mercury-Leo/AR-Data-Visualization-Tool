using Core.DataProcessor;
using Core.FileReader;
using Infrastructure.DataProcessor;
using Infrastructure.FileReader;
using Presentation.FileSelector.Scripts;
using UnityEngine;

namespace Presentation.Visualizer {
	public class DataVisualizer : MonoBehaviour {

		[SerializeField] FileSelectorHandler _setup;

		IFileReader<string[], string> _fileReader;
		IDataProcessor<string, string[,]> _dataProcessor;

		void Awake() {
			_fileReader = new CSVReader();
			_dataProcessor = new CSVDataProcessor(_fileReader);
		}

		void OnEnable() {
			_setup.OnFileSelected += FileSelected;
		}

		void OnDisable() {
			_setup.OnFileSelected -= FileSelected;
		}

		void FileSelected(string filePath) {
			var data = _dataProcessor.ProcessData(filePath);
		}
	}
}
