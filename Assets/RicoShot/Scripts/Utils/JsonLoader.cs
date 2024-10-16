using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RicoShot.Utils
{
    public static class JsonLoader
    {
        public static T LoadJson<T>(string path) where T : new()
        {
            if (!File.Exists(path))
            {  
                Debug.LogError($"File not found: {path}");
                var instance = new T();
                File.WriteAllText(path, JsonUtility.ToJson(instance));
                Debug.Log($"File created: {path}");
                return instance;
            }
            var json = File.ReadAllText(path);
            var obj = JsonUtility.FromJson<T>(json);
            Debug.Log($"File loaded: {path}");
            return obj;
        }
    }
}
