using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Palavras
{
    public class GroupItemScript : MonoBehaviour
    {
        public static event Action<string> OnSelecionarButtonClick;

        public void HandleSelecionarButtonClick()
        {
            OnSelecionarButtonClick?.Invoke(gameObject.transform.Find("Title").GetComponent<TextMeshProUGUI>().text);
        }
    }
}
