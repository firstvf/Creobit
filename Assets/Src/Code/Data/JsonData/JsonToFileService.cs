using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Src.Code.Data.JsonData
{
    public class JsonToFileService : IDataService
    {
        public bool CheckData(string key)
        {
            if (File.Exists(BuildPath(key)))
                return true;
            else return false;
        }

        public void Load<T>(string key, Action<T> callback)
        {
            string path = BuildPath(key);

            Debug.Log("Load: " + path);

            using (var fileStream = new StreamReader(path))
            {
                var json = fileStream.ReadToEnd();
                var data = JsonConvert.DeserializeObject<T>(json);
                callback.Invoke(data);
            }
        }

        public void Save(string key, object data, Action<bool> callback = null)
        {
            string path = BuildPath(key);
            string json = JsonConvert.SerializeObject(data);

            Debug.Log("Save: " + path);

            using (var fileStream = new StreamWriter(path))
            {
                fileStream.Write(json);
            }

            callback?.Invoke(true);
        }

        private string BuildPath(string key)
        => Application.streamingAssetsPath + "/" + key + ".json";
    }
}