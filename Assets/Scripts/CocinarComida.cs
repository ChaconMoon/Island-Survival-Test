using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocinarComida : MonoBehaviour
{
    public Material materialElementoCocinada;
    public DatoElemento datosElementoCocinada;
    public MeshRenderer renderizado;
    public ParticleSystem humo;

    // Start is called before the first frame update
    IEnumerator Cocinar()
    {
            yield return new WaitForSeconds(4f);
            GetComponent<ObjetoElemento>().elemento = datosElementoCocinada;
            renderizado.material = materialElementoCocinada;
            humo.Play();
            GetComponent<AudioSource>().Play();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LimiteZonaVolcanica") || other.gameObject.CompareTag("ZonaCalorExtremo"))
        {
            StartCoroutine(Cocinar());
        }
    }
}
