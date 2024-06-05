using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ControlInventario : MonoBehaviour
{
    public ElementoEstanteriaUI[] uiElementoEstanteria; //Parte gráfica
    public ElementoEstanteria[] elementoEstanteria; //Parte de información

    public GameObject ventanaInventario;
    public Transform posicionSolar;

    [Header("Elemento Seleccionado")]
    private ElementoEstanteria elementoSeleccionado;
    private int indiceElementoSeleccionado;
    public TextMeshProUGUI nombreElementoSeleccionado;
    public TextMeshProUGUI descripcionElementoSeleccionado;
    //public TextMeshProUGUI nombreNecesidadElementoSeleccionado;
    // public TextMeshProUGUI valoresNecesidadElementoSeleccionado;
    public int cantidadElemento;
    public GameObject botonUsar;
    public GameObject botonSoltar;

    private ControlJugador controlJugador;
    private ControlIndicadores controlIndicadores;

    [Header("Eventos")]
    public UnityEvent onAbrirInventario;
    public UnityEvent onCerrarInventario;

    //Singleton
    public static ControlInventario instancia;



    private void Awake()
    {
        instancia = this;
        controlJugador = GetComponent<ControlJugador>();
        controlIndicadores = GetComponent<ControlIndicadores>();
    }

    private void Start()
    {
        ventanaInventario.SetActive(false);
        elementoEstanteria = new ElementoEstanteria[uiElementoEstanteria.Length];

        for (int x = 0; x < elementoEstanteria.Length; x++)
        {
            elementoEstanteria[x] = new ElementoEstanteria();
            uiElementoEstanteria[x].indice = x;
            uiElementoEstanteria[x].Limpiar();
        }

        LimpiarVentanaElementoSeleccionado();
    }

    public void OnBotonInventario(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            AbrirCerrarVentanaInventario();
        }
    }

    public void AbrirCerrarVentanaInventario()
    {

        if (ventanaInventario.activeInHierarchy)
        {
            ventanaInventario.SetActive(false);
            onCerrarInventario.Invoke();
            controlJugador.PonerQuitarPunteroRaton(false);
        }
        else
        {
            ventanaInventario.SetActive(true);
            onAbrirInventario.Invoke();
            LimpiarVentanaElementoSeleccionado();
            controlJugador.PonerQuitarPunteroRaton(true);
        }


    }

    public bool EstaAbierto()
    {
        return ventanaInventario.activeInHierarchy;
    }

    public void AnadirElemento(DatoElemento elemento)
    {
        ElementoEstanteria elementoParaAlmacenar = ObtenerElementoAlmacenado(elemento);

        if (elementoParaAlmacenar != null)
        {

            elementoParaAlmacenar.cantidad++;
            ActualizarUI();
            return;
        }

        ElementoEstanteria elementoVacio = ObtenerEspacioVacio();
        if (elementoVacio != null)
        {

            elementoVacio.elemento = elemento;
            elementoVacio.cantidad = 1;
            ActualizarUI();
            return;
        }

        SoltarElemento(elemento);
    }



    void SoltarElemento(DatoElemento elemento)
    {
        Instantiate(elemento.PrefabSoltar, posicionSolar.position, Quaternion.Euler
                    (Vector3.one * Random.value * 360.0f)).GetComponent<Rigidbody>().isKinematic = false;
    }

    void ActualizarUI()
    {

        for (int x = 0; x < elementoEstanteria.Length; x++)
        {

            if (elementoEstanteria[x].elemento != null)
                uiElementoEstanteria[x].Establecer(elementoEstanteria[x]);
            else
                uiElementoEstanteria[x].Limpiar();
        }

    }

    ElementoEstanteria ObtenerElementoAlmacenado(DatoElemento elemento)
    {

        for (int x = 0; x < elementoEstanteria.Length; x++)
        {
            if (elementoEstanteria[x].elemento == elemento)
                return elementoEstanteria[x];

        }
        return null;
    }
    ElementoEstanteria ObtenerEspacioVacio()
    {

        for (int x = 0; x < elementoEstanteria.Length; x++)
        {
            if (elementoEstanteria[x].elemento == null)
                return elementoEstanteria[x];
        }

        return null;
    }

    public void ElementoSeleccionado(int indice)
    {

        if (elementoEstanteria[indice].elemento == null)
            return;
        elementoSeleccionado = elementoEstanteria[indice];
        indiceElementoSeleccionado = indice;

        nombreElementoSeleccionado.text = elementoSeleccionado.elemento.nombre;
        descripcionElementoSeleccionado.text = elementoSeleccionado.elemento.descripcion;

        botonUsar.SetActive(true);
        botonSoltar.SetActive(true);



    }
    void LimpiarVentanaElementoSeleccionado()
    {
        elementoSeleccionado = null;
        nombreElementoSeleccionado.text = string.Empty;
        descripcionElementoSeleccionado.text = string.Empty;

        botonUsar.SetActive(false);
        botonSoltar.SetActive(false);
    }

    public void OnBotonUsar()
    {
        Debug.Log(elementoSeleccionado.elemento.tipo.ToString());
        switch (elementoSeleccionado.elemento.tipo)
        {
            case TipoUsoElemento.Salud:
                controlIndicadores.Recuperar(elementoSeleccionado.elemento.valorElemento);

                break;
            case TipoUsoElemento.Hambre:
                controlIndicadores.Comer(elementoSeleccionado.elemento.valorElemento);

                break;
            case TipoUsoElemento.Sed:
                controlIndicadores.Beber(elementoSeleccionado.elemento.valorElemento);

                break;
            case TipoUsoElemento.Descanso:
                controlIndicadores.Descansar(elementoSeleccionado.elemento.valorElemento);

                break;
        }
        if (!(elementoSeleccionado.elemento.resistencia == EfectoResistencia.Frio 
            && controlJugador.resistenciaActual == EfectoResistencia.FrioExtremo) 
            && elementoSeleccionado.elemento.resistencia != EfectoResistencia.Ninguna)
        {
            controlJugador.TemporizadorResistencia(elementoSeleccionado.elemento.resistencia);
            controlJugador.resistenciaActual = elementoSeleccionado.elemento.resistencia;
            controlJugador.solicitarActualizacionBioma = true;
        }
        else if(!(elementoSeleccionado.elemento.resistencia == EfectoResistencia.Calor 
            && controlJugador.resistenciaActual != EfectoResistencia.CalorExtremo) 
            && elementoSeleccionado.elemento.resistencia != EfectoResistencia.Ninguna)
        {
            controlJugador.TemporizadorResistencia(elementoSeleccionado.elemento.resistencia);
            controlJugador.resistenciaActual = elementoSeleccionado.elemento.resistencia;
            controlJugador.solicitarActualizacionBioma = true;
        }

        EliminarElementoSeleccionado();
    }

    public void OnBotonSoltar()
    {
        SoltarElemento(elementoSeleccionado.elemento);
        EliminarElementoSeleccionado();
    }
    void EliminarElementoSeleccionado()
    {
        elementoSeleccionado.cantidad--;
        if (elementoSeleccionado.cantidad == 0)
        {
            elementoSeleccionado.elemento = null;
            LimpiarVentanaElementoSeleccionado();
        }
        ActualizarUI();

    }

}


public class ElementoEstanteria
{
    public DatoElemento elemento;
    public int cantidad;
}
