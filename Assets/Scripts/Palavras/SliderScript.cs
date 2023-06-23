using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private GameObject content, buttonsParent;

    [SerializeField]
    private Sprite selectedButtonImage, unselectedButtonImage;

    private GameObject[] buttons;

    private float scroll_pos;
    private float[] pos;
    private bool runIt;
    private float time;
    private int selectedButtonIndex;

    private void Start()
    {
        scroll_pos = 0;
        runIt = false;
        selectedButtonIndex = 0;
        buttons = new GameObject[buttonsParent.transform.childCount];
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = buttonsParent.transform.GetChild(i).gameObject;
            //buttons[i].GetComponent<Button>().onClick.AddListener(() => OnSelectButtonClick(buttons[i]));
            if (i == 0)
            {
                buttons[i].GetComponent<Image>().sprite = selectedButtonImage;
            }
        }
        if (content.transform.childCount == 1)
        {
            float viewportWidth = content.transform.parent.GetComponent<RectTransform>().rect.width / 2;
            float groupItemWidth = content.transform.GetChild(0).GetComponent<RectTransform>().rect.width *
                content.transform.GetChild(0).transform.localScale.x / 2;
            content.GetComponent<HorizontalLayoutGroup>().padding.left = (int) (viewportWidth - groupItemWidth);
        } 
    }
    private void OnEnable()
    {
        ButtonScript.OnButtonClick += ButtonScript_OnButtonClick;
    }

    private void OnDisable()
    {
        ButtonScript.OnButtonClick -= ButtonScript_OnButtonClick;
    }


    private void Update()
    {
        Slider();
    }


    private void Slider()
    {
        pos = new float[content.transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        if (runIt)
        {
            MoverItem(distance, pos, selectedButtonIndex);
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                content.transform.GetChild(i).localScale = Vector2.Lerp(content.transform.GetChild(i).localScale, 
                    new Vector2(0.9f, 0.9f), 0.1f);
                buttons[i].transform.localScale = Vector2.Lerp(buttons[i].transform.localScale, 
                    new Vector2(1.2f, 1.2f), 0.1f);
                buttons[i].transform.GetComponent<Image>().sprite = selectedButtonImage;
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        buttons[j].transform.GetComponent<Image>().sprite = unselectedButtonImage;
                        buttons[j].transform.localScale = Vector2.Lerp(buttons[j].transform.localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        content.transform.GetChild(j).localScale = Vector2.Lerp(content.transform.GetChild(j).localScale, 
                            new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
    private void MoverItem(float distance, float[] pos, int btnIndex)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnIndex], 1f * Time.deltaTime);
            }
        }
    }

    private void ButtonScript_OnButtonClick(GameObject btn)
    {
        btn.GetComponent<Image>().sprite = selectedButtonImage;
        buttons[selectedButtonIndex].GetComponent<Image>().sprite = unselectedButtonImage;
        selectedButtonIndex = btn.transform.GetSiblingIndex();
        runIt = true;
        time = 0;
        scroll_pos = pos[selectedButtonIndex];
    }

    public void OnRightButtonClick()
    {
        if (selectedButtonIndex < buttons.Length - 1)
        {
            buttons[selectedButtonIndex++].GetComponent<Image>().sprite = unselectedButtonImage;
            buttons[selectedButtonIndex].GetComponent<Image>().sprite = selectedButtonImage;
            runIt = true;
            time = 0;
            scroll_pos = pos[selectedButtonIndex];
        }
    }

    public void OnLeftButtonClick() 
    { 
        if (selectedButtonIndex > 0)
        {
            buttons[selectedButtonIndex--].GetComponent<Image>().sprite = unselectedButtonImage;
            buttons[selectedButtonIndex].GetComponent<Image>().sprite = selectedButtonImage;
            runIt = true;
            time = 0;
            scroll_pos = pos[selectedButtonIndex];
        }
    }

    
}
