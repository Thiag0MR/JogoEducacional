using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SerializableList<T>
{
    public List<T> list;

    public SerializableList() { }
    public SerializableList(List<T> list)
    {
        this.list = list;
    }
}

public class JsonFileManager 
{
    public static void SaveListToJson<T>(List<T> list, string path)
    {
        var serializableList = new SerializableList<T>(list);
        string content = JsonUtility.ToJson(serializableList);
        FileManager.WriteTextToFile(content, path);
    }

    public async static Task<List<T>> ReadListFromJson<T>(string path)
    {
        string content = await FileManager.ReadTextFromFile(path);
        List<T> list = null;
        if (content != null)
        {
            SerializableList<T> serializableList = new SerializableList<T>();
            serializableList = JsonUtility.FromJson<SerializableList<T>>(content);
            list = serializableList.list;
        }
        return list;
    }
}
