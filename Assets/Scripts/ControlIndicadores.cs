using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class Indicadores
{
    [HideInInspector]
    public float valorActual;
    public float valorMaximo;
    public float valorMinimo;
    public float valorInicial;
    public float indiceRecuperación;
    public float indiceDeterioro;
    public Scrollbar barraUI;
    public Scrollbar barraUI2;

    public void Sumar(float cantidad)
    {
        valorActual = Mathf.Min(valorActual + cantidad, valorMaximo);
    }
    public void Restar(float cantidad)
    {
        valorActual = Mathf.Max(valorActual - cantidad, 0.0f);
    }
    public float ObtenerPorcentaje()
    {
        return valorActual / valorMaximo;

    }
}
    public class ControlIndicadores : MonoBehaviour, IDeterioro {
    public void Recuperar(float cantidad)
    {
        salud.Sumar(cantidad);
    }
    public void Comer(float cantidad)
    {
        Debug.Log("Hambre: " + hambre.valorActual);
        hambre.Sumar(cantidad);
        Debug.Log("Hambre: " + hambre.valorActual);
    }
    public void Beber(float cantidad)
    {
        sed.Sumar(cantidad);
    }
    public void Descansar(float cantidad)
    {
        cansancio.Restar(cantidad);
    }
    public void ProducirDeterioro(int cantidad)
    {
        salud.Restar(cantidad);
        onSufrirDeterioro?.Invoke();
    }

    private void OnGUI()
    {
        //GUILayout.TextArea("Salud: " + salud.valorActual);
        //GUILayout.TextArea("Salud: " + hambre.valorActual);
    }
    public Indicadores salud;
    public Indicadores hambre;
    public Indicadores sed;
    public Indicadores cansancio;

    public float reduccionSaludHambre;
    public float reduccionSaludconSed;
    public UnityEvent onSufrirDeterioro;

    // Start is called before the first frame update
    void Start()
    {
        salud.valorActual = salud.valorInicial;
        hambre.valorActual = hambre.valorInicial;
        sed.valorActual = sed.valorInicial;
        cansancio.valorActual = cansancio.valorInicial;

    }

    // Update is called once per frame
    void Update()
    {
        salud.Restar(salud.indiceDeterioro * Time.deltaTime);
        hambre.Restar(hambre.indiceDeterioro * Time.deltaTime);
        sed.Restar(sed.indiceDeterioro * Time.deltaTime);
        cansancio.Sumar(cansancio.indiceRecuperación * Time.deltaTime);

        if (hambre.valorActual == 0)
        {
            salud.Restar(reduccionSaludHambre * Time.deltaTime);

        }
        if (sed.valorActual == 0)
        {
            sed.Restar(reduccionSaludconSed * Time.deltaTime);
        }
        if (salud.valorActual == 0.0f)
        {
            Morir();
        }
        if (hambre.valorActual > 80f && salud.valorActual != 100f)
        {
            hambre.Restar(hambre.indiceDeterioro*5 * Time.deltaTime);
            salud.Sumar(salud.indiceRecuperación*Time.deltaTime);
        }

        salud.barraUI.size = salud.ObtenerPorcentaje();
        hambre.barraUI.size = hambre.ObtenerPorcentaje();
        sed.barraUI.size = sed.ObtenerPorcentaje();
        cansancio.barraUI.size = cansancio.ObtenerPorcentaje();

        salud.barraUI2.size = salud.ObtenerPorcentaje();
        hambre.barraUI2.size = hambre.ObtenerPorcentaje();
        sed.barraUI2.size = sed.ObtenerPorcentaje();
        cansancio.barraUI2.size = cansancio.ObtenerPorcentaje();

    }

    public void Morir()
    {
        Debug.Log("El jugador a muerto");
    }
}
public interface IDeterioro
{
    void ProducirDeterioro(int cantidadDeterioro);
}
