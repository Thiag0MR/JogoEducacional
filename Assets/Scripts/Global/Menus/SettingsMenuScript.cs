using Consoantes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public static bool BUBBLE_CREATION_SOUND;
    public static float BACKGROUND_MUSIC_VOLUME;

    [SerializeField]
    private Toggle bubbleCreationSoundToogle;

    [SerializeField]
    private Slider backgroundMusicVolumeSlider;

    [SerializeField]
    private AudioSource audioSourceBackgroundMusic;

    [SerializeField] GameObject gameManager;
    private IGameManager gameManagerScript;

    void Awake()
    {
        gameManagerScript = gameManager.GetComponent<IGameManager>();
        bubbleCreationSoundToogle.onValueChanged.AddListener(v => HandleToogleOnValueChange(v));
        backgroundMusicVolumeSlider.onValueChanged.AddListener(v => HandleSliderOnChangeValue(v));
    }

    // Método chamado no GameManagerScript pois o mesmo inicia desativado
    public void LoadSettings()
    {
        // Ao obter a chave se o valor não existir retorna o valor 1 (default)
        BUBBLE_CREATION_SOUND = PlayerPrefs.GetInt("bubbleCreationSound", 1) > 0 ? true : false;
        BACKGROUND_MUSIC_VOLUME = PlayerPrefs.GetFloat("backgroundMusicVolume", 0.1f);

        bubbleCreationSoundToogle.isOn = BUBBLE_CREATION_SOUND;
        backgroundMusicVolumeSlider.value = BACKGROUND_MUSIC_VOLUME;
        audioSourceBackgroundMusic.volume = BACKGROUND_MUSIC_VOLUME;

        Debug.Log("Opções carregadas com sucesso!");
    }

    public void HandleSaveButtonClick()
    {
        PlayerPrefs.SetInt("bubbleCreationSound", BUBBLE_CREATION_SOUND ? 1 : 0);
        PlayerPrefs.SetFloat("backgroundMusicVolume", BACKGROUND_MUSIC_VOLUME);
        PlayerPrefs.Save();
        gameManagerScript.UpdateGameState(GameState.Settings);
        Debug.Log("Opções salvas com sucesso!");
    }
    public void HandleCloseButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Settings);
    }

    private void HandleToogleOnValueChange(bool value)
    {
        BUBBLE_CREATION_SOUND = value;
    }

    private void HandleSliderOnChangeValue(float value)
    {
        Debug.Log("Slider changed!");
        BACKGROUND_MUSIC_VOLUME = value;
        audioSourceBackgroundMusic.volume = BACKGROUND_MUSIC_VOLUME;
    }
}
