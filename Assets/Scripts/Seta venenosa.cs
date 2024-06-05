using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setavenenosa : MonoBehaviour
{
    public int cantidadVeneno;
    public int indiceDeterioro;
    private List<IDeterioro> ListaParaDeteriorar = new List<IDeterioro>();

    private void Start()
    {
        StartCoroutine(ManejarDeterioro());
    }

    IEnumerator ManejarDeterioro()
    {
        while (true)
        {
            for (int i = 0; i < ListaParaDeteriorar.Count; i++)
            {
                ListaParaDeteriorar[i].ProducirDeterioro(cantidadVeneno);
            }
            yield return new WaitForSeconds(indiceDeterioro);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<IDeterioro>() != null)
        {
            ListaParaDeteriorar.Add(collision.gameObject.GetComponent<IDeterioro>());
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<IDeterioro>() != null)
            ListaParaDeteriorar.Remove(collision.gameObject.GetComponent<IDeterioro>());
    }
}
