using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class IndicadorDeterioro : MonoBehaviour
{
    public Image imagenMarco;
    public Sprite imagenMarcoNormal;
    public Sprite imagenMarcoCalor;
    public Sprite imagenMarcoFrio;
    public float velocidadAparicion;

    private Coroutine desaparecer;
       public void Aparecer()
        {
        imagenMarco.sprite = imagenMarcoNormal;
        if (desaparecer != null) {
        StopCoroutine(desaparecer);
        }

        imagenMarco.enabled = true;
        imagenMarco.color = Color.white;
        desaparecer = StartCoroutine(Desaparecer());
    }

    IEnumerator Desaparecer()
    {
        float alpha = 1.0f;
        while (alpha>0.0f)
        {
            alpha -= (1.0f/velocidadAparicion)*Time.deltaTime;
            imagenMarco.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
        imagenMarco.enabled = false;
    }
    public void AparecerCambioBioma(string bioma)
    {
        if ( bioma == "G")
        {
            imagenMarco.sprite = imagenMarcoFrio;
        } else if (bioma == "V")
        {
            imagenMarco.sprite = imagenMarcoCalor;
        }

        imagenMarco.enabled = true;
    }
    public void QuitarCambioBioma()
    {
        imagenMarco.enabled=false;
    }
}
