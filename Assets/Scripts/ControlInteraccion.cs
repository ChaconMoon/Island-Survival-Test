
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ControlInteraccion : MonoBehaviour
{
    public float indiceTiempoChequeo = 0.05f;
    private float ultimoTiempoChequeo;
    public float maxDistanciaChequeo;
    public LayerMask capaRayMasc;

    private GameObject actualGameObjectInteractuable;
    private IInteractuable actualInteractuable;

    public TextMeshProUGUI mensajeTexto;
    private Camera camara;



    private void Start()
    {
        camara = Camera.main;

    }

    private void Update()
    {
        if (Time.time - ultimoTiempoChequeo > indiceTiempoChequeo)
        {
            ultimoTiempoChequeo = Time.time;
            Ray rayo = camara.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit lanzaRayo;


            if (Physics.Raycast(rayo, out lanzaRayo, maxDistanciaChequeo, capaRayMasc))
            {

                if (lanzaRayo.collider.gameObject != actualGameObjectInteractuable)
                {
                    actualGameObjectInteractuable = lanzaRayo.collider.gameObject;
                    actualInteractuable = lanzaRayo.collider.GetComponent<IInteractuable>();
                    establecerMensajeTexto();
                }
            }
            else
            {
                actualGameObjectInteractuable = null;
                actualInteractuable = null;
                mensajeTexto.gameObject.SetActive(false);

            }
        }

    }
    void establecerMensajeTexto()
    {

        mensajeTexto.gameObject.SetActive(true);

        mensajeTexto.text = string.Format("<b>[E]</b> {0}", actualInteractuable.obtenerMensajeInteractuable());
    }

    public void OnEntradaInteractiva(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && actualInteractuable != null)
        {
            actualInteractuable.OnInteracturar();
            actualGameObjectInteractuable = null;
            actualInteractuable = null;
            mensajeTexto.gameObject.SetActive(false);


        }
    }
}


public interface IInteractuable
{
    string obtenerMensajeInteractuable();
    void OnInteracturar();
}