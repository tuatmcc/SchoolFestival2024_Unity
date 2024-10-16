using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RicoShot.Utils
{
    public static class JsonFileHandler
    {
        public static T LoadJson<T>(string path) where T : new()
        {
            if (!File.Exists(path))
            {  
                Debug.Log($"File not found at: {path}");
                var instance = new T();
                File.WriteAllText(path, JsonUtility.ToJson(instance));
                Debug.Log($"File created to: {path}");
                return instance;
            }
            var json = File.ReadAllText(path);
            var obj = JsonUtility.FromJson<T>(json);
            Debug.Log($"File loaded from: {path}");
            return obj;
        }

        public static void WriteJson<T>(string path, T data)
        {
            File.WriteAllText(path, JsonUtility.ToJson(data));
            Debug.Log($"File saved to: {path}");
        }
    }
}
