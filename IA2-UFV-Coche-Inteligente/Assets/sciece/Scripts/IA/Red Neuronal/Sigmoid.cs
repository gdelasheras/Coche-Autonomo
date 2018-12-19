using System;

namespace NeuralNetwork.NetworkModels
{
    public static class Sigmoid
    {
        public static double Salida(double x)
        {
            if (x > 1) return 1.0;
            else if (x < -1) return -1.0;
            else
                return Math.Tanh(x);
        }

        public static double Relu (double x)
        {
            return Math.Max(0, x);
        }
    }
}