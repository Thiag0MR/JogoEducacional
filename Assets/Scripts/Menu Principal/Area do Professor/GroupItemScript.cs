using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroupItemScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nomeGrupo;

    public static event Action<string> OnClick;
    public void HandleClick()
    {
        OnClick?.Invoke(nomeGrupo.text);
    }
}
