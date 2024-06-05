using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlIconoTemperatura : MonoBehaviour
{
    public Sprite iconoCalor;
    public Sprite iconoTemperaturaNormal;
    public Sprite iconoFrio;
    public Sprite iconoTempladoCalor;
    public Sprite iconoTempladoFrio;
    public Sprite iconoTempladoCalorExtremo;
    public Sprite iconoTempladoFrioExtremo;

    public void PonerTemperaturaNormal() {
    
        GetComponent<RawImage>().texture = iconoTemperaturaNormal.texture;
    }
    public void PonerTemperaturaCalor()
    {
        GetComponent<RawImage>().texture = iconoCalor.texture;
    }
    public void PonerTemperaturaFrio()
    {
        GetComponent<RawImage>().texture = iconoFrio.texture;
    }
    public void PonerTemperaturaTempladoFrio()
    {
        GetComponent<RawImage>().texture = iconoTempladoFrio.texture;
    }
    public void PonerTemperaturaTempladoCalor() { 

        GetComponent<RawImage>().texture = iconoTempladoCalor.texture;
    }
    public void PonerTemperaturaTempladoFrioExtremo()
    {
        GetComponent<RawImage>().texture = iconoTempladoFrioExtremo.texture;
    }
    public void PonerTemperaturaTempladoCalorExtremo()
    {

        GetComponent<RawImage>().texture = iconoTempladoCalorExtremo.texture;
    }
}
