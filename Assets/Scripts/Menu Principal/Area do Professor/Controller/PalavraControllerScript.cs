using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

public class PalavraControllerScript : BaseControllerScript
{
    [SerializeField]
    private GameObject painelAdicionarGrupo, painelDeletarGrupo, painelAdvertencia, botoesPainelAdicionarGrupo;

    [SerializeField]
    private Text nomeImagemPainelAdicionarPalavra, nomeImagemPainelAdicionarGrupo;

    [SerializeField]
    protected TextMeshProUGUI mensagemResultadoOperacaoGrupoAddTxtMeshPro, mensagemResultadoOperacaoGrupoDelTxtMeshPro, mensagemNenhumGrupoCriadoTxtMeshPro;

    [SerializeField]
    private GameObject groupItemPrefab, groupViewContent, contentPrefab;

    [SerializeField]
    private TMP_Dropdown dropdownPainelAdicionarPalavra, dropdownPainelDeletarGrupo;

    [SerializeField]
    private TMP_InputField groupInputField;

    [SerializeField]
    private GameObject palavrasViewport;

    private List<Group> groupList;
    private string wordImageFolder, groupImageFolder, selectedImagePathFromDisk, groupPath;

    private new List<PalavraEntry> entryList;

    Dictionary<string, GameObject> groupNameToPalavraViewContent = new Dictionary<string, GameObject>();

    private string activeGroup;
    private int editGroupIndex;
    private Color groupSelectedColor;
    private Color groupNotSelectedColor;

    public PalavraControllerScript () : base(
            Path.Combine("Data", "Palavras.txt"),
            Path.Combine("Data","Audio", "Palavras"),
            "Digite o nome da palavra")
    {
        this.wordImageFolder = Path.Combine("Data", "Image", "Palavras");
        this.groupImageFolder = Path.Combine("Data", "Image", "Grupo");
        this.groupPath = Path.Combine("Data", "GrupoPalavras.txt");
        this.selectedImagePathFromDisk = null;
        this.groupSelectedColor = new Color(0.000f, 0.361f, 0.776f, 0.471f);
        this.groupNotSelectedColor = new Color(0.000f, 0.361f, 0.776f, 0.000f);
    }
    protected override async void Awake()
    {
        string ROOT_PATH = Application.isMobilePlatform ? Application.streamingAssetsPath : Application.dataPath;
        this.wordImageFolder = Path.Combine(ROOT_PATH, this.wordImageFolder);
        this.groupImageFolder = Path.Combine(ROOT_PATH, this.groupImageFolder);
        this.groupPath = Path.Combine(ROOT_PATH, this.groupPath);
        await InitializeGroupList();
        PopulateViewGroup();
        InitializeDictionary();
        ActivateGroupInTheView();
        LoadDropdown();
        base.Awake();
        Debug.Log("Awake PalavraController executado com sucesso!");
    }

    void OnEnable()
    {
        PalavraItemScript.OnPlayAudioButtonClick += PalavraItemScript_OnPlayAudioButtonClick;
        PalavraItemScript.OnDeleteButtonClick += PalavraItemScript_OnDeleteButtonClick;
        PalavraItemScript.OnEditButtonClick += PalavraItemScript_OnEditButtonClick;
        GroupItemScript.OnClick += GroupItemScript_OnClick;
        TopBarScript.OnAdicionarButtonClick += TopBarScript_OnAdicionarButtonClick;
    }

    void OnDisable()
    {
        PalavraItemScript.OnPlayAudioButtonClick -= PalavraItemScript_OnPlayAudioButtonClick;
        PalavraItemScript.OnDeleteButtonClick -= PalavraItemScript_OnDeleteButtonClick;
        PalavraItemScript.OnEditButtonClick -= PalavraItemScript_OnEditButtonClick;
        GroupItemScript.OnClick -= GroupItemScript_OnClick;
        TopBarScript.OnAdicionarButtonClick -= TopBarScript_OnAdicionarButtonClick;
    }

    // Eventos
    private void PalavraItemScript_OnPlayAudioButtonClick(string name)
    {
        PalavraEntry entry = entryList.Find(e => e.name.Equals(name));
        base.HandlePlayAudioButtonClickItem(Path.Combine(audioFolder, entry.groupName, entry.audioName));
    }
    private void PalavraItemScript_OnDeleteButtonClick(string name)
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
        Debug.Log("Palavra removida com sucesso!");
    }
    private void PalavraItemScript_OnEditButtonClick(string name)
    {
        PalavraEntry entry = this.entryList.Find(e => e.name.Equals(name));
        if (entry != null)
        {
            PreencherCamposPainelAdicionar(entry);
            base.AtivarBotaoEditar();
            painelAdicionar.SetActive(true);
            editEntryIndex = this.entryList.IndexOf(entry);
        }
    }
    private void GroupItemScript_OnClick(string groupName)
    {
        groupNameToPalavraViewContent[activeGroup].gameObject.SetActive(false);
        groupViewContent.transform.Find(activeGroup).GetComponent<Image>().color = groupNotSelectedColor;
        groupNameToPalavraViewContent[groupName].gameObject.SetActive(true);
        activeGroup = groupName;
        groupViewContent.transform.Find(groupName).GetComponent<Image>().color = groupSelectedColor;
        viewContent = groupNameToPalavraViewContent[groupName];
        palavrasViewport.transform.parent
            .GetComponent<ScrollRect>().content = groupNameToPalavraViewContent[groupName].GetComponent<RectTransform>();
        mensagemNenhumItemAddTxtMeshPro.gameObject.SetActive(groupNameToPalavraViewContent[groupName].transform.childCount == 0);
    }
    private void TopBarScript_OnAdicionarButtonClick()
    {
        if (groupList != null && groupList.Count != 0)
        {
            SetDropdownValue(dropdownPainelAdicionarPalavra, activeGroup);
            base.AtivarBotaoSalvar();
            painelAdicionar.gameObject.SetActive(true);
        } 
        else
        {
            painelAdvertencia.gameObject.SetActive(true);
        }
    }

    // Relacionado a inicialização
    private async Task InitializeGroupList()
    {
        this.groupList = await JsonFileManager.ReadListFromJson<Group>(this.groupPath);
        if (this.groupList == null || this.groupList.Count == 0)
        {
            mensagemNenhumGrupoCriadoTxtMeshPro.gameObject.SetActive(true);
            mensagemNenhumItemAddTxtMeshPro.gameObject.SetActive(true);
            Debug.Log("Nenhum grupo criado!");
        } else
        {
            mensagemNenhumGrupoCriadoTxtMeshPro.gameObject.SetActive(false);
        }
    }
    public override async Task InitializeEntryList()
    {
        await Task.Delay(1000);
        this.entryList = await JsonFileManager.ReadListFromJson<PalavraEntry>(this.jsonFilePath);   
    }
    private void InitializeDictionary()
    {
        if (groupList != null && groupList.Count > 0)
        {
            foreach(var group in groupList)
            {
                AddGroupToDictionary(group.name);
            }
            Debug.Log("Dicionário inicializado com sucesso!");
        } else
        {
            Debug.Log("Dicionário não inicializado pois groupList é nulo ou está vazio!");
        }
    }
    private void AddGroupToDictionary(string groupName)
    {
        if (!groupNameToPalavraViewContent.ContainsKey(groupName))
        {
            groupNameToPalavraViewContent.Add(groupName, Instantiate(contentPrefab, palavrasViewport.transform, false));
            groupNameToPalavraViewContent[groupName].gameObject.name = "Content_" + groupName;
            groupNameToPalavraViewContent[groupName].gameObject.SetActive(false);
        }
    }

    // Relacionado ao painel adicionar palavra

    public override void HandleSalvarButtonClickPainelAdicionar()
    {
        string name = nomeInputFieldPainelAdicionar.text.ToLower();

        if (!name.Equals("") && selectedAudioPathFromDisk != null && selectedImagePathFromDisk != null 
            && dropdownPainelAdicionarPalavra.options.Count > 0)
        {
            try
            {
                FileInfo audioInfo = new FileInfo(selectedAudioPathFromDisk);
                FileInfo imageInfo = new FileInfo(selectedImagePathFromDisk);
                string groupName = dropdownPainelAdicionarPalavra.options[dropdownPainelAdicionarPalavra.value].text;
                string newAudioName = name.Trim() + audioInfo.Extension;
                string newImageName = name.Trim() + imageInfo.Extension;
                string newAudioPath = Path.Combine(audioFolder, groupName, newAudioName);
                string newImagePath = Path.Combine(wordImageFolder, groupName, newImageName);
                PalavraEntry entry = new PalavraEntry (name.Trim(), newAudioName, newImageName, groupName);
                if (this.entryList == null)
                {
                    this.entryList = new List<PalavraEntry>();
                }
                if (!this.entryList.Contains(entry))
                {
                    AddEntry(entry, newAudioPath, newImagePath);
                    AddGameObjectToView(entry, groupNameToPalavraViewContent[groupName]);
                    Debug.Log("Palavra adicionada com sucesso!");
                    base.MostrarResultadoDaOperacao("Palavra adicionada com sucesso!");
                } else
                {
                    base.MostrarResultadoDaOperacao("Palavra já existe! Não adicionada.");
                }
            } catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
    public async override void HandleEditarButtonClickPainelAdicionar()
    {
        string newName = nomeInputFieldPainelAdicionar.text.ToLower();

        if (!newName.Equals("") && selectedAudioPathFromDisk != null && selectedImagePathFromDisk != null 
            && dropdownPainelAdicionarPalavra.options.Count > 0)
        {
            try
            {
                string message;
                bool changeAudio = false;
                bool changeImage = false;
                PalavraEntry oldEntry = this.entryList[editEntryIndex];
                string oldName = oldEntry.name;
                GameObject obj = viewContent.transform.Find(oldName).gameObject;

                string oldGroupName = oldEntry.groupName;
                string newGroupName = dropdownPainelAdicionarPalavra.options[dropdownPainelAdicionarPalavra.value].text;

                if (!oldName.Equals(newName))
                {
                    if(this.entryList.Find(entry => entry.name.Equals(newName) && entry.groupName.Equals(newGroupName)) != null) {
                        message = "Já existe outra palavra com o mesmo nome!";
                        Debug.Log(message);
                        base.MostrarResultadoDaOperacao(message);
                        return;
                    }
                    oldEntry.name = newName;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = newName;
                    obj.name = newName;

                    changeAudio = true;
                    changeImage = true;
                }

                if (!oldGroupName.Equals(newGroupName))
                {
                    oldEntry.groupName = newGroupName;
                    obj.transform.SetParent(groupNameToPalavraViewContent[newGroupName].transform);
                    changeAudio = true;
                    changeImage = true;
                }

                string oldAudioPath = Path.Combine(audioFolder, oldGroupName, oldEntry.audioName);
                bool newAudioAdded = !selectedAudioPathFromDisk.Equals(oldAudioPath);
                if (newAudioAdded || changeAudio)
                {
                    FileInfo audioInfo = new FileInfo(newAudioAdded ? selectedAudioPathFromDisk : oldAudioPath);
                    string newAudioName = newName.Trim() + audioInfo.Extension;
                    string newAudioPath = Path.Combine(audioFolder, newGroupName, newAudioName);
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

                string oldImagePath = Path.Combine(wordImageFolder, oldGroupName, oldEntry.imageName);
                bool newImageAdded = !selectedImagePathFromDisk.Equals(oldImagePath); 
                if (newImageAdded || changeImage)
                {
                    FileInfo imageInfo = new FileInfo(newImageAdded ? selectedImagePathFromDisk : oldImagePath);
                    string newImageName = newName.Trim() + imageInfo.Extension;
                    string newImagePath = Path.Combine(wordImageFolder, newGroupName, newImageName);
                    oldEntry.imageName = newImageName;
                    if (newImageAdded)
                    {
                        FileManager.DeleteFile(oldImagePath);
                        FileManager.CopyFile(selectedImagePathFromDisk, newImagePath);
                    } else
                    {
                        FileManager.MoveFile(oldImagePath, newImagePath);
                    }
                    Texture2D texture = await FileManager.LoadImageFromDisk(newImagePath);
                    obj.GetComponentInChildren<RawImage>().texture = texture;
                }
                JsonFileManager.SaveListToJson(entryList, jsonFilePath);
                message = "Palavra editada com sucesso!"; 
                Debug.Log(message);
                base.MostrarResultadoDaOperacao(message);
                LimparCamposPainelAdicionar();
            } catch(Exception ex)
            {
                Debug.LogError(ex.Message);
            } 
        }
    }
    public void HandleEscolherImagemButtonClickPainelAdicionar()
    {
        // https://github.com/yasirkula/UnitySimpleFileBrowser
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Downloads", Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), null);
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Selecionar imagem", "Selecionar");

        if (FileBrowser.Success)
        {
            this.selectedImagePathFromDisk = FileBrowser.Result[0];
            FileInfo imageInfo = new(selectedImagePathFromDisk);
            this.nomeImagemPainelAdicionarPalavra.text = imageInfo.Name;
            this.nomeImagemPainelAdicionarGrupo.text = imageInfo.Name;
        }
    }
    private void AddEntry(PalavraEntry entry, string newAudioPath, string newImagePath)
    {
        AddEntry(entryList, entry, newAudioPath);
        FileManager.CopyFile(selectedImagePathFromDisk, newImagePath);
        selectedImagePathFromDisk = null;
        nomeImagemPainelAdicionarPalavra.text = "";
    }

    private void RemoveEntry (PalavraEntry entry)
    {
        entryList.Remove(entry);
        FileManager.DeleteFile(Path.Combine(audioFolder, entry.groupName, entry.audioName));
        FileManager.DeleteFile(Path.Combine(wordImageFolder, entry.groupName, entry.imageName));
        JsonFileManager.SaveListToJson<PalavraEntry>(entryList, jsonFilePath);
    }

    private void RemoveAllEntryByGroupName(string groupName)
    {
        if (entryList != null)
        {
            for (int i = entryList.Count - 1; i >= 0; i--)
            {
                if (entryList[i].groupName == groupName)
                {
                    FileManager.DeleteFile(Path.Combine(audioFolder, entryList[i].groupName, entryList[i].audioName));
                    FileManager.DeleteFile(Path.Combine(wordImageFolder, entryList[i].groupName, entryList[i].imageName));
                    entryList.RemoveAt(i);
                }
            }
            JsonFileManager.SaveListToJson<PalavraEntry>(entryList, jsonFilePath);
        }
    }

    public void PreencherCamposPainelAdicionar(PalavraEntry entry)
    {
        nomeInputFieldPainelAdicionar.text = entry.name;
        nomeAudioPainelAdicionar.text = entry.audioName;
        selectedAudioPathFromDisk = Path.Combine(audioFolder, entry.groupName, entry.audioName);
        nomeImagemPainelAdicionarPalavra.text = entry.imageName;
        selectedImagePathFromDisk = Path.Combine(wordImageFolder, entry.groupName, entry.imageName);
        if (dropdownPainelAdicionarPalavra.options.Count > 1)
        {
            dropdownPainelAdicionarPalavra.value = FindDropdownIndex(activeGroup);
        }
    }

    private int FindDropdownIndex(string activeGroup)
    {
        for (int i = 0; i < dropdownPainelAdicionarPalavra.options.Count; i++)
        {
            if (dropdownPainelAdicionarPalavra.options[i].text.Equals(activeGroup))
            {
                return i;
            }
        }
        return 0;
    }

    public override void LimparCamposPainelAdicionar()
    {
        base.LimparCamposPainelAdicionar();
        this.nomeImagemPainelAdicionarPalavra.text = "";
        this.nomeImagemPainelAdicionarGrupo.text = "";
        //this.dropdownPainelAdicionarPalavra.value = 0;
    }
    private async void MostrarResultadoDaOperacaoGrupoAdd(string mensagem)
    {
        this.mensagemResultadoOperacaoGrupoAddTxtMeshPro.text = mensagem;
        await Task.Delay(2000);
        this.mensagemResultadoOperacaoGrupoAddTxtMeshPro.text = "";
    }
    private async void MostrarResultadoDaOperacaoGrupoDel(string mensagem)
    {
        this.mensagemResultadoOperacaoGrupoDelTxtMeshPro.text = mensagem;
        await Task.Delay(2000);
        this.mensagemResultadoOperacaoGrupoDelTxtMeshPro.text = "";
    }


    // Relacionado ao painel adicionar, editar e deletar grupo

    public void HandleAdicionarGrupoButtonClick()
    {
        AtivarBotaoSalvarPainelAdicionarGrupo();
        painelAdicionarGrupo.SetActive(true);
    }
    public void HandleDeletarGrupoButtonClick()
    {
        SetDropdownValue(dropdownPainelDeletarGrupo, activeGroup);
        painelDeletarGrupo.SetActive(true);
    }
    public void HandleEditarGrupoButtonClick()
    {
        Group group = groupList.Find(e => e.name.Equals(activeGroup));
        if (group != null)
        {
            PreencherCamposPainelAdicionarGrupo(group);
            AtivarBotaoEditarPainelAdicionarGrupo();
            painelAdicionarGrupo.SetActive(true);
            editGroupIndex = groupList.IndexOf(group);
        }
    }
    private void AtivarBotaoSalvarPainelAdicionarGrupo()
    {
        this.botoesPainelAdicionarGrupo.transform.Find("Salvar").gameObject.SetActive(true);
        this.botoesPainelAdicionarGrupo.transform.Find("Editar").gameObject.SetActive(false);
    }
    protected void AtivarBotaoEditarPainelAdicionarGrupo()
    {
        this.botoesPainelAdicionarGrupo.transform.Find("Salvar").gameObject.SetActive(false);
        this.botoesPainelAdicionarGrupo.transform.Find("Editar").gameObject.SetActive(true);
    }
    private void PreencherCamposPainelAdicionarGrupo(Group group)
    {
        groupInputField.text = group.name;
        selectedImagePathFromDisk = Path.Combine(groupImageFolder, group.imageName);
        nomeImagemPainelAdicionarGrupo.text = group.imageName;
    }
    public void HandleFecharButtonClickPainelGrupo()
    {
        painelAdicionarGrupo.SetActive(false);
        painelDeletarGrupo.SetActive(false);
    }
    public void HandleDeletarButtonClickPainelDeletarGrupo()
    {
        if (dropdownPainelDeletarGrupo.options.Count > 0)
        {
            string groupName = dropdownPainelDeletarGrupo.options[dropdownPainelDeletarGrupo.value].text;
            RemoveGroup(groupName);
        }
    }
    public async void HandleEditarButtonClickPainelAdicionarGrupo()
    {
        string newGroupName = groupInputField.text.ToLower();

        if (!newGroupName.Equals("") && selectedImagePathFromDisk != null)
        {
            try
            {
                string message;
                string oldImagePath;
                Group oldGroup = this.groupList[editGroupIndex];
                string oldGroupName = oldGroup.name;
                GameObject obj = groupViewContent.transform.Find(oldGroupName).gameObject;
                if (!oldGroupName.Equals(newGroupName))
                {
                    if(this.groupList.Find(entry => entry.name.Equals(newGroupName)) != null) {
                        message = "Já existe outro grupo com o mesmo nome!"; 
                        Debug.Log(message);
                        MostrarResultadoDaOperacaoGrupoAdd(message);
                        return;
                    }
                    groupNameToPalavraViewContent[oldGroup.name].name = "Content_" + newGroupName;
                    groupNameToPalavraViewContent.Add(newGroupName, groupNameToPalavraViewContent[oldGroup.name]);
                    groupNameToPalavraViewContent.Remove(oldGroup.name);
                    activeGroup = newGroupName;
                    
                    oldGroup.name = newGroupName;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = newGroupName;
                    obj.name = newGroupName;

                    oldImagePath = Path.Combine(groupImageFolder, oldGroup.imageName); 
                    FileInfo imageInfo = new FileInfo(oldImagePath);
                    string newImageName = newGroupName.Trim() + imageInfo.Extension;
                    string newImagePath = Path.Combine(groupImageFolder, newImageName);
                    FileManager.MoveFile(oldImagePath, newImagePath);
                    if (selectedImagePathFromDisk.Equals(oldImagePath)) selectedImagePathFromDisk = newImagePath;
                    oldGroup.imageName = newImageName;

                    foreach(var entry in entryList) {
                        if (entry.groupName.Equals(oldGroupName))
                        {
                            entry.groupName = newGroupName;
                        }
                    }
                    JsonFileManager.SaveListToJson(entryList, jsonFilePath);
                }
                oldImagePath = Path.Combine(groupImageFolder, oldGroup.imageName);
                if (!selectedImagePathFromDisk.Equals(oldImagePath))
                {
                    FileInfo imageInfo = new FileInfo(selectedImagePathFromDisk);
                    string newImageName = newGroupName.Trim() + imageInfo.Extension;
                    string newImagePath = Path.Combine(groupImageFolder, newImageName);
                    oldGroup.imageName = newImageName;
                    FileManager.DeleteFile(oldImagePath);
                    FileManager.CopyFile(selectedImagePathFromDisk, newImagePath);

                    Texture2D texture = await FileManager.LoadImageFromDisk(newImagePath);
                    obj.GetComponentInChildren<RawImage>().texture = texture;
                }
                JsonFileManager.SaveListToJson(groupList, groupPath);
                message = "Grupo editado com sucesso!"; 
                Debug.Log(message);
                MostrarResultadoDaOperacaoGrupoAdd(message);
                LimparCamposPainelAdicionarGrupo();
            } catch(Exception ex)
            {
                Debug.LogError(ex.Message);
            } 
        }
    }
    public void HandleSalvarButtonClickPainelAdicionarGrupo()
    {
        string name = groupInputField.text.Trim().ToLower();
        if (!name.Equals("") && selectedImagePathFromDisk != null)
        {
            FileInfo imageInfo = new FileInfo(selectedImagePathFromDisk);
            string newImageName = name.Trim() + imageInfo.Extension;
            string newImagePath = Path.Combine(groupImageFolder, newImageName);
            Group group = new Group(name, newImageName);
            if (groupList == null)
            {
                groupList = new List<Group>();
            }
            if (!groupList.Contains(group))
            {
                AddGroup(group, newImagePath);
                AddGroupToView(group);
                AddGroupToDictionary(group.name);
                LoadDropdown();
                if (groupList.Count == 1) ActivateGroupInTheView();
                Debug.Log("Grupo adicionado com sucesso!");
                MostrarResultadoDaOperacaoGrupoAdd("Grupo adicionado com sucesso!");
            }
        }
    }
    private void AddGroup(Group group, string newImagePath)
    {
        groupList.Add(group);
        JsonFileManager.SaveListToJson<Group>(groupList, groupPath);
        FileManager.CopyFile(selectedImagePathFromDisk, newImagePath);
        LimparCamposPainelAdicionarGrupo();
        mensagemNenhumGrupoCriadoTxtMeshPro.gameObject.SetActive(false);
    }
    private void LimparCamposPainelAdicionarGrupo()
    {
        this.selectedImagePathFromDisk = null;
        groupInputField.text = "";
        nomeImagemPainelAdicionarGrupo.text = "";
        nomeImagemPainelAdicionarPalavra.text = ""; 
    }

    private void RemoveGroup(string groupName)
    {
        Group group = groupList.Find(group => group.name == groupName);
        if (group != null)
        {
            groupList.Remove(group);
            FileManager.DeleteFile(Path.Combine(groupImageFolder, group.imageName));            
            JsonFileManager.SaveListToJson<Group>(groupList, groupPath);
            RemoveGroupFromView(groupName);
            groupNameToPalavraViewContent.Remove(groupName);
            RemoveAllEntryByGroupName(groupName);
            FileManager.DeleteDirectory(Path.Combine(audioFolder, groupName));
            FileManager.DeleteDirectory(Path.Combine(wordImageFolder, groupName));
            LoadDropdown();
            if (groupList.Count > 0)
            {
                activeGroup = groupList.First().name;
            } else
            {
                mensagemNenhumGrupoCriadoTxtMeshPro.gameObject.SetActive(true);
            }
            Debug.Log("Grupo removido com sucesso!");
            MostrarResultadoDaOperacaoGrupoDel("Grupo removido com sucesso!");
        }
    }

    // Relacionado ao painel de advertência

    public void HandleOkButtonClickPaineAdvertencia()
    {
        painelAdvertencia.gameObject.SetActive(false);
    }

    // Métodos usados para popular a interface
    public override void PopulateView()
    {
        if (groupList != null && groupList.Count > 0 && entryList != null && entryList.Count > 0)
        {
            for (int i = 0; i < entryList.Count; i++)
            {
                PalavraEntry entry = entryList[i];
                AddGameObjectToView(entry, groupNameToPalavraViewContent[entry.groupName]);
            }
            mensagemNenhumItemAddTxtMeshPro.gameObject.SetActive(
                groupNameToPalavraViewContent[groupList.First().name].transform.childCount == 0);
        }
    }
    private async void AddGameObjectToView(PalavraEntry entry, GameObject contentGroup)
    {
        GameObject obj = Instantiate(itemPrefab, contentGroup.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = entry.name;
        obj.name = entry.name;
        Texture2D texture = await FileManager.LoadImageFromDisk(Path.Combine(wordImageFolder, entry.groupName, entry.imageName));
        obj.GetComponentInChildren<RawImage>().texture = texture;
    }

    private void PopulateViewGroup()
    {
        if (groupList != null && groupList.Count > 0)
        {
            for (int i = 0; i < groupList.Count; i++)
            {
                // Se o grupo não estiver na view
                if (groupViewContent.transform.Find(groupList[i].name) == null)
                {
                    AddGroupToView(groupList[i]);
                }
            }
            Debug.Log("View group populada com sucesso!");
        } else
        {
            Debug.Log("View group não populada pois groupList é nulo ou está vazio!");
        }
    }
    
    private async void AddGroupToView(Group group)
    {
        GameObject obj = Instantiate(groupItemPrefab, groupViewContent.transform, false);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = group.name;
        obj.name = group.name;
        Texture2D texture = await FileManager.LoadImageFromDisk(Path.Combine(groupImageFolder, group.imageName));
        obj.GetComponentInChildren<RawImage>().texture = texture;
    }

    private void RemoveGroupFromView(string groupName)
    {
        GameObject contentGroup = groupNameToPalavraViewContent[groupName];
        Destroy(contentGroup);
        Transform groupItemTransform = groupViewContent.transform.Find(groupName);
        Destroy(groupItemTransform.gameObject);
    }

    // Métodos relacionados a atualização dos componentes

    // Método usado para carregar o dropdownPainelAdicionarPalavra e ...DeletarGrupo com os grupos disponíveis
    private void LoadDropdown()
    {
        if (groupList != null && groupList.Count > 0)
        {
            dropdownPainelAdicionarPalavra.ClearOptions();
            dropdownPainelDeletarGrupo.ClearOptions();
            foreach(var group in this.groupList) {
                var optionData = new TMP_Dropdown.OptionData();
                optionData.text = group.name;
                dropdownPainelAdicionarPalavra.options.Add(optionData);
                dropdownPainelDeletarGrupo.options.Add(optionData);
            }
            dropdownPainelAdicionarPalavra.value = 0;
            dropdownPainelDeletarGrupo.value = 0;
            Debug.Log("Dropdown carregado com sucesso!");
        } else
        {
            Debug.Log("Dropdown não carregado pois não existe grupo criado!");
        }
    }
    private void SetDropdownValue(TMP_Dropdown dropdown, string groupName)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text.Equals(groupName))
            {
                dropdown.value = i;
                break;
            }
        }
    }
    private void ActivateGroupInTheView()
    {
        if (groupList != null && groupList.Count > 0)
        {
            string groupName = groupList.First().name;
            groupNameToPalavraViewContent[groupName].gameObject.SetActive(true);
            this.activeGroup = groupName;
            Transform childTransform = this.groupViewContent.transform.Find(groupName);
            childTransform.GetComponent<Image>().color = groupSelectedColor;
            palavrasViewport.transform.parent
                .GetComponent<ScrollRect>().content = groupNameToPalavraViewContent[groupName].GetComponent<RectTransform>();
            mensagemNenhumItemAddTxtMeshPro.gameObject.SetActive(groupNameToPalavraViewContent[groupName].transform.childCount == 0);
            viewContent = groupNameToPalavraViewContent[groupName];
            Debug.Log("Scrollview atualizada com sucesso!");
        } else
        {
            Debug.Log("Scrollview não atualizada pois não existe grupo criado!");
        }
    }
}
