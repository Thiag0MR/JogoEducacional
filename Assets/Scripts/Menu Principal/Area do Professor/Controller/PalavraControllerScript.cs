using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;
using System.Threading.Tasks;

public class PalavraControllerScript : BaseControllerScript
{
    [SerializeField]
    private GameObject painelAdicionarGrupo, painelDeletarGrupo;

    [SerializeField]
    private Text nomeImagem;

    [SerializeField]
    private GameObject groupItemPrefab, groupViewContent, contentPrefab;

    [SerializeField]
    private TMP_Dropdown dropdownPainelAddPalavra, dropdownPainelDeletarGrupo;

    [SerializeField]
    private TMP_InputField groupInputField;

    [SerializeField]
    private GameObject palavrasViewport;

    private List<Group> groupList;
    private string imageFolder, imagePath, groupPath;

    private new List<PalavraEntry> entryList;

    Dictionary<string, GameObject> groupNameToPalavraViewContent = new Dictionary<string, GameObject>();

    private string activeGroup;
    private Color groupSelectedColor;
    private Color groupNotSelectedColor;

    public PalavraControllerScript () : base("/Data/Palavras.txt", "/Data/Audio/Palavras/")
    {
        this.imageFolder = "/Data/Image/Palavras/";
        this.groupPath =  "/Data/GrupoPalavras.txt";
        this.imagePath = null;
        groupSelectedColor = new Color(0.000f, 0.361f, 0.776f, 0.471f);
        groupNotSelectedColor = new Color(0.000f, 0.361f, 0.776f, 0.000f);
    }
    protected override async void Awake()
    {
        this.imageFolder = Application.dataPath + this.imageFolder;
        this.groupPath = Application.dataPath + this.groupPath;
        PalavraItemScript.OnPlayAudioButtonClick += ItemScript_OnPlayAudioButtonClick;
        PalavraItemScript.OnDeleteButtonClick += ItemScript_OnDeleteButtonClick;
        GroupItemScript.OnClick += GroupItemScript_OnClick;
        this.groupList = await JsonFileManager.ReadListFromJson<Group>(this.groupPath);
        PopulateViewGroup();
        LoadDropdown();
        InitializeDictionary();
        UpdateScrollview("geral");
        base.Awake();
    }

    void OnDestroy()
    {
        PalavraItemScript.OnPlayAudioButtonClick -= ItemScript_OnPlayAudioButtonClick;
        PalavraItemScript.OnDeleteButtonClick -= ItemScript_OnDeleteButtonClick;
        GroupItemScript.OnClick -= GroupItemScript_OnClick;
    }
    public override async Task InicializarEntryList()
    {
        await Task.Delay(1000);
        this.entryList = await JsonFileManager.ReadListFromJson<PalavraEntry>(this.filePath);       
    }

    private void UpdateScrollview(string groupName)
    {
        groupNameToPalavraViewContent[groupName].gameObject.SetActive(true);
        this.activeGroup = groupName;
        Transform childTransform = this.groupViewContent.transform.Find(groupName);
        childTransform.GetComponent<Image>().color = groupSelectedColor;
        palavrasViewport.transform.parent
            .GetComponent<ScrollRect>().content = groupNameToPalavraViewContent[groupName].GetComponent<RectTransform>();
        mensagemNenhumItemAdd.gameObject.SetActive(groupNameToPalavraViewContent[groupName].transform.childCount == 0);
        viewContent = groupNameToPalavraViewContent[groupName];
    }

    private void InitializeDictionary()
    {
        foreach(var group in groupList)
        {
            AddGroupToDictionary(group.name);
        }
    }
    private void AddGroupToDictionary(string groupName)
    {
        groupNameToPalavraViewContent.Add(groupName, Instantiate(contentPrefab, palavrasViewport.transform, true));
        groupNameToPalavraViewContent[groupName].gameObject.name = "Content_" + groupName;
        groupNameToPalavraViewContent[groupName].gameObject.SetActive(false);
    }
    
    // Eventos
    private async void ItemScript_OnPlayAudioButtonClick(string name)
    {
        Entry entry = this.entryList.Find(e => e.name.Equals(name));
        AudioClip audioClip = await FileManager.LoadAudioFromDisk(audioFolder + entry.audioName);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(audioClip);
    }
    private void ItemScript_OnDeleteButtonClick(string name)
    {
        PalavraEntry entry = this.entryList.Find(e => e.name.Equals(name));
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
    private void GroupItemScript_OnClick(string groupName)
    {
        groupNameToPalavraViewContent[activeGroup].gameObject.SetActive(false);
        this.groupViewContent.transform.Find(activeGroup).GetComponent<Image>().color = groupNotSelectedColor;
        groupNameToPalavraViewContent[groupName].gameObject.SetActive(true);
        this.activeGroup = groupName;
        this.groupViewContent.transform.Find(groupName).GetComponent<Image>().color = groupSelectedColor;
        mensagemNenhumItemAdd.gameObject.SetActive(groupNameToPalavraViewContent[groupName].transform.childCount == 0);
    }

    // Relacionado ao painel adicionar palavra

    public override void HandleSalvarButtonClick()
    {
        string name = nomeInputField.text.ToLower();

        if (!name.Equals("") && audioPath != null && imagePath != null)
        {
            try
            {
                FileInfo audioInfo = new FileInfo(audioPath);
                FileInfo imageInfo = new FileInfo(imagePath);
                string audioName = audioInfo.Name;
                string imageName = imageInfo.Name;
                string newAudioName = name.Trim() + audioInfo.Extension;
                string newImageName = name.Trim() + imageInfo.Extension;
                string newAudioPath = audioFolder + newAudioName;
                string newImagePath = imageFolder + newImageName;
                string groupName = dropdownPainelAddPalavra.options[dropdownPainelAddPalavra.value].text;
                PalavraEntry entry = new PalavraEntry (name.Trim(), newAudioName, newImageName, groupName);
                if (this.entryList == null)
                {
                    this.entryList = new List<PalavraEntry>();
                }
                if (!this.entryList.Contains(entry))
                {
                    this.entryList.Add(entry);
                    AddEntry(newAudioPath, newImagePath);
                    AddGameObjectToView(entry, groupNameToPalavraViewContent[groupName]);
                }
            } catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
    private void AddEntry(string newAudioPath, string newImagePath)
    {
        base.AddEntry<PalavraEntry>(entryList, newAudioPath);
        FileManager.CopyFile(imagePath, newImagePath);
        this.imagePath = null;
        this.nomeImagem.text = "";
    }

    private void RemoveEntry (PalavraEntry entry)
    {
        this.entryList.Remove(entry);
        FileManager.DeleteFile(audioFolder + entry.audioName);
        FileManager.DeleteFile(imageFolder + entry.imageName);
        JsonFileManager.SaveListToJson<PalavraEntry>(entryList, filePath);
    }

    private void RemoveAllEntryByGroupName(string groupName)
    {
        for (int i = entryList.Count - 1; i >= 0; i--)
        {
            if (entryList[i].groupName == groupName)
            {
                FileManager.DeleteFile(audioFolder + entryList[i].audioName);
                FileManager.DeleteFile(imageFolder + entryList[i].imageName);
                entryList.RemoveAt(i);
            }
        }
        JsonFileManager.SaveListToJson<PalavraEntry>(entryList, filePath);
    }

    public void HandleAdicionarImagemButtonClick()
    {
        // https://github.com/yasirkula/UnitySimpleFileBrowser
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Downloads", Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads", null);
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Selecionar imagem", "Selecionar");

        if (FileBrowser.Success)
        {
            this.imagePath = FileBrowser.Result[0];
            FileInfo imageInfo = new(imagePath);
            this.nomeImagem.text = imageInfo.Name;
        }
    }

    // Relacionado ao painel adicionar e deletar grupo
    public void HandleAdicionarGrupoButtonClick()
    {
        painelAdicionarGrupo.SetActive(true);
    }
    public void HandleDeletarGrupoButtonClick()
    {
        painelDeletarGrupo.SetActive(true);
    }
    public void HandleFecharPainelGrupoButtonClick()
    {
        painelAdicionarGrupo.SetActive(false);
        painelDeletarGrupo.SetActive(false);
    }
    public void HandleDeletarButtonClick()
    {
        string groupName = dropdownPainelDeletarGrupo.options[dropdownPainelDeletarGrupo.value].text;
        RemoveGroup(groupName);
    }
    public void HandleSalvarGrupoButtonClick()
    {
        string name = groupInputField.text.Trim().ToLower();
        if (!name.Equals(""))
        {
            Group group = new Group(name);
            AddGroup(group);
        }
    }
    private void AddGroup(Group group)
    {
        if (groupList == null)
        {
            groupList = new List<Group>();
        }
        if (!groupList.Contains(group))
        {
            groupList.Add(group);
            AddGroupToView(group.name);
            AddGroupToDictionary(group.name);
            JsonFileManager.SaveListToJson<Group>(groupList, groupPath);
            groupInputField.text = "";
            LoadDropdown();
        }
    }

    private void RemoveGroup(string groupName)
    {
        Group group = groupList.Find(group => group.name == groupName);
        if (group != null && groupName != "geral")
        {
            groupList.Remove(group);
            RemoveGroupFromView(groupName);
            groupNameToPalavraViewContent.Remove(groupName);
            JsonFileManager.SaveListToJson<Group>(groupList, groupPath);
            RemoveAllEntryByGroupName(groupName);
            activeGroup = "geral";
            LoadDropdown();
        }
    }

    // Métodos usados para popular a interface
    public override void PopulateView()
    {
        if (groupList != null && entryList != null)
        {
            for (int i = 0; i < entryList.Count; i++)
            {
                PalavraEntry entry = entryList[i];
                AddGameObjectToView(entry, groupNameToPalavraViewContent[entry.groupName]);
            }
            mensagemNenhumItemAdd.gameObject.SetActive(groupNameToPalavraViewContent["geral"].transform.childCount == 0);
        }
    }
    private async void AddGameObjectToView(PalavraEntry entry, GameObject contentGroup)
    {
        GameObject obj = Instantiate(itemPrefab, contentGroup.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = entry.name;
        obj.name = entry.name;
        Texture2D texture = await FileManager.LoadImageFromDisk(imageFolder + entry.imageName);
        obj.GetComponentInChildren<RawImage>().texture = texture;
    }

    private void PopulateViewGroup()
    {
        if (groupList != null)
        {
            for (int i = 0; i < groupList.Count; i++)
            {
                AddGroupToView(groupList[i].name);
            }
        } else
        {
            AddGroup(new Group("geral"));
        }
    }
    
    private void AddGroupToView(string groupName)
    {
        GameObject obj = Instantiate(groupItemPrefab, groupViewContent.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = groupName;
        obj.name = groupName;
    }

    private void RemoveGroupFromView(string groupName)
    {
        GameObject contentGroup = groupNameToPalavraViewContent[groupName];
        Destroy(contentGroup);
        Transform groupItemTransform = groupViewContent.transform.Find(groupName);
        Destroy(groupItemTransform.gameObject);
    }

    // Método usado para carregar o dropdownPainelAddPalavra com os grupos disponíveis
    private void LoadDropdown()
    {
        dropdownPainelAddPalavra.ClearOptions();
        dropdownPainelDeletarGrupo.ClearOptions();
        foreach(var group in this.groupList) {
            var optionData = new TMP_Dropdown.OptionData();
            optionData.text = group.name;
            dropdownPainelAddPalavra.options.Add(optionData);
            if (group.name != "geral") dropdownPainelDeletarGrupo.options.Add(optionData);
            dropdownPainelAddPalavra.value = 0;
            dropdownPainelDeletarGrupo.value = 0;
        }
    }
}
