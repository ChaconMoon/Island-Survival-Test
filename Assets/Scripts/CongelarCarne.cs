using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongelarCarne : MonoBehaviour
{
    public Material materialCarneCongelada;
    public DatoElemento datosCarneCongelada;
    public MeshRenderer[] fasesCarne;
    public ParticleSystem humoCongelado;

    // Start is called before the first frame update
    IEnumerator Congelar()
    {
            yield return new WaitForSeconds(4f);
            GetComponent<ObjetoElemento>().elemento = datosCarneCongelada;
            gameObject.GetComponent<MeshRenderer>().material = materialCarneCongelada;
            humoCongelado.Play();
            GetComponent<AudioSource>().Play();
                foreach (var item in fasesCarne)
                {
                    item.material = materialCarneCongelada;
                }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LimiteZonaGelida") || other.CompareTag("ZonaFrioExtremo"))
        {
            StartCoroutine(Congelar());
        }
    }
}
