using System;
using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private int puntos;
    private TextMeshProUGUI textMesh;

    public int Puntos => puntos;
    public event Action<int> PuntosCambiados;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        puntos = 0;
        ActualizarTexto();
    }

    public void SumarPuntos(float puntosEntrada)
    {
        int cantidad = Mathf.Max(0, Mathf.RoundToInt(puntosEntrada));
        puntos += cantidad;
        ActualizarTexto();
        PuntosCambiados?.Invoke(puntos);
    }

    private void ActualizarTexto()
    {
        if (textMesh != null)
        {
            textMesh.text = $"PUNTAJE  {puntos:0000}";
        }
    }
}
