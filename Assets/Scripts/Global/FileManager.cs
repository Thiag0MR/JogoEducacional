#nullable enable

using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class FileManager {
    public static void DeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void CopyFile (string fromPath, string toPath)
    {
        try
        {
            if (!fromPath.Equals(toPath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(toPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(toPath));
                }

                if (File.Exists(toPath))
                {
                    File.Delete(toPath);
                    Debug.Log("Arquivo " + toPath + " deletado com sucesso!");
                }                
                File.Copy(fromPath, toPath);
                Debug.Log("Arquivo " + fromPath + " copiado para " + toPath);
            }
        }
        catch (IOException ex)
        {
            Debug.LogError(ex);    
        }
    }

    public static void RenameFile (string fromPath, string toPath)
    {
        try
        {
            if (!fromPath.Equals(toPath))
            {
                File.Move(fromPath, toPath);
                Debug.Log("Arquivo " + fromPath + " movido para " + toPath);
            }
        } 
        catch (IOException ex)
        {
            Debug.LogError(ex);
        }
    }
    public async static Task<string?> ReadTextFromFile(string path)
    {
        string? content = null;
        try
        {
            // Necessário utilizar a classe UnityWebRequest no android
            if (Application.platform == RuntimePlatform.Android)
            {
                using (UnityWebRequest uwr = UnityWebRequest.Get(path))
                {
                    uwr.SendWebRequest();
                    while (!uwr.isDone) await Task.Delay(5);

                    if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                        uwr.result == UnityWebRequest.Result.ProtocolError) 
                        Debug.Log($"{uwr.error}");
                    else
                    {
                        content = uwr.downloadHandler.text;
                    }
                }
            } else
            {
                if (File.Exists(path))
                {
                    content = await File.ReadAllTextAsync(path);
                } else
                {
                    Debug.Log("File " + path + " doesn't exist!");
                }
            }
        } 
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        return content;
    }

    public async static void WriteTextToFile(string content, string path)
    {
        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                Debug.Log("Diretório " + Path.GetDirectoryName(path) + " criado com sucesso!");
            }
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                Debug.Log("File " + path + " created!");
            }
            else
            {
                Debug.Log("File " + path + " exists!");
            }
            File.SetAttributes(path, FileAttributes.Normal);
            await File.WriteAllTextAsync(path, content);
        } 
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }


    public static async Task<AudioClip?> LoadAudioFromDisk (string path)
    {
        FileInfo fileInfo = new(path);

        AudioType audioType = AudioType.WAV;
        if (fileInfo.Extension.Equals(".mp3")) 
            audioType = AudioType.MPEG;

        AudioClip? audioClip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError) 
                    Debug.Log($"{uwr.error}");
                else
                {
                    audioClip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return audioClip;
    }

    public static async Task<Texture2D?> LoadImageFromDisk(string path)
    {
        Texture2D? texture = null;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError) 
                    Debug.Log($"{uwr.error}");
                else
                {
                    texture = DownloadHandlerTexture.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }
        return texture;
    }

    public static async Task<Texture2D?> LoadImageFromDisk1 (string path)
    {
        Texture2D? texture = null;
        try
        {
            byte[] bytes = await File.ReadAllBytesAsync(path);
            texture = new Texture2D(1,1);
            texture.LoadImage(bytes);
        } catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return texture;
    }
}
