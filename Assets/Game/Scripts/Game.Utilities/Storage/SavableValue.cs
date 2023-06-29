using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game.Utilities
{
    [Serializable]
    public sealed class SavableValue<T>
    {
        private readonly string playerPrefsPath;
        private T value;
        private T prevValue;
        public event Action OnChanged = () => { };

        public T Value
        {
            get => value;
            set
            {
                prevValue = this.value;
                this.value = value;
                SaveToPrefs();
                OnChanged.Invoke();
            }
        }

        public T PrevValue => prevValue;

        public SavableValue(string playerPrefsPath, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(playerPrefsPath))
            {
                throw new Exception("empty playerPrefsPath in savableValue");
            }

            this.playerPrefsPath = playerPrefsPath;

            value = defaultValue;
            prevValue = defaultValue;

            LoadFromPrefs();
        }

        private void LoadFromPrefs()
        {
            if (!PlayerPrefs.HasKey(playerPrefsPath))
            {
                SaveToPrefs();
                return;
            }

            var stringToDeserialize = PlayerPrefs.GetString(playerPrefsPath, "");

            var bytes = Convert.FromBase64String(stringToDeserialize);
            var memoryStream = new MemoryStream(bytes);
            var bf = new BinaryFormatter();

            value = (T)bf.Deserialize(memoryStream);
        }

        private void SaveToPrefs()
        {
            var memoryStream = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(memoryStream, value);
            var stringToSave = Convert.ToBase64String(memoryStream.ToArray());

            PlayerPrefs.SetString(playerPrefsPath, stringToSave);
        }
    }
}