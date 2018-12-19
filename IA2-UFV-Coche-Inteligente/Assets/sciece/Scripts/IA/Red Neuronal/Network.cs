using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.NetworkModels
{
	public class Network
	{
        #region -- Properties --
        public List<Neuron> InputLayer { get; set; }
        public List<Neuron> HiddenLayer { get; set; }
        public List<Neuron> OutputLayer { get; set; }

        public List<Synapse> Conexiones { get; set; }
		#endregion

		#region -- Globals --
		private static readonly Random Random = new Random();
		#endregion

		#region -- Constructor --
		public Network()
		{
			InputLayer = new List<Neuron>();
            HiddenLayer = new List<Neuron>();
            OutputLayer = new List<Neuron>();
            Conexiones = new List<Synapse>();
        }

		public Network(int inputSize, int hiddenSize, int outputSize)
		{
            InputLayer = new List<Neuron>();
            HiddenLayer = new List<Neuron>();
            OutputLayer = new List<Neuron>();

            for (var i = 0; i < inputSize; i++)
                InputLayer.Add(new Neuron());

            for (var i = 1; i < hiddenSize; i++)            
                HiddenLayer.Add(new Neuron());            

            for (var i = 0; i < outputSize; i++)
                OutputLayer.Add(new Neuron(HiddenLayer));
        }
        #endregion

        #region -- Training --
        public  void ForwardPropagate(params double[] inputs)
        {
            var i = 0;
            InputLayer.ForEach(a => a.Value = inputs[i++]);
            HiddenLayer.ForEach(a => a.CalculateValue());
            OutputLayer.ForEach(a => a.CalculateValue());
        }
        #endregion

        public List<double> GetSalidas()
        {
            List<double> result = new List<double>();

            foreach (Neuron neurona in OutputLayer)
            {
                result.Add(neurona.Value);
            }

            return result;
        }

        public void SetEntrada(double[] Entrada)
        {
            try
            {
                if (Entrada.Length == InputLayer.Count)
                {
                    for (int i = 0; i < Entrada.Length; i++)
                    {
                        InputLayer[i].Value = Entrada[i];
                    }
                }
                else
                {
                    throw new Exception("Error, los campos de entrada y la capa entrada no coincidn.");
                }
            }
            catch (Exception E)
            {

            }            
        }

        public void SetPesos(double[] MatrizDePesos)
        {
            try
            {
                if (MatrizDePesos.Length == Conexiones.Count)
                {
                    for (int i = 0; i < MatrizDePesos.Length; i++)
                    {
                        Conexiones[i].Weight = MatrizDePesos[i];
                    }
                }
                else
                {
                    throw new Exception("Error, los campos de entrada y la capa entrada no coincidn.");
                }
            }
            catch (Exception E)
            {

            }
        }

        #region -- Helpers --
        public static double GetRandom()
		{
			return 2 * Random.NextDouble() - 1;
		}
        #endregion        
    }
}