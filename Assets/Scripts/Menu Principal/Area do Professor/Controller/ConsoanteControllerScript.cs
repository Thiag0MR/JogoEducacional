using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class ConsoanteControllerScript : BaseControllerScript
{
    public ConsoanteControllerScript () : base("/Data/Consoantes.txt", "/Data/Audio/Consoantes/")
    {
    }
    protected override void Awake()
    {
        ConsoanteItemScript.OnPlayAudioButtonClick += ItemScript_OnPlayAudioButtonClick;
        ConsoanteItemScript.OnDeleteButtonClick += ItemScript_OnDeleteButtonClick;
        base.Awake();
    }
    void OnDestroy()
    {
        ConsoanteItemScript.OnPlayAudioButtonClick -= ItemScript_OnPlayAudioButtonClick;
        ConsoanteItemScript.OnDeleteButtonClick -= ItemScript_OnDeleteButtonClick;   
    }
    private void ItemScript_OnPlayAudioButtonClick(string name)
    {
        base.HandlePlayAudioButtonClick(name);
    }
    private void ItemScript_OnDeleteButtonClick(string name)
    {
        base.HandleDeleteButtonClick(name); 
    }
}
