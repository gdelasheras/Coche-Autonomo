using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


namespace Assets.Editor.Algoritmo_Genetico
{
    public class AlgoritmoGenetico
    {
        #region - Atributos de clase -

        public List<GameObject> Individuos;
        public double[] Fitness;
        public double FitnessMaximo;

        public int NumIndividuos = 20;
        public int NumMaxGeneraciones = 10;
        public int Generacion = 1;
        public int NumIndividuoVivo = -1;

        public GameObject IndividuoVivo;

        public bool GeneracionEnMarcha = false;

        #endregion

        #region - UI -

        public Text TxtVelocidad;
        public Text TxtGeneracion;
        public Text TxtIndividuo;
        public Text TxtRotacion;
        public Text TxtMatriz;

        #endregion

        #region - Coordenadas de instanciación -

        GameObject inicio;

        public float x = 6.0f;
        public float y = 0.47f;
        public float z = 18.0f;

        #endregion

        #region - Constructor -

        public AlgoritmoGenetico()
        {
            inicio = GameObject.FindGameObjectWithTag("Inicio");

            Individuos = new List<GameObject>();

            for (int i = 0; i < NumIndividuos; i++)
            {
                GameObject prefab = null;
                prefab = (GameObject) Resources.Load("SportsCar", typeof(GameObject));
                GameObject coche = GameObject.Instantiate((GameObject)prefab, inicio.transform.position, Quaternion.Euler(0, 0, 0));
                coche.gameObject.name = "Coche - " + i + "-";
                coche.SetActive(false);
                coche.GetComponent<Coche>().SetGenotipo();
                coche.GetComponent<Coche>().PasarGenotipoARedNeuronal();

                Individuos.Add(coche);
            }

            Fitness = new double[Individuos.Count];
            FitnessMaximo = 0f;
        }

        #endregion

        private void DesactivarCoches()
        {
            foreach (GameObject coche in Individuos)
            {
                coche.SetActive(false);
            }
        }

        public void IndividuoAdelante()
        {
            // Generacion en marcha
            Debug.Log("Generacion: " + (Generacion) + ", Individuo: " + (1 + NumIndividuoVivo));

            IndividuoVivo = Individuos[NumIndividuoVivo];
            IndividuoVivo.GetComponent<Coche>().SetCamara(TxtVelocidad, TxtRotacion);
            IndividuoVivo.GetComponent<Coche>().Velocimetro = GameObject.FindGameObjectWithTag("Velocimetro");
            IndividuoVivo.SetActive(true);

            TxtGeneracion.text = "Generación: " + Generacion;
            TxtIndividuo.text = "Individuo: " + (NumIndividuoVivo + 1);
            TxtMatriz.text = "Matriz: " + IndividuoVivo.GetComponent<Coche>().PrintGenotipo();
        }

        public void Evolucionar()
        {
            NumIndividuoVivo++;
            GeneracionEnMarcha = true;
            DesactivarCoches();
            if (NumIndividuoVivo == 20)
            {
                NuevaGeneracionTest();
            }
            else
            {
                IndividuoAdelante();
            }
        }
        public void NuevaGeneracionTest()
        {
            Individuos = Individuos.OrderByDescending(i => i.GetComponent<Coche>().Fitness).ToList();
            List<GameObject> IndividuosNew = new List<GameObject>();
            List<GameObject> IndividuosMejores = new List<GameObject>();

            // Apartar los mejores
            for (int i = 0; i < 4; i++)
            {
                IndividuosMejores.Add(Individuos[i]);
            }

            int numMejores = 0;

            foreach (GameObject nuevoindividuo in IndividuosMejores)
            {
                double[] mejorPesos = nuevoindividuo.gameObject.GetComponent<Coche>().GetMatrizDePesos();
                double mejorFitness = nuevoindividuo.gameObject.GetComponent<Coche>().GetFitness();

                for (int i = 0; i < NumIndividuos / 4; i++)
                {
                    GameObject prefab = null;
                    prefab = (GameObject)Resources.Load("SportsCar", typeof(GameObject));
                    GameObject coche = GameObject.Instantiate((GameObject)prefab, inicio.transform.position, Quaternion.Euler(0, 0, 0));
                    coche.GetComponent<Coche>().Mutar(mejorPesos, nuevoindividuo.gameObject.GetComponent<Coche>().Fitness);
                    coche.GetComponent<Coche>().PasarGenotipoARedNeuronal();
                    coche.SetActive(false);

                    IndividuosNew.Add(coche);
                }

                IndividuosNew[numMejores].GetComponent<Coche>().Fitness = mejorFitness;
                IndividuosNew[numMejores].GetComponent<Coche>().SetGenotipo(mejorPesos);
                IndividuosNew[numMejores].GetComponent<Coche>().PasarGenotipoARedNeuronal();

                numMejores += NumIndividuos / 4;
            }

            for (int i = 0; i < NumIndividuos; i++)
            {
                Individuos[i].gameObject.GetComponent<Coche>().Destruir();
            }

            Individuos = new List<GameObject>();

            Individuos = IndividuosNew; 

            Generacion++;

            TxtGeneracion.text = "Generación: " + Generacion;

            NumIndividuoVivo = -1;

            GeneracionEnMarcha = false;

            Evolucionar();
        }

        public void NuevaGeneracion()
        {
            Individuos = Individuos.OrderByDescending(i => i.GetComponent<Coche>().Fitness).ToList();

            GameObject Mejor = null;

            for (int i = 0; i < Individuos.Count - 1; i++)
            {
                double a = Individuos[i].GetComponent<Coche>().GetFitness();
                double b;

                if (Mejor == null)
                {
                    Mejor = Individuos[i];
                }
                else
                {
                    b = Mejor.GetComponent<Coche>().GetFitness();

                    if (a > b)
                        Mejor = Individuos[i];
                }
            }

            double[] mejorPesos = Mejor.gameObject.GetComponent<Coche>().GetMatrizDePesos();

            for (int i = 0; i < NumIndividuos; i++)
            {
                Individuos[i].gameObject.GetComponent<Coche>().Destruir();
            }

            Individuos = new List<GameObject>();

            Generacion++;

            TxtGeneracion.text = "Generación: " + Generacion;

            NumIndividuoVivo = -1;

            for (int i = 0; i < NumIndividuos; i++)
            {
                GameObject prefab = null;
                prefab = (GameObject )Resources.Load("SportsCar", typeof(GameObject));
                GameObject coche = GameObject.Instantiate((GameObject)prefab, inicio.transform.position, Quaternion.Euler(0, 0, 0));
                coche.GetComponent<Coche>().Mutar(mejorPesos, Mejor.gameObject.GetComponent<Coche>().Fitness);
                coche.GetComponent<Coche>().PasarGenotipoARedNeuronal();
                coche.SetActive(false);

                Individuos.Add(coche);
            }

            Individuos[0].GetComponent<Coche>().SetGenotipo(mejorPesos);
            Individuos[0].GetComponent<Coche>().PasarGenotipoARedNeuronal();

            GeneracionEnMarcha = false;

            Evolucionar();
        }
    }
}
