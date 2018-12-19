using NeuralNetwork.NetworkModels;
using Assets.Scripts;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Utils;
using UnityEngine.UI;

public class Coche : MonoBehaviour
{
    public double Velocidad = 0;
    public double Angulo = 0;

    double distanciaRecorrida = 0;
    Vector3 ultimaposicion;

    public WheelDrive MovimientoCoche;

    public RedNeuronal Red;

    public Vector3 origen = new Vector3(6.0f, 0.40f, 18.0f);    

    public bool IsVivo, IsArrancado = false;
    
    float tiempo = 0;

    #region - UI -

    public Text txtVelocidad, txtRotacion;
    public GameObject Volante;
    public GameObject Camara;

    public void SetCamara(Text txtVelocidad, Text txtRotacion)
    {
        this.Camara = GameObject.FindGameObjectWithTag("MainCamera");
        Camara.transform.parent = gameObject.transform;
        Camara.transform.localPosition = new Vector3(0, 8f, -12.5f);
        Camara.transform.LookAt(gameObject.transform);
        this.txtVelocidad = txtVelocidad;
        this.txtRotacion = txtRotacion;
    }

    #endregion

    #region - Sensores -

    // Sensores
    public GameObject SensorFrontal;
    public GameObject SensorIzquierda;
    public GameObject SensorDerecha;
    public GameObject SensorFrontalIzq;
    public GameObject SensorFrontalDer;
    public GameObject SensorLateralIzq;
    public GameObject SensorLateralDer;

    public int numPos = 0;

    public GameObject Velocimetro;

    public double[] EntradasSensores;

    public double[] GetEntradaSensores()
    {
        double[] entradas = new double[7];

        entradas[0] = SensorFrontal.GetComponent<Sensor>().GetDistancia();

        entradas[1] = SensorIzquierda.GetComponent<Sensor>().GetDistancia();
        entradas[2] = SensorDerecha.GetComponent<Sensor>().GetDistancia();

        entradas[3] = SensorFrontalIzq.GetComponent<Sensor>().GetDistancia();
        entradas[4] = SensorFrontalDer.GetComponent<Sensor>().GetDistancia();

        entradas[5] = SensorLateralIzq.GetComponent<Sensor>().GetDistancia();
        entradas[6] = SensorLateralDer.GetComponent<Sensor>().GetDistancia();

        return entradas;
    }

    #endregion

    #region - Genotipo - 

    public double Fitness;
    public  double[] MatrizDePesos;

    public double[] GetMatrizDePesos()
    {
        return MatrizDePesos;
    }

    public double GetFitness()
    {
        return Fitness;
    }

    public void Mutar(double[] Matriz, double Fitness)
    {
        for (int i = 0; i < MatrizDePesos.Length; i++)
        {
            if (Utils.GetMutacion())
            {
                // Muta
                    MatrizDePesos[i] = Utils.GetRandom();
               
            }
            else
            {
                MatrizDePesos[i] = Matriz[i];
                // No muta
            }
        }

        this.Fitness = 0.0;
    }

    public void SetGenotipo(double[] MatrizDePesos)
    {
        this.MatrizDePesos = MatrizDePesos;
        this.Fitness = 0.0;
    }

    public void SetGenotipo(int Sinapsis)
    {
        MatrizDePesos = new double[Sinapsis];

        for (int i = 0; i < MatrizDePesos.Length; i++)
        {
            MatrizDePesos[i] = Utils.GetRandom();
        }

        this.Fitness = 0.0;
    }

    public void SetGenotipo()
    {
        MatrizDePesos = new double[Red.Red.Conexiones.Count];

        for (int i = 0; i < MatrizDePesos.Length; i++)
        {
            MatrizDePesos[i] = Utils.GetRandom();
        }

        this.Fitness = 0.0;
    }

    public void PasarGenotipoARedNeuronal()
    {
        for (int i = 0; i < MatrizDePesos.Length; i++)
        {
            Red.Red.Conexiones[i].Weight = MatrizDePesos[i];
        }
    }

    public string PrintGenotipo()
    {
        string result = "{ ";

        for (int i = 0; i < MatrizDePesos.Length; i++)
        {
            result += MatrizDePesos[i] + ", ";
        }

        result += "}";

        return result;
    }

    #endregion
    
    void Awake()
    {
        Red = new RedNeuronal(7, 5, 2);
        IsVivo = true;
        IsArrancado = false;
        MovimientoCoche = gameObject.GetComponent<WheelDrive>();
        Volante = GameObject.FindGameObjectWithTag("Volante");
        Volante.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
        this.MatrizDePesos = new double[Red.Red.Conexiones.Count];

        ultimaposicion = transform.position;
    }
    
    void Update()
    {
        if (IsVivo)
        {
            numPos++;
            tiempo += Time.deltaTime;
            EntradasSensores = GetEntradaSensores();
            Red.Red.ForwardPropagate(EntradasSensores);
            Velocidad = Red.Red.GetSalidas()[0];
            Angulo = Red.Red.GetSalidas()[1];
                      

            Volante.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
            Volante.transform.Rotate(new Vector3(0f, 0f, 1f), (-1) * (float) Angulo * 30);
            
            MovimientoCoche.EntradaLados = (float)Angulo;
            MovimientoCoche.EntradaVelocidad = (float)Velocidad;
                       
            Velocimetro.GetComponent<SpeedoMeterScript>().currentValue = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            txtRotacion.text = "Ángulo: " + (Angulo * 30) + "º";
            txtVelocidad.text = "Velocidad: " + (Velocidad);

            distanciaRecorrida += Vector3.Distance(transform.position, ultimaposicion);

            ultimaposicion = transform.position;

            // Se ha parado
            if (transform.InverseTransformDirection(gameObject.GetComponent<Rigidbody>().velocity).z <= -0.1)
            {
                Morir();
            }

            if (numPos % 1000 == 0)
            {
                if (distanciaRecorrida < 5)
                {
                    Morir();
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Morir();
    }

    public void Morir()
    {
        Camara.transform.parent = null;
        Velocimetro = null;
        Volante = null;
        Fitness = distanciaRecorrida;
        Debug.Log("FITNESS: " + Fitness);
        IsVivo = false;
        gameObject.SetActive(false);

        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
     
        GC.GetComponent<GameController>().AlgoritmoGenetico.Evolucionar();
    }

    public void Destruir()
    {
        Destroy(gameObject);
    }
}
