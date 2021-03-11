using System;
using System.Collections.Generic;
using System.IO;

namespace src
{
	class Program
	{
		public static void SaveResult()
		{
			FileStream file = new FileStream("results/data/t.txt", FileMode.Open, FileAccess.Write);
			StreamWriter writer_t = new StreamWriter(file);

			file = new FileStream("results/data/U.txt", FileMode.Open, FileAccess.Write);
			StreamWriter writer_U = new StreamWriter(file);

			file = new FileStream("results/data/I.txt", FileMode.Open, FileAccess.Write);
			StreamWriter writer_I = new StreamWriter(file);

			List<double> arr_t = new List<double>();
			List<double> arr_I = new List<double>();
			List<double> arr_U = new List<double>();
			Tuple<double, double> curr;

			arr_t.Add(0);
			arr_I.Add(Constants.I_0);
			arr_U.Add(Constants.U_c0);

			for (double t = 0; t < 800e-6; t += 1e-6)
			{
				writer_t.WriteLine(arr_t[arr_t.Count - 1]);
				writer_I.WriteLine(arr_I[arr_I.Count - 1]);
				writer_U.WriteLine(arr_U[arr_U.Count - 1]);

				curr = Runge.Runge_Kutta(Functions.f, Functions.g, 0, arr_I[arr_I.Count - 1], arr_U[arr_U.Count - 1], 1e-6);

				arr_t.Add(t);
				arr_I.Add(curr.Item1);
				arr_U.Add(curr.Item2);
			}

			writer_t.Close();
			writer_I.Close();
			writer_U.Close();
			file.Close();

		}
		static void Main(string[] args)
		{
			SaveResult();
		}
	}
}

