using System;

namespace Assets.Scripts.Utils
{
    public static class Utils
    {
        private static readonly Random Random = new Random();

        public static double GetRandom()
        {
            return GetSigno() * Random.NextDouble();
        }

        public static int GetSigno()
        {
            if (Random.Next(2) == 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = Random.NextDouble();
            return minValue + (next * (maxValue - minValue));
        }

        public static double[] GetMatrizRandom(int Cantidad)
        {
            double[] Matriz = new double[Cantidad];

            for (int i = 0; i < Cantidad; i++)
            {
                Matriz[i] = GetRandom();
            }

            return Matriz;
        }

        public static bool GetMutacion()
        {
            double valor = Random.NextDouble();

            if (valor >= 0.85)
            {
                return true;
            }

            return false;
        }
    }
}
