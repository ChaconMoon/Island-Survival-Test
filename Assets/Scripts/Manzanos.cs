using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manzanos : MonoBehaviour
{
    public GameObject[] manzanas = new GameObject[4];
    public int tiempoCreacionManzana = 5;
    private GameObject actualApple;
    private GameObject spawnedApple;
    private bool isSpawningApple;
    private bool isPoisen;


    private void Start()
    {
        if (1 == Random.Range(1, 11))
        {
            actualApple = manzanas[3];
        }
        else
        {
            actualApple = manzanas[Random.Range(0, manzanas.Length - 1)];
        }

        spawnedApple = Instantiate(actualApple, transform.position, Quaternion.identity);
        spawnedApple.transform.Rotate(-90, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isSpawningApple && spawnedApple == null)
        {
            if (1 == Random.Range(1, 11))
            {
                actualApple = manzanas[3];
            }
            else
            {
                actualApple = manzanas[Random.Range(0, manzanas.Length - 1)];
            }
            StartCoroutine(createApple());
        }
    }

    IEnumerator createApple()
    {
        isSpawningApple = true;
        yield return new WaitForSeconds(tiempoCreacionManzana);
        spawnedApple = Instantiate(actualApple, transform.position,Quaternion.identity);
        spawnedApple.transform.Rotate(-90, 0, 0);
        isSpawningApple = false;
        
    }
}
