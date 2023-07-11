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
    protected GameObject painelAdicionar, botoesPainelAdicionar;

    [SerializeField]
    protected TMP_InputField nomeInputFieldPainelAdicionar;

    [SerializeField]
    protected Text nomeAudioPainelAdicionar;

    [SerializeField]
    protected TextMeshProUGUI nomeInputPlaceholderPainelAdicionar, mensagemNenhumItemAddTxtMeshPro, mensagemResultadoOperacaoTxtMeshPro;

    [SerializeField]
    protected GameObject viewContent, itemPrefab;

    [SerializeField]
    protected SimpleSpinner spinner;

    protected AudioSource audioSource;
    protected List<Entry> entryList;
    protected string selectedAudioPathFromDisk, jsonFilePath, audioFolder, nomePlaceholder, mensagemResultadoOperacao;

    protected int editEntryIndex;

    public BaseControllerScript (string filePath, string audioFolder, string nomePlaceholder)
    {
        this.jsonFilePath = filePath;
        this.audioFolder = audioFolder;
        this.nomePlaceholder = nomePlaceholder;
        this.selectedAudioPathFromDisk = null;
    }
    protected virtual async void Awake()
    {
        string ROOT_PATH = Application.isMobilePlatform ? Application.streamingAssetsPath : Application.dataPath;
        this.jsonFilePath = Path.Combine(ROOT_PATH, this.jsonFilePath);
        this.audioFolder = Path.Combine(ROOT_PATH, this.audioFolder);
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.nomeInputPlaceholderPainelAdicionar.text = this.nomePlaceholder;
        this.nomeAudioPainelAdicionar.text = "";
        await InitializeEntryList();
        PopulateView();
        spinner.gameObject.SetActive(false);
        Debug.Log("Awake BaseController executado com sucesso!");
    }

    void OnEnable()
    {
        TopBarScript.OnAdicionarButtonClick += TopBarScript_OnAdicionarButtonClick;
        ItemScript.OnPlayAudioButtonClick += ItemScript_OnPlayAudioButtonClick;
        ItemScript.OnEditButtonClick += ItemScript_OnEditButtonClick;
        ItemScript.OnDeleteButtonClick += ItemScript_OnDeleteButtonClick;
    }
    void OnDisable()
    {
        TopBarScript.OnAdicionarButtonClick -= TopBarScript_OnAdicionarButtonClick;
        ItemScript.OnPlayAudioButtonClick -= ItemScript_OnPlayAudioButtonClick;
        ItemScript.OnEditButtonClick -= ItemScript_OnEditButtonClick;
        ItemScript.OnDeleteButtonClick -= ItemScript_OnDeleteButtonClick;
    }

    // Relacionado aos eventos

    private void ItemScript_OnDeleteButtonClick(string name)
    {
        HandleDeleteButtonClickItem(name);
    }

    private void ItemScript_OnEditButtonClick(string name)
    {
        HandleEditButtonClickItem(name);
    }

    private void ItemScript_OnPlayAudioButtonClick(string name)
    {
        Entry entry = entryList.Find(e => e.name.Equals(name));
        HandlePlayAudioButtonClickItem(Path.Combine(audioFolder, entry.audioName));
    }
    private void TopBarScript_OnAdicionarButtonClick()
    {
        AtivarBotaoSalvar();
        painelAdicionar.gameObject.SetActive(true);
    }

    // Relacionado a inicialização
    public virtual async Task InitializeEntryList()
    {
        await Task.Delay(1000);
        this.entryList = await JsonFileManager.ReadListFromJson<Entry>(this.jsonFilePath);
        if (entryList == null || entryList.Count == 0 )
        {
            mensagemNenhumItemAddTxtMeshPro.gameObject.SetActive(true);
            Debug.Log("Lista vazia");
        } else
        {
            Debug.Log("Lista lida com sucesso!");
        }
    }

    // Relacionado a barra de busca
    public void HandleBuscarButtonClick(string searchString)
    {
        ResetSearch();
        int children = viewContent.gameObject.transform.childCount;
        for (int i = 0; i < children; i++) {
            Transform child = viewContent.transform.GetChild(i);
            if (!child.name.Contains(searchString))
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

    // Relacionado ao painel adicionar item (vogal, consoante, sílaba)
     
    public virtual void HandleSalvarButtonClickPainelAdicionar()
    {
        string name = nomeInputFieldPainelAdicionar.text.ToLower();

        if (!name.Equals("") && selectedAudioPathFromDisk != null)
        {
            try
            {
                FileInfo audioInfo = new FileInfo(selectedAudioPathFromDisk);
                string newAudioName = name.Trim() + audioInfo.Extension;
                string newAudioPath = Path.Combine(audioFolder, newAudioName);
                Entry entry = new Entry(name.Trim(), newAudioName);
                if (this.entryList == null)
                    this.entryList = new List<Entry>();
                if (!this.entryList.Contains(entry))
                {
                    AddEntry<Entry>(entryList, entry, newAudioPath);
                    AddGameObjectToView(entry.name);
                    Debug.Log("Item adicionado com sucesso!");
                    MostrarResultadoDaOperacao("Item adicionado com sucesso!");
                }
            } catch (Exception ex)
            {
                Debug.LogError(ex);
            }        
        }
    }
    
    protected void AddEntry<T>(List<T> entryList, T entry, string newAudioPath)
    {
        entryList.Add(entry);
        JsonFileManager.SaveListToJson<T>(entryList, jsonFilePath);
        FileManager.CopyFile(selectedAudioPathFromDisk, newAudioPath);
        LimparCamposPainelAdicionar();
        this.mensagemNenhumItemAddTxtMeshPro.gameObject.SetActive(false); 
    }
    public virtual void HandleEditarButtonClickPainelAdicionar()
    {
        string newName = nomeInputFieldPainelAdicionar.text.ToLower();

        if (!newName.Equals("") && selectedAudioPathFromDisk != null)
        {
            try
            {
                bool changeAudio = false;
                string message;
                Entry oldEntry = this.entryList[editEntryIndex];
                string oldName = oldEntry.name;
                GameObject obj = viewContent.transform.Find(oldName).gameObject;
                if (!oldName.Equals(newName))
                {
                    if (this.entryList.Find(entry => entry.name.Equals(newName)) != null)
                    {
                        message = "Já existe outra palavra com o mesmo nome!"; 
                        Debug.Log(message);
                        MostrarResultadoDaOperacao(message);
                        return;
                    }
                    oldEntry.name = newName;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = newName;
                    obj.name = newName;

                    changeAudio = true;
                }
                string oldAudioPath = Path.Combine(audioFolder, oldEntry.audioName);
                bool newAudioAdded = !selectedAudioPathFromDisk.Equals(oldAudioPath);
                if (newAudioAdded || changeAudio)
                {
                    FileInfo audioInfo = new FileInfo(newAudioAdded ? selectedAudioPathFromDisk : oldAudioPath);
                    string newAudioName = newName.Trim() + audioInfo.Extension;
                    string newAudioPath = Path.Combine(audioFolder, newAudioName);
                    oldEntry.audioName = newAudioName;
                    if (newAudioAdded)
                    {
                        FileManager.DeleteFile(oldAudioPath);
                        FileManager.CopyFile(selectedAudioPathFromDisk, newAudioPath);
                    } else
                    {
                        FileManager.MoveFile(oldAudioPath, newAudioPath);
                    }
                }
                JsonFileManager.SaveListToJson(entryList, jsonFilePath);
                message = "Palavra editada com sucesso!"; 
                Debug.Log(message);
                MostrarResultadoDaOperacao(message);
                LimparCamposPainelAdicionar();
            } catch(Exception ex)
            {
                Debug.LogError(ex.Message);
            } 
        }
    }
    
    public void HandleFecharButtonClickPainelAdicionar()
    {
        painelAdicionar.gameObject.SetActive(false);
        LimparCamposPainelAdicionar();
    }
    
    public void HandleEscolherAudioButtonClickPainelAdicionar()
    {
        // https://github.com/yasirkula/UnitySimpleFileBrowser
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Sounds", ".mp3", ".wav" ) );
		FileBrowser.SetDefaultFilter( ".wav" );
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
		FileBrowser.AddQuickLink( "Downloads", Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), null );
        StartCoroutine( ShowLoadDialogCoroutine() );
    }

    IEnumerator ShowLoadDialogCoroutine()
	{
	    yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Selecionar áudio", "Selecionar" );

		if( FileBrowser.Success )
		{
            this.selectedAudioPathFromDisk = FileBrowser.Result[0];
            FileInfo audioInfo = new(selectedAudioPathFromDisk);
            this.nomeAudioPainelAdicionar.text = audioInfo.Name;
		}
	}
    public void PreencherCamposPainelAdicionar(Entry entry)
    {
        this.nomeInputFieldPainelAdicionar.text = entry.name;
        this.nomeAudioPainelAdicionar.text = entry.audioName;
        this.selectedAudioPathFromDisk = Path.Combine(audioFolder, entry.audioName);
    }
    public virtual void LimparCamposPainelAdicionar()
    {
        this.nomeInputFieldPainelAdicionar.text = "";
        this.nomeAudioPainelAdicionar.text = "";
        this.selectedAudioPathFromDisk = null;
    }
    protected void AtivarBotaoSalvar()
    {
        this.botoesPainelAdicionar.transform.Find("Salvar").gameObject.SetActive(true);
        this.botoesPainelAdicionar.transform.Find("Editar").gameObject.SetActive(false);
    }
    protected void AtivarBotaoEditar()
    {
        this.botoesPainelAdicionar.transform.Find("Salvar").gameObject.SetActive(false);
        this.botoesPainelAdicionar.transform.Find("Editar").gameObject.SetActive(true);
    }

    protected async void MostrarResultadoDaOperacao(string mensagem)
    {
        this.mensagemResultadoOperacaoTxtMeshPro.text = mensagem;
        await Task.Delay(2000);
        this.mensagemResultadoOperacaoTxtMeshPro.text = "";
    }

    // Relacionado ao item na scrollview

    protected async void HandlePlayAudioButtonClickItem(string audioPath)
    {
        AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioPath);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(audioClip);
        Debug.Log("Play audio");
    } 

    private void HandleEditButtonClickItem(string name) 
    {
        Entry entry = this.entryList.Find(e => e.name.Equals(name));
        if (entry != null)
        {
            PreencherCamposPainelAdicionar(entry);
            AtivarBotaoEditar();
            painelAdicionar.SetActive(true);
            editEntryIndex = this.entryList.IndexOf(entry);
        }
    }

    private void HandleDeleteButtonClickItem(string name)
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
        Debug.Log("Item removido com sucesso!");
    }
    private void RemoveEntry(Entry entry)
    {
        this.entryList.Remove(entry);
        FileManager.DeleteFile(Path.Combine(audioFolder, entry.audioName));
        JsonFileManager.SaveListToJson(entryList, jsonFilePath);
    }

    // Métodos usados para popular a interface
    public virtual void PopulateView()
    {
        if (entryList != null)
        {
            for (int i = 0; i < entryList.Count; i++) {
                AddGameObjectToView(entryList[i].name);
            }
            Debug.Log("View populada com sucesso!");
        }
    }

    private void AddGameObjectToView(string name)
    {
        GameObject obj = Instantiate(itemPrefab, viewContent.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = name;
        obj.name = name;
    }
    private void EditGameObjectInView(string oldName, string newName)
    {
        if (!oldName.Equals(newName))
        {
            GameObject obj = viewContent.transform.Find(oldName).gameObject;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = newName;
            obj.name = newName;
        }
    }
}
