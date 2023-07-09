using Palavras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectGroupMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject content, groupItemPrefab;

    [SerializeField]
    private GameObject buttonPrefab, buttonsParent;
    
    void Awake()
    {
        // Cria os grupos e os botões
        CreateContent();
    }

    private async void CreateContent()
    {
        string ROOT_PATH = Application.isMobilePlatform ? Application.streamingAssetsPath : Application.dataPath;
        string filePath = Path.Combine(ROOT_PATH, "Data", "GrupoPalavras.txt");
        string imageFolder = Path.Combine(ROOT_PATH, "Data", "Image", "Grupo");
        List<Group> groupList = await JsonFileManager.ReadListFromJson<Group>(filePath);
        if (groupList != null && groupList.Count > 0)
        {
            foreach (Group group in groupList)
            {
                Instantiate(buttonPrefab, buttonsParent.transform, false);
                GameObject obj = Instantiate(groupItemPrefab, content.transform, false);
                obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = group.name;
                
                Texture2D texture = await FileManager.LoadImageFromDisk(Path.Combine(imageFolder, group.imageName));
                obj.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Sprite.Create(
                    texture, 
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
            }
            GameManagerScript.Instance.UpdateGameState(GameState.GroupOfWordsCreated);
            Debug.Log("Grupo de objetos criado com sucesso!");
        }
        await Task.Delay(2000);
        GameManagerScript.Instance.UpdateGameState(GameState.ContentFinishedLoading);
    }
}
