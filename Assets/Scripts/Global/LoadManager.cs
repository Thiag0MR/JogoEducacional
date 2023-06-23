using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public static async Task LoadVowels(Dictionary<string, Letter> vowels)
    {
        string filePath = Application.dataPath + "/Data/Vogais.txt";
        string audioFolder = Application.dataPath + "/Data/Audio/Vogais/";
        List<Entry> entries = await JsonFileManager.ReadListFromJson<Entry>(filePath);

        if (entries != null)
        {
            foreach(var entry in entries)
            {
                AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioFolder + entry.audioName);
                Letter vogal = new Letter(entry.name, audioClip);
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
        string filePath = Application.dataPath + "/Data/Consoantes.txt";
        string audioFolder = Application.dataPath + "/Data/Audio/Consoantes/";
        List<Entry> entries = await JsonFileManager.ReadListFromJson<Entry>(filePath);

        if (entries != null)
        {
            foreach(var entry in entries)
            {
                AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioFolder + entry.audioName);
                Letter vogal = new Letter(entry.name, audioClip);
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
        string filePath = Application.dataPath + "/Data/Palavras.txt";
        string audioFolder = Application.dataPath + "/Data/Audio/Palavras/";
        string imageFolder = Application.dataPath + "/Data/Image/Palavras/";
        List<PalavraEntry> entries = await JsonFileManager.ReadListFromJson<PalavraEntry>(filePath);
        List<Word> words = null;

        if (entries != null)
        {
            words = new List<Word>();
            foreach(var entry in entries)
            {
                if (entry.groupName.Equals(groupName))
                {
                    AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioFolder + entry.audioName);
                    Texture2D texture = await FileManager.LoadImageFromDisk(imageFolder + entry.imageName);
                    Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    Word word = new Word(entry.name, audioClip, image);
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
}
