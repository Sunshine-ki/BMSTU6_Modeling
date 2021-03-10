using System;

namespace src
{
	class Program
	{
		public static double T_0;
		public static double m;

		public static double Rp(double I)
		{
			T_0 = Interpolation.LinearInterpolation(Constants.I, Constants.T_0, I);
			m = Interpolation.LinearInterpolation(Constants.I, Constants.m, I);
			double integral_value = Integral.Trapezoidal(f, 0, 1);
			double denominator = 2 * Math.PI * Constants.R_squared * integral_value;
			return Constants.l_e / denominator;
		}

		public static double f(double x)
		{
			double sigma_arg = (Constants.T_w - T_0) * Math.Pow(x, m) + T_0;
			double[] new_sigma = new double[Constants.Sigma.Length - 1];

			// FIXME: log + exp ???
			for (int i = 0; i < new_sigma.Length; i++)
				new_sigma[i] = Math.Log(Constants.Sigma[i]);

			return Math.Exp(Interpolation.LinearInterpolation(Constants.T, new_sigma, sigma_arg)) * x;
		}

		static void Main(string[] args)
		{
			// Console.WriteLine(Constants.I);
			// Console.WriteLine(Integral.Trapezoidal(my_f, 1, 6));
			// Console.WriteLine(Interpolation.LinearInterpolation(Constants.T, Constants.Sigma, 8500));
			Console.WriteLine($"Rp = {Rp(0.65)}");
		}
	}
}

