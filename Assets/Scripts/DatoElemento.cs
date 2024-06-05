using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoUsoElemento
{
    Hambre,Sed,Salud,Descanso
}
public enum EfectoResistencia
{
    Ninguna,Calor, Frio, CalorExtremo, FrioExtremo
}

[CreateAssetMenu(fileName ="Elementos", menuName ="Nuevo Elemento")]
public class DatoElemento : ScriptableObject
{
    [Header("Info")]
    public string nombre;
    public string descripcion;
    public int valorElemento;
    public Sprite icono;
    public GameObject PrefabSoltar;
    public TipoUsoElemento tipo;
    public EfectoResistencia resistencia;
}
