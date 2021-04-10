using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace src
{
	class Program
	{
		public static void SaveResult()
		{
			FileStream file = new FileStream("results/data/t_4.txt", FileMode.Open, FileAccess.Write);
			StreamWriter writer_t = new StreamWriter(file);

			// file = new FileStream("results/data/U.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_U = new StreamWriter(file);

			file = new FileStream("results/data/I_4.txt", FileMode.Open, FileAccess.Write);
			StreamWriter writer_I = new StreamWriter(file);

			// file = new FileStream("results/data/IU.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_IU = new StreamWriter(file);

			// file = new FileStream("results/data/RpI.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_RpI = new StreamWriter(file);

			// file = new FileStream("results/data/T_0.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_T_0 = new StreamWriter(file);

			// file = new FileStream("results/data/T0_Rp.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_T0_Rp = new StreamWriter(file);
			// double T_0, R_p;

			// file = new FileStream("results/data/T0_Rp_Rk_0.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_Rp_Rk_0 = new StreamWriter(file);

			// file = new FileStream("results/data/T0_Rp_Rk_200.txt", FileMode.Open, FileAccess.Write);
			// StreamWriter writer_Rp_Rk_200 = new StreamWriter(file);


			List<double> arr_t = new List<double>();
			List<double> arr_I = new List<double>();
			List<double> arr_U = new List<double>();
			List<double> arr_IU = new List<double>();

			Tuple<double, double> curr;
			double I_curr, U_curr, IU_curr;

			arr_t.Add(0);
			arr_I.Add(Constants.I_0);
			arr_U.Add(Constants.U_c0);
			arr_IU.Add(Constants.I_0 * Constants.U_c0);

			// double step = 1e-6;
			// for (double t = 0; t <= 20; t += step)
			// for (double t = 0; t <= 20e-6; t += 1e-6)
			// for (double t = 1E-06; t <= 5000e-6; t += 1e-6)
			for (double t = 1E-06; t <= 800e-6; t += 1e-6)
			{
				I_curr = arr_I[arr_I.Count - 1];
				U_curr = arr_U[arr_U.Count - 1];
				IU_curr = arr_IU[arr_IU.Count - 1];

				// writer_t.WriteLine(arr_t[arr_t.Count - 1]);
				// * Functions.Rp(I_curr));
				// writer_I.WriteLine(I_curr);
				// writer_U.WriteLine(U_curr);
				// writer_IU.WriteLine(IU_curr);
				// writer_RpI.WriteLine(I_curr);
				// T_0 = Interpolation.LinearInterpolation(Constants.I, Constants.T_0, arr_I[arr_I.Count - 1]);
				// R_p = Functions.Rp(arr_I[arr_I.Count - 1]);
				// writer_T0_Rp.WriteLine(T_0 * R_p);
				// writer_Rp_Rk_0.WriteLine(I_curr);

				// writer_Rp_Rk_200.WriteLine(I_curr);

				curr = Runge.Runge_Kutta(Functions.f, Functions.g, 0, arr_I[arr_I.Count - 1], arr_U[arr_U.Count - 1], 1e-6);

				arr_t.Add(t);
				arr_I.Add(curr.Item1);
				arr_U.Add(curr.Item2);
				arr_IU.Add(curr.Item1 * curr.Item2);
			}


			double Imax = arr_I.Max();
			double pulseDuration = arr_I.Count(I => I > Imax * 0.35);

			Console.WriteLine($"C_k = {Constants.C_k} Ф\nL_k = {Constants.L_k} Гн\nR_k = {Constants.R_k} Ом\n");
			Console.WriteLine($"Значение тока в максимуме Imax = {Imax}");
			Console.WriteLine($"Длительность импульса t_имп = {pulseDuration}");


			writer_t.Close();
			writer_I.Close();
			// writer_U.Close();
			// writer_IU.Close();
			// writer_RpI.Close();
			// writer_T_0.Close();
			// writer_T0_Rp.Close();
			// writer_Rp_Rk_0.Close();
			// writer_Rp_Rk_200.Close();
			file.Close();

		}
		static void Main(string[] args)
		{
			// Output with point.
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			SaveResult();
		}
	}
}

