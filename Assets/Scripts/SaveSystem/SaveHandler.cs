using System;
using System.IO;
using UnityEngine;

namespace Save {
    public class SaveHandler {

        private string _dataDirPath = Application.persistentDataPath;                        //Getting the application base data path

        public void LoadData<T>(string _filePath, out T _gameData) where T : GameData {
            string _fullPath = Path.Combine(_dataDirPath, _filePath);                       //Building file path
            _gameData = null;
            if (File.Exists(_fullPath)) {                                                   //Checking if there's a file in the path
                try {
                    string _optionsToLoad = "";
                    using (FileStream _stream = new FileStream(_fullPath, FileMode.Open)) { //Opening file path
                        using (StreamReader _reader = new StreamReader(_stream)) {
                            _optionsToLoad = _reader.ReadToEnd();                           //Reading file
                        }
                    }
                
                    _gameData = JsonUtility.FromJson<T>(_optionsToLoad);                    //Putting file in class
                }
                catch (Exception _e) {
                    Debug.LogError(_e);
                }
            }
        }
        
        public void Save<T>(string _filePath, T _gameData) where T : GameData {
            string _fullPath = Path.Combine(_dataDirPath, _filePath);                       //Building the file path
            try {
                Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));
                string _dataToStore = JsonUtility.ToJson(_gameData, true);          //Converting data to Json
                using (FileStream _stream = new FileStream(_fullPath, FileMode.Create)) {   //Opening the file / creating one
                    using (StreamWriter _writer = new StreamWriter(_stream)) {
                        _writer.Write(_dataToStore);                                        //Writing in file
                    }
                }
            }
            catch (Exception _e) {
                Debug.LogError(_e);
            }
        }
    }
}