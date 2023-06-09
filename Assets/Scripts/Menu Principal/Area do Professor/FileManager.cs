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
            if (!Directory.Exists(Path.GetDirectoryName(toPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(toPath));
            }

            if (File.Exists(toPath))
            {
                File.Delete(toPath);
            }                
            File.Copy(fromPath, toPath);
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
            if (File.Exists(path))
            {
                content = await File.ReadAllTextAsync(path);
            } else
            {
                Debug.Log("File doesn't exist!");
            }
        } catch(Exception e)
        {
            Debug.LogException(e);
        }
        return content;
    }

    public async static void WriteTextToFile(string content, string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                Debug.Log("File created");
            }
            File.SetAttributes(path, FileAttributes.Normal);
            await File.WriteAllTextAsync(path, content);
        } catch(Exception e)
        {
            Debug.LogException(e);
        }
    }


    public static async Task<AudioClip?> LoadAudioFromDisk (string path)
    {
        AudioClip? clip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
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
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return clip;
    }
    public static async Task<Texture2D?> LoadImageFromDisk (string path)
    {
        Texture2D? loadTexture = null;
        try
        {
            byte[] bytes = await File.ReadAllBytesAsync(path);
            loadTexture = new Texture2D(1,1);
            loadTexture.LoadImage(bytes);
        } catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return loadTexture;
    }
}
