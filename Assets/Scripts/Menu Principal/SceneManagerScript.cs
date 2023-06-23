using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance;

    private GameObject spinner, menus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        if (!sceneName.Equals("Menu Principal"))
        {
            GameObject.FindGameObjectWithTag("Menus").SetActive(false);
            GameObject.FindGameObjectWithTag("Spinner").transform.GetChild(0).gameObject.SetActive(true);
        }

        do
        {
        } while (scene.progress < 0.9f);

        await Task.Delay(1000);

        if (!sceneName.Equals("Menu Principal")) 
        {
            GameObject.FindGameObjectWithTag("Spinner").transform.GetChild(0).gameObject.SetActive(true);
        }
        scene.allowSceneActivation = true;
    }
}
