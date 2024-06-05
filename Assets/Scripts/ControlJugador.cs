using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    private Vector2 ratonDelta;
    private Vector2 moviniento;
    private Rigidbody rb;
    private float rotacionActualCamara;
    private bool isInFreezeZone;
    private bool isInVulcanZone;
    [Header("Vista Camara")]
    public Transform camara;
    public float maxXVista;
    public float minXVista;
    public float sensibilidadRaton;
    [Header("moviniento")]
    public Vector2 movinientoActualEntrada;
    public float velocidadMoviniento;
    public float fuerzaSalto;
    public LayerMask capaSuelo;
    private bool isJumping;
    public GameObject origenRayos;
    public GameObject iconoControlTemperatura;
    public IndicadorDeterioro controlDeterioro;
    public ControlIndicadores controlIndicadores;
    public ControlInventario controlInventario;
    public EfectoResistencia resistenciaActual;
    public bool solicitarActualizacionBioma = false;
    private float indiceDeterioroGrave;
    private float indiceDeterioroNormal;
    private ControlIconoTemperatura iconoTemperatura;
    private TextMeshProUGUI infoResistencia;
    private TextMeshProUGUI tiempoResistencia;


    public void OnVistaInput(InputAction.CallbackContext context)
    {

        ratonDelta = context.ReadValue<Vector2>();
        iconoTemperatura = iconoControlTemperatura.GetComponent<ControlIconoTemperatura>();
        infoResistencia = iconoControlTemperatura.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        tiempoResistencia = iconoControlTemperatura.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();


    }
    public void OnMovinientoInput(InputAction.CallbackContext context)
    {

        if(context.phase == InputActionPhase.Performed)
        {
            movinientoActualEntrada = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movinientoActualEntrada = Vector2.zero;

        }

    }
    public void OnSaltoInput(InputAction.CallbackContext context)
    {

        isJumping = context.ReadValueAsButton();
        if (context.phase == InputActionPhase.Started)
        {

            if (EstaenSuelo())
            {   
                rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            }
        }
    }
    public void OnExitInput(InputAction.CallbackContext context)
    {
       Application.Quit();
    }

    private bool EstaenSuelo()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray (origenRayos.transform.position + (origenRayos.transform.forward*0.2f) +(Vector3.up*0.01f), Vector3.down),
            new Ray (origenRayos.transform.position + (-origenRayos.transform.forward*0.2f) +(Vector3.up*0.01f), Vector3.down),
            new Ray (origenRayos.transform.position + (origenRayos.transform.right*0.2f) +(Vector3.up*0.01f), Vector3.down),
            new Ray (origenRayos.transform.position + (-origenRayos.transform.right*0.2f) +(Vector3.up*0.01f), Vector3.down)
        };
        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray.origin, ray.direction, 0.5f, capaSuelo)){
                return true;
            } 
        }
        return false;
    }

    private void OnGUI()
    {
        //GUILayout.TextArea(resistenciaActual.ToString());
        //GUILayout.TextArea(solicitarActualizacionBioma.ToString());


    }

    private void LateUpdate()
    {
        if (!controlInventario.EstaAbierto())
        {
            VistaCamara();
        }
    }
    private void FixedUpdate()
    {
        Moviniento();
    }

    private void Moviniento()
    {
        Vector3 direccion = transform.forward * movinientoActualEntrada.y + transform.right * movinientoActualEntrada.x;
        direccion *= velocidadMoviniento;
        direccion.y = rb.linearVelocity.y;
        rb.linearVelocity = direccion;
    }

    private void VistaCamara()
    {
        rotacionActualCamara += -ratonDelta.y*sensibilidadRaton;
        rotacionActualCamara = Mathf.Clamp(rotacionActualCamara, minXVista, maxXVista);
        camara.localEulerAngles = new Vector3(rotacionActualCamara,0,0);
        transform.eulerAngles += new Vector3(0, ratonDelta.x * sensibilidadRaton, 0);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        indiceDeterioroGrave = controlIndicadores.salud.indiceDeterioro * 5f;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        indiceDeterioroNormal = controlIndicadores.salud.indiceDeterioro;
        TemporizadorResistencia(EfectoResistencia.Ninguna);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Ray(origenRayos.transform.position + (origenRayos.transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down));
        Gizmos.DrawRay(new Ray(origenRayos.transform.position + (-origenRayos.transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down));
        Gizmos.DrawRay(new Ray(origenRayos.transform.position + (origenRayos.transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down));
        Gizmos.DrawRay(new Ray(origenRayos.transform.position + (-origenRayos.transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down));
    }

    public void PonerQuitarPunteroRaton(bool quitar)
    {
        Cursor.lockState = quitar ? CursorLockMode.None : CursorLockMode.Locked ;
        
    }

    //Controla lo que pasa cuando entras de un bioma
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("LimiteZonaGelida")) {
            if (resistenciaActual != EfectoResistencia.Frio && resistenciaActual != EfectoResistencia.FrioExtremo)
            {
                Debug.Log("EntrarEnzona de hielo sin protecci�n");
                isInFreezeZone = true;
                isInVulcanZone = false;
                iconoTemperatura.PonerTemperaturaFrio();
                controlDeterioro.AparecerCambioBioma("G");
                controlIndicadores.salud.indiceDeterioro = indiceDeterioroGrave;


            }
            else
            {
                isInFreezeZone = false;
                iconoTemperatura.PonerTemperaturaTempladoFrio();
                controlDeterioro.QuitarCambioBioma();
                controlIndicadores.salud.indiceDeterioro = indiceDeterioroNormal;

            } 
        }
        if (other.gameObject.CompareTag("LimiteZonaVolcanica"))
        {
            if (resistenciaActual != EfectoResistencia.Calor && resistenciaActual != EfectoResistencia.CalorExtremo)
            {
                Debug.Log("EntrarEnzona de fuego sin protecci�n");
                isInVulcanZone = true;
                isInFreezeZone = false;
                iconoTemperatura.PonerTemperaturaCalor();
                controlDeterioro.AparecerCambioBioma("V");
                controlIndicadores.salud.indiceDeterioro = indiceDeterioroGrave;
            }
            else
            {
                isInVulcanZone = false;
                iconoTemperatura.PonerTemperaturaTempladoCalor();
                controlDeterioro.QuitarCambioBioma();
                controlIndicadores.salud.indiceDeterioro = indiceDeterioroNormal;
            }
        }
        if (other.gameObject.CompareTag("ZonaFrioExtremo"))
        {
            if (resistenciaActual != EfectoResistencia.FrioExtremo)
            {
                Debug.Log("Cerca de hongo gelido");
                isInFreezeZone = true;
                iconoTemperatura.PonerTemperaturaFrio();
                controlDeterioro.AparecerCambioBioma("G");
                controlIndicadores.salud.indiceDeterioro = indiceDeterioroGrave * 2;
            }
            else
            {
                iconoTemperatura.PonerTemperaturaTempladoFrioExtremo();
            }
        }
        if (other.gameObject.CompareTag("ZonaCalorExtremo"))
        {
            if (resistenciaActual != EfectoResistencia.CalorExtremo)
            {
                Debug.Log("Cerca de hongo de calor");
                isInVulcanZone = true;
                iconoTemperatura.PonerTemperaturaCalor();
                controlDeterioro.AparecerCambioBioma("V");
                controlIndicadores.salud.indiceDeterioro = indiceDeterioroGrave * 2;
            }
            else
            {
                iconoTemperatura.PonerTemperaturaTempladoCalorExtremo(); 
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LimiteZonaVolcanica") || other.gameObject.CompareTag("LimiteZonaGelida") || other.gameObject.CompareTag("ZonaFrioExtremo") || other.gameObject.CompareTag("ZonaCalorExtremo"))
        {
            isInVulcanZone = false;
            isInFreezeZone = false;
            iconoTemperatura.PonerTemperaturaNormal();
            controlDeterioro.QuitarCambioBioma();
            controlIndicadores.salud.indiceDeterioro = indiceDeterioroNormal;
        }
    }

    public void QuitarTemperaturaExtrema()
    {
        isInVulcanZone = false;
        isInFreezeZone = false;
        iconoTemperatura.PonerTemperaturaNormal();
        controlDeterioro.QuitarCambioBioma();
        controlIndicadores.salud.indiceDeterioro = indiceDeterioroNormal;
    }

   public void TemporizadorResistencia(EfectoResistencia resistencia)
    {
        switch (resistencia)
        {
            case EfectoResistencia.Ninguna:
                infoResistencia.text = "Resistencia Normal";
                infoResistencia.color = Color.green; break;
            case EfectoResistencia.Frio:
                infoResistencia.text = "Resistencia al Frio";
                infoResistencia.color = Color.blue; break;
            case EfectoResistencia.FrioExtremo:
                infoResistencia.text = "Resistencia al Frio Extremo";
                infoResistencia.color = Color.cyan; break;
            case EfectoResistencia.Calor:
                infoResistencia.text = "Resistencia al Calor";
                infoResistencia.color = Color.yellow; break;
            case EfectoResistencia.CalorExtremo:
                infoResistencia.text = "Resistencia al Calor Extremo";
                infoResistencia.color = Color.red; break;

        }
    }

    IEnumerator resetearBioma(Collider bioma)
    {
        bioma.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        bioma.gameObject.SetActive(true);
    }
}
