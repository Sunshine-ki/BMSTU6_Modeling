using System;
using System.Collections.Generic;

namespace src
{
	class Program
	{
		static void Main(string[] args)
		{
			List<double> arr_t = new List<double>();
			List<double> arr_I = new List<double>();
			List<double> arr_U = new List<double>();
			Tuple<double, double> curr;

			arr_t.Add(0);
			arr_I.Add(Constants.I_0);
			arr_U.Add(Constants.U_c0);

			for (double t = 0; t < 800e-6; t += 1e-6)
			{
				curr = Runge.Runge_Kutta(Functions.f, Functions.g, 0, arr_I[arr_I.Count - 1], arr_U[arr_U.Count - 1], 1e-6);

				arr_t.Add(t);
				arr_I.Add(curr.Item1);
				arr_U.Add(curr.Item2);
			}


			for (int i = 0; i < arr_I.Count; i++)
			{
				Console.WriteLine($"{arr_I[i]}");
			}

			// foreach (double elem in arr_I)
			// 	Console.WriteLine(elem);
		}
	}
}

