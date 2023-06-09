using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class VogalControllerScript : BaseControllerScript
{
    public VogalControllerScript () : base("/Data/Vogais.txt", "/Data/Audio/Vogais/")
    {
    }
    protected override void Awake()
    {
        VogalItemScript.OnPlayAudioButtonClick += ItemScript_OnPlayAudioButtonClick;
        VogalItemScript.OnDeleteButtonClick += ItemScript_OnDeleteButtonClick;
        base.Awake();
    }
    void OnDestroy()
    {
        VogalItemScript.OnPlayAudioButtonClick -= ItemScript_OnPlayAudioButtonClick;
        VogalItemScript.OnDeleteButtonClick -= ItemScript_OnDeleteButtonClick;   
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
