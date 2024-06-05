using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreadorObjetos : MonoBehaviour
{
    public GameObject itemSpawn;
    public int periodoSpawn;
    private GameObject actualSpawn;
    private bool isSpawningObject = false;
    // Start is called before the first frame update
    void Start()
    {
        actualSpawn = Instantiate(itemSpawn, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawningObject & actualSpawn == null)
        {
            StartCoroutine(sacarObjeto());
        }
    }

    IEnumerator sacarObjeto() {
    yield return new WaitForSeconds(periodoSpawn);
        isSpawningObject = true;
        actualSpawn = Instantiate(itemSpawn, transform.position, Quaternion.identity);
        isSpawningObject = false;
    }
}
