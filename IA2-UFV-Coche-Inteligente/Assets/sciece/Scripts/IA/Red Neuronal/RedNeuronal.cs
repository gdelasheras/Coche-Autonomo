using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NeuralNetwork.NetworkModels
{
    public class RedNeuronal
    {
        public Network Red;

        public RedNeuronal(int Entradas, int CapasOcultas, int Salidas)
        {
            List<Synapse> sinapsis = new List<Synapse>();
            var network = new Network();
            var allNeurons = new List<Neuron>();

            #region - Capas -

            //Input Layer
            for (int i = 0; i < Entradas; i++)
            {
                var neuron = new Neuron
                {
                    Id = Guid.NewGuid(),
                    Value = 0.0
                };

                network.InputLayer.Add(neuron);
                allNeurons.Add(neuron);
            }

            //Hidden Layers
            for (int i = 0; i < CapasOcultas; i++)
            {
                var neuron = new Neuron
                {
                    Id = Guid.NewGuid(),
                    Value = 0.0
                };

                network.HiddenLayer.Add(neuron);
                allNeurons.Add(neuron);
            }

            //Salidas
            for (int i = 0; i < Salidas; i++)
            {
                var neuron = new Neuron
                {
                    Id = Guid.NewGuid(),
                    Value = 0.0
                };

                network.OutputLayer.Add(neuron);
                allNeurons.Add(neuron);
            }

            #endregion

            //Synapses
            //Conexión capa de entrada y oculta
            for (int i = 0; i < network.InputLayer.Count; i++)
            {
                var neuronaCapaEntrada = network.InputLayer[i];

                for (int j = 0; j < network.HiddenLayer.Count; j++)
                {
                    // Cogemos cada neurona de la capa de entrada
                    var synapse = new Synapse { Id = Guid.NewGuid() };

                    // Unimos con cada neurona de la cada oculta
                    var neuronaCapaOculta = network.HiddenLayer[j];

                    synapse.InputNeuron = neuronaCapaEntrada;
                    synapse.OutputNeuron = neuronaCapaOculta;
                    synapse.Weight = 0f;

                    neuronaCapaEntrada.OutputSynapses.Add(synapse);
                    neuronaCapaOculta.InputSynapses.Add(synapse);

                    sinapsis.Add(synapse);
                }                    
            }

            //Conexión capa oculta y la salida
            for (int i = 0; i < network.HiddenLayer.Count; i++)
            {
                var neuronaCapaOculta = network.HiddenLayer[i];

                for (int j = 0; j < network.OutputLayer.Count; j++)
                {
                    // Cogemos cada neurona de la capa de entrada
                    var synapse = new Synapse { Id = Guid.NewGuid() };

                    // Unimos con cada neurona de la cada oculta
                    var neuronaCapaSalida = network.OutputLayer[j];

                    synapse.InputNeuron = neuronaCapaOculta;
                    synapse.OutputNeuron = neuronaCapaSalida;
                    synapse.Weight = 0f;

                    neuronaCapaOculta.OutputSynapses.Add(synapse);
                    neuronaCapaSalida.InputSynapses.Add(synapse);

                    sinapsis.Add(synapse);
                }
            }

            Red = network;
            Red.Conexiones = sinapsis;
        }
        
        public void ImprimirConexiones()
        {
            string result = "{ ";

            for (int i = 0; i < Red.Conexiones.Count; i++)
            {
                result += Red.Conexiones[i].Weight + ", ";
            }

            result += "}";

            Debug.Log("Matriz de pesos: " + result);
        }

        public void SetPesos(double[] MatrizDePesos)
        {
            Red.SetPesos(MatrizDePesos);
        }

        private HelperNetwork GetHelperNetwork()
        {
            try
            {
                string path = "Assets\\Scripts\\IA\\Red Neuronal\\Modelo\\red.txt";

                StreamReader reader = new StreamReader(path);

                string json = reader.ReadToEnd();

                json = Regex.Replace(json, @"\s+", "");

                //Debug.Log(json);

                return JsonUtility.FromJson<HelperNetwork>(json.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    #region - Helpers -

    [System.Serializable]
    public class HelperNetwork
    {
        public List<HelperNeuron> InputLayer;
        public List<HelperNeuron> HiddenLayer;
        public List<HelperNeuron> OutputLayer;
        public List<HelperSynapse> Synapses;

        public HelperNetwork()
        {
            InputLayer = new List<HelperNeuron>();
            HiddenLayer = new List<HelperNeuron>();
            OutputLayer = new List<HelperNeuron>();
            Synapses = new List<HelperSynapse>();
        }
    }

    [System.Serializable]
    public class HelperNeuron
    {
        public string Id;
        public double Value;

        public HelperNeuron()
        {
        }
    }

    [System.Serializable]
    public class HelperSynapse
    {
        public string Id;
        public string OutputNeuronId;
        public string InputNeuronId;
        public double Weight;
    }

    #endregion
}