using Assets.Editor.Algoritmo_Genetico;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public AlgoritmoGenetico AlgoritmoGenetico;      
    public GameObject ModeloDeCoche;
    public Text txtVelocidad, txtGeneracion, txtIndividuo, txtRotacion, txtMatriz;

    void Awake()
    {
        this.AlgoritmoGenetico = new AlgoritmoGenetico();

        AlgoritmoGenetico.TxtVelocidad = txtVelocidad;
        AlgoritmoGenetico.TxtGeneracion = txtGeneracion;
        AlgoritmoGenetico.TxtIndividuo = txtIndividuo;
        AlgoritmoGenetico.TxtRotacion = txtRotacion;
        AlgoritmoGenetico.TxtMatriz = txtMatriz;        

        this.AlgoritmoGenetico.Evolucionar();
    }

    void Update()
    {
        if (AlgoritmoGenetico.GeneracionEnMarcha == false)
        {
            AlgoritmoGenetico.Evolucionar();
        }
    }
}
