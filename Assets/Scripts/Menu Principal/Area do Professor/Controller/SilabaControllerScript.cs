using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class SilabaControllerScript : BaseControllerScript
{
    public SilabaControllerScript () : base("/Data/Silabas.txt", "/Data/Audio/Silabas/")
    {
    }
    protected override void Awake()
    {
        SilabaItemScript.OnPlayAudioButtonClick += ItemScript_OnPlayAudioButtonClick;
        SilabaItemScript.OnDeleteButtonClick += ItemScript_OnDeleteButtonClick;
        base.Awake();
    }
    void OnDestroy()
    {
        SilabaItemScript.OnPlayAudioButtonClick -= ItemScript_OnPlayAudioButtonClick;
        SilabaItemScript.OnDeleteButtonClick -= ItemScript_OnDeleteButtonClick;   
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
