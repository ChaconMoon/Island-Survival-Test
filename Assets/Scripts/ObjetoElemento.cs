using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoElemento : MonoBehaviour, IInteractuable
{
    public DatoElemento elemento;
    public string obtenerMensajeInteractuable()
    {

        return string.Format("Obtener {0}", elemento.nombre);
    }
    public void OnInteracturar()
    {
        if (elemento.resistencia == EfectoResistencia.CalorExtremo || elemento.resistencia == EfectoResistencia.FrioExtremo)
        {
            FindObjectOfType<ControlJugador>().QuitarTemperaturaExtrema();
        }
        ControlInventario.instancia.AnadirElemento(elemento);
        Destroy(gameObject);
    }

}
