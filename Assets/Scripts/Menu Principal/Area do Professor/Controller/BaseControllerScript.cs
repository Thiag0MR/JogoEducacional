using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;
using Assets.SimpleSpinner;
using System.Threading.Tasks;

public class BaseControllerScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject painelAdicionar;

    [SerializeField]
    protected TMP_InputField nomeInputField;

    [SerializeField]
    protected Text nomeAudio;

    [SerializeField]
    protected TextMeshProUGUI mensagemNenhumItemAdd;

    [SerializeField]
    protected GameObject viewContent, itemPrefab;

    [SerializeField]
    protected SimpleSpinner spinner;

    protected AudioSource audioSource;
    protected List<Entry> entryList;
    protected string audioPath, filePath, audioFolder;

    public BaseControllerScript (string filePath, string audioFolder)
    {
        this.filePath = filePath;
        this.audioFolder = audioFolder;
        this.audioPath = null;
    }
    protected virtual async void Awake()
    {
        this.filePath = Application.dataPath + this.filePath;
        this.audioFolder = Application.dataPath + this.audioFolder;
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.nomeAudio.text = "";
        await InicializarEntryList();
        PopulateView();
        spinner.gameObject.SetActive(false);
        TopBarScript.OnAdicionarButtonClick += TopBarScript_OnAdicionarButtonClick;
    }

    void OnDestroy()
    {
        TopBarScript.OnAdicionarButtonClick -= TopBarScript_OnAdicionarButtonClick;
    }

    public virtual async Task InicializarEntryList()
    {
        await Task.Delay(1000);
        this.entryList = await JsonFileManager.ReadListFromJson<Entry>(this.filePath);
        mensagemNenhumItemAdd.gameObject.SetActive(entryList == null);
    }

    // Relacionado a barra de busca
    public void HandleBuscarButtonClick(string searchString)
    {
        ResetSearch();
        int children = viewContent.gameObject.transform.childCount;
        for (int i = 0; i < children; i++) {
            Transform child = viewContent.transform.GetChild(i);
            if (!child.name.Equals(searchString))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public void ResetSearch ()
    {
        int children = viewContent.gameObject.transform.childCount;
        for (int i = 0; i < children; i++) {
            Transform child = viewContent.transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
    }

    private void TopBarScript_OnAdicionarButtonClick()
    {
        painelAdicionar.gameObject.SetActive(true);
    }

    // Relacionado ao painel adicioar item (vogal, consoante, sílaba)
     
    public virtual void HandleSalvarButtonClick()
    {
        string name = nomeInputField.text.ToLower();

        if (!name.Equals("") && audioPath != null)
        {
            try
            {
                FileInfo audioInfo = new FileInfo(audioPath);
                string audioName = audioInfo.Name;
                string newAudioName = name.Trim() + audioInfo.Extension;
                string newAudioPath = audioFolder + newAudioName;
                Entry entry = new Entry(name.Trim(), newAudioName);
                if (this.entryList == null)
                    this.entryList = new List<Entry>();
                if (!this.entryList.Contains(entry))
                {
                    this.entryList.Add(entry);
                    AddEntry<Entry>(entryList, newAudioPath);
                    AddGameObjectToView(entry.name);
                }
            } catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
    
    protected void AddEntry<T>(List<T> entryList, string newAudioPath)
    {
        JsonFileManager.SaveListToJson<T>(entryList, filePath);
        FileManager.CopyFile(audioPath, newAudioPath);
        this.audioPath = null;
        this.nomeAudio.text = "";
        this.nomeInputField.text = "";
        this.mensagemNenhumItemAdd.gameObject.SetActive(false); 
    }
    public void HandleFecharButtonClick()
    {
        painelAdicionar.gameObject.SetActive(false);
    }
    
    public void HandleAdicionarAudioButtonClick()
    {
        // https://github.com/yasirkula/UnitySimpleFileBrowser
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Sounds", ".mp3", ".wav" ) );
		FileBrowser.SetDefaultFilter( ".wav" );
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
		FileBrowser.AddQuickLink( "Downloads", Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads", null );
        StartCoroutine( ShowLoadDialogCoroutine() );
    }

    IEnumerator ShowLoadDialogCoroutine()
	{
	    yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Selecionar áudio", "Selecionar" );

		if( FileBrowser.Success )
		{
            this.audioPath = FileBrowser.Result[0];
            FileInfo audioInfo = new(audioPath);
            this.nomeAudio.text = audioInfo.Name;
		}
	}

    // Relacionado ao item na scrollview

    protected async void HandlePlayAudioButtonClick(string name)
    {
        Entry entry = this.entryList.Find(e => e.name.Equals(name));
        AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioFolder + entry.audioName);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(audioClip);
    } 

    protected void HandleDeleteButtonClick(string name)
    {
        Entry entry = this.entryList.Find(e => e.name.Equals(name));
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name.Equals(entry.name))
            {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
                break;
            }
        }
        RemoveEntry(entry);
    }
    private void RemoveEntry(Entry entry)
    {
        this.entryList.Remove(entry);
        FileManager.DeleteFile(audioFolder + entry.audioName);
        JsonFileManager.SaveListToJson(entryList, filePath);
    }

    // Métodos usados para popular a interface
    public virtual void PopulateView()
    {
        if (entryList != null)
        {
            for (int i = 0; i < entryList.Count; i++) {
                AddGameObjectToView(entryList[i].name);
            }
        }
    }

    private void AddGameObjectToView(string name)
    {
        GameObject obj = Instantiate(itemPrefab, viewContent.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = name;
        obj.name = name;
    }
}
