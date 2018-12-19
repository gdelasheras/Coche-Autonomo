using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.NetworkModels
{
	public class Neuron
	{
		#region -- Properties --

		public Guid Id { get; set; }
		public List<Synapse> InputSynapses { get; set; }
		public List<Synapse> OutputSynapses { get; set; }
		public double NormalizedValue { get; set; }
		public double Value { get; set; }

		#endregion

		#region -- Constructors --
		public Neuron()
		{
			Id = Guid.NewGuid();
			InputSynapses = new List<Synapse>();
			OutputSynapses = new List<Synapse>();
		}

		public Neuron(IEnumerable<Neuron> inputNeurons) : this()
		{
			foreach (var inputNeuron in inputNeurons)
			{
				var synapse = new Synapse(inputNeuron, this);
				inputNeuron.OutputSynapses.Add(synapse);
				InputSynapses.Add(synapse);
			}
		}
		#endregion

		#region -- Values & Weights --
		public virtual double CalculateValue()
		{
			return Value = Sigmoid.Salida(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value));
		}

        public virtual double CalculateHiddenValue()
        {
            return Value = Sigmoid.Relu(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value));
        }


        public double CalculateError(double target)
		{
			return target - Value;
		}
		#endregion
	}
}