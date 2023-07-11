using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public static async Task LoadVowels(Dictionary<string, Letter> vowels)
    {
        string ROOT_PATH = GetRootPath();
        string filePath = Path.Combine(ROOT_PATH, "Data", "Vogais.txt");
        string audioFolder = Path.Combine(ROOT_PATH, "Data", "Audio", "Vogais");
        List<Entry> entries = await JsonFileManager.ReadListFromJson<Entry>(filePath);

        if (entries != null && entries.Count > 0)
        {
            foreach(var entry in entries)
            {
                AudioClip audioClip = await FileManager.LoadAudioFromDisk(Path.Combine(audioFolder, entry.audioName));
                Letter vogal = new(entry.name, audioClip);
                vowels.Add(entry.name, vogal);
            }
            Debug.Log("Vogais carregadas com sucesso!");
        } else
        {
            Debug.Log("Erro ao carregar vogais");
        }

    }
    public static async Task LoadConsonants(Dictionary<string, Letter> consonants)
    {
        string ROOT_PATH = GetRootPath();
        string filePath = Path.Combine(ROOT_PATH, "Data", "Consoantes.txt");
        string audioFolder = Path.Combine(ROOT_PATH, "Data", "Audio", "Consoantes");
        List<Entry> entries = await JsonFileManager.ReadListFromJson<Entry>(filePath);

        if (entries != null && entries.Count > 0)
        {
            foreach(var entry in entries)
            {
                AudioClip audioClip = await FileManager.LoadAudioFromDisk(Path.Combine(audioFolder, entry.audioName));
                Letter vogal = new(entry.name, audioClip);
                consonants.Add(entry.name, vogal);
            }
            Debug.Log("Consoantes carregadas com sucesso!");
        } else
        {
            Debug.Log("Erro ao carregar consoantes!");
        }
    }
    public static async Task<List<Word>> LoadWords(string groupName)
    {
        string ROOT_PATH = GetRootPath();
        string filePath = Path.Combine(ROOT_PATH, "Data", "Palavras.txt");
        string audioFolder = Path.Combine(ROOT_PATH, "Data", "Audio", "Palavras");
        string imageFolder = Path.Combine(ROOT_PATH, "Data", "Image", "Palavras");
        List<PalavraEntry> entries = await JsonFileManager.ReadListFromJson<PalavraEntry>(filePath);
        List<Word> words = null;

        if (entries != null && entries.Count > 0)
        {
            words = new List<Word>();
            foreach(var entry in entries)
            {
                if (entry.groupName.Equals(groupName))
                {
                    string audioPath = Path.Combine(audioFolder, entry.groupName, entry.audioName);
                    string imagePath = Path.Combine(imageFolder, entry.groupName, entry.imageName);
                    AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioPath);
                    Texture2D texture = await FileManager.LoadImageFromDisk(imagePath);
                    Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    Word word = new(entry.name, audioClip, image);
                    words.Add(word);
                }
            }
            Debug.Log("Palavras carregadas com sucesso!");
        } else
        {
            Debug.Log("Erro ao carregar palavras!");
        }
        return words;
    }
    private static string GetRootPath()
    {
        return Application.isMobilePlatform ? Application.streamingAssetsPath : Application.dataPath;
    }
}
