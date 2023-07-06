using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vogais
{
    public class FishGeneratorScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] leftOriginFishPrefab;

        [SerializeField]
        private GameObject[] rightOriginFishPrefab;

        [SerializeField]
        private GameObject fishColliderLeft;

        [SerializeField]
        private GameObject fishColliderRight;

        [SerializeField]
        private GameObject fishGroup;

        [SerializeField]
        private float[] fishSpeed;

        private Vector2 screenBounds;
        private float fishBornPositionHorizontalLeft, fishBornPositionHorizontalRight, yTop, yBottom;

        [SerializeField]
        private int maxPoolSize, spawnTime;
        HashSet<GameObject> leftInactiveFish = new HashSet<GameObject>();
        HashSet<GameObject> rightInactiveFish = new HashSet<GameObject>();

        private int createdInstances = 0;

        private const int FISH_LAYER = 6;

        void Start()
        {
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            fishBornPositionHorizontalLeft = -screenBounds.x - 1;
            fishBornPositionHorizontalRight = screenBounds.x + 1;
            yTop = -screenBounds.y + 1;
            yBottom = screenBounds.y - 1;
            //fishColliderLeft.transform.position = new Vector2(fishBornPositionHorizontalLeft - 2, 0f);
            fishColliderLeft.transform.localScale = new Vector2(1f, screenBounds.y * 2);
            //fishColliderRight.transform.position = new Vector2(fishBornPositionHorizontalRight + 2, 0f);
            fishColliderRight.transform.localScale = new Vector2(1f, screenBounds.y * 2);

            FishScript.OnFishCollide += FishScript_OnFishCollide;

            InvokeRepeating(nameof(FishGenerator), 1, spawnTime);
        }

        void OnDestroy()
        {
            FishScript.OnFishCollide -= FishScript_OnFishCollide;
        }

        void Update()
        {
            fishBornPositionHorizontalLeft = Camera.main.transform.position.x + -screenBounds.x - 1;
            fishBornPositionHorizontalRight = Camera.main.transform.position.x + screenBounds.x + 1;
        }

        private void FishGenerator()
        {
            if (Random.Range(0, 100) > 50)
            {
                GenerateLeftFish();
            }
            else
            {
                GenerateRightFish();
            }
        }

        private void GenerateLeftFish()
        {
            GameObject leftFish;
            if (createdInstances < maxPoolSize) // Cria um novo peixe
            {
                int randomIndexR = Random.Range(0, leftOriginFishPrefab.Length);
                leftFish = Instantiate(leftOriginFishPrefab[randomIndexR], fishGroup.transform);
                leftFish.layer = FISH_LAYER;
                createdInstances++;
                leftFish.transform.position = new Vector2(fishBornPositionHorizontalLeft, Random.Range(yTop, yBottom));
                leftFish.GetComponent<Rigidbody2D>().AddForce(Vector2.right * fishSpeed[Random.Range(0, fishSpeed.Length)], ForceMode2D.Force);
            }
            else if (leftInactiveFish.Count > 0) // Pega um peixe já criado que está inativo
            {
                int randomIndex = Random.Range(0, leftInactiveFish.Count);
                leftFish = leftInactiveFish.ElementAt(randomIndex);
                leftInactiveFish.Remove(leftFish);
                leftFish.SetActive(true);
                leftFish.transform.position = new Vector2(fishBornPositionHorizontalLeft, Random.Range(yTop, yBottom));
                leftFish.GetComponent<Rigidbody2D>().AddForce(Vector2.right * fishSpeed[Random.Range(0, fishSpeed.Length)], ForceMode2D.Force);
            }
        }


        private void GenerateRightFish()
        {
            GameObject rightFish;
            if (createdInstances < maxPoolSize) // Cria um novo peixe
            {
                int randomIndexL = Random.Range(0, rightOriginFishPrefab.Length);
                rightFish = Instantiate(rightOriginFishPrefab[randomIndexL], fishGroup.transform);
                rightFish.layer = FISH_LAYER;
                createdInstances++;
                rightFish.transform.position = new Vector2(fishBornPositionHorizontalRight, Random.Range(yTop, yBottom));
                rightFish.GetComponent<Rigidbody2D>().AddForce(Vector2.left * fishSpeed[Random.Range(0, fishSpeed.Length)], ForceMode2D.Force);
            }
            else if (rightInactiveFish.Count > 0) // Pega um peixe já criado que está inativo
            {
                int randomIndex = Random.Range(0, rightInactiveFish.Count);
                rightFish = rightInactiveFish.ElementAt(randomIndex);
                rightInactiveFish.Remove(rightFish);
                rightFish.SetActive(true);
                rightFish.transform.position = new Vector2(fishBornPositionHorizontalRight, Random.Range(yTop, yBottom));
                rightFish.GetComponent<Rigidbody2D>().AddForce(Vector2.left * fishSpeed[Random.Range(0, fishSpeed.Length)], ForceMode2D.Force);
            }
        }

        // Esse método é chamado quando o peixe colidir com o collider
        private void FishScript_OnFishCollide(GameObject obj)
        {
            obj.SetActive(false);
            if (obj.tag == "LeftFish")
            {
                leftInactiveFish.Add(obj);
            }
            else if (obj.tag == "RightFish")
            {
                rightInactiveFish.Add(obj);
            }
        }
    }
}