using System;
using System.IO;

namespace src
{
	class Functions
	{
		static double T_0;
		static double m;

		static public double Rp(double I)
		{
			T_0 = Interpolation.LinearInterpolation(Constants.I, Constants.T_0, I);
			m = Interpolation.LinearInterpolation(Constants.I, Constants.m, I);
			double integral_value = Integral.Trapezoidal(f_integral, 0, 1);
			double denominator = 2 * Math.PI * Constants.R_squared * integral_value;
			return Constants.l_e / denominator;
		}

		static double f_integral(double x)
		{
			double sigma_arg = (Constants.T_w - T_0) * Math.Pow(x, m) + T_0;
			double[] new_sigma = new double[Constants.Sigma.Length - 1];
			for (int i = 0; i < new_sigma.Length; i++)
				new_sigma[i] = Math.Log(Constants.Sigma[i]);
			return Math.Exp(Interpolation.LinearInterpolation(Constants.T, new_sigma, sigma_arg)) * x;
		}

		static double dI(double I, double U)
		{
			return (U - (Constants.R_k + Rp(I)) * I) / Constants.L_k;
			// return U / Constants.L_k; // График для (Rp + Rk) = 0
			// return (U - 200 * I) / Constants.L_k; // График для (Rp + Rk) = 200
		}

		static double dU(double I)
		{
			return -I / Constants.C_k;
		}

		public static double f(double x, double I, double U) => dI(I, U);
		public static double g(double x, double I, double z) => dU(I);

	}
}