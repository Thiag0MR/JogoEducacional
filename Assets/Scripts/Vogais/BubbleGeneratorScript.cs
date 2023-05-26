using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGeneratorScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bubbleGroup;

    [SerializeField]
    private GameObject bubblePrefab;

    private GameObject[] bubbles;

    [SerializeField]
    private int numberOfBubbles;
    private GameObject bubble;

    [SerializeField]
    private GameObject[] lettersPrefab;
    private GameObject[] letters;
    private Vector2 screenBounds;

    [SerializeField]
    private float width;

    [SerializeField]
    private float height;

    [SerializeField]
    private float startTileY;

    void Awake()
    {
        GameManagerScript.OnGameStateChange += GameManagerScript_OnGameStateChange;
    }

    void OnDestroy()
    {
        GameManagerScript.OnGameStateChange -= GameManagerScript_OnGameStateChange;
    }

    private void GameManagerScript_OnGameStateChange(GameManagerScript.GameState state)
    {
        if (state == GameManagerScript.GameState.Play)
        {
            GenerateBubbles();
        }
    }

    private void GenerateBubbles() {   
        bubbles = new GameObject[numberOfBubbles];
        letters = new GameObject[numberOfBubbles];
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        float paddingX = bubblePrefab.GetComponent<Renderer>().bounds.size.x / 2;
        float paddingY = bubblePrefab.GetComponent<Renderer>().bounds.size.y / 2;

        // float screenBoundXLeft = paddingX + -screenBounds.x;
        // float screenBoundXRight = -paddingX + screenBounds.x;
        // float screenBoundYUp = -paddingY + screenBounds.y;
        // float screenBoundYDown = paddingY + -screenBounds.y;

        float screenBoundXLeft = -screenBounds.x;
        float screenBoundXRight = screenBounds.x;
        float screenBoundYUp = screenBounds.y;
        float screenBoundYDown = -screenBounds.y;

        float tileWidth = (screenBounds.x * 2) / width;
        float tileHeight = (screenBounds.y * 2) / height;

        float boundXLeft = screenBoundXLeft;
        float boundXRight = screenBoundXLeft + tileWidth;
        float boundYUp = screenBoundYDown + tileHeight;
        float boundYDown = screenBoundYDown;

        boundYUp += startTileY * tileHeight;
        boundYDown += startTileY * tileHeight;

        // Debug.Log(boundXLeft);
        // Debug.Log(boundXRight);
        // Debug.Log(boundYDown);
        // Debug.Log(boundYUp);

        // Shuffle(lettersPrefab);
        Shuffle(letters);

        for (int i = 0; i < bubbles.Length; i++) {
            bubbles[i] = Instantiate(bubblePrefab, bubbleGroup.transform) as GameObject;
            letters[i] = Instantiate(lettersPrefab[i], bubbles[i].transform) as GameObject;
            // bubbles[i].transform.position = new Vector2(Random.Range(boundXLeft + paddingX, boundXRight - paddingX), 
                // Random.Range(boundYDown + paddingY, boundYUp - paddingY));
            bubbles[i].transform.position = new Vector2((boundXLeft + boundXRight)/2, (boundYDown +  boundYUp)/2);

            boundXLeft = boundXRight;
            boundXRight += tileWidth;

            if (boundXRight > screenBoundXRight + 0.1) {
                boundXLeft = screenBoundXLeft;
                boundXRight = screenBoundXLeft + tileWidth;

                boundYDown = boundYUp;
                boundYUp += tileHeight;
                
                if (boundYUp > screenBoundYUp + 0.1) {
                    boundYDown = screenBoundYDown;
                    boundYUp = screenBoundYDown + tileHeight;
                }
            }
        }
    }
    private void Shuffle (GameObject[] array) {
        for (int i = 0; i < array.Length; i++) {
            int randomIndex = Random.Range(0, array.Length);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        } 
    }

}
