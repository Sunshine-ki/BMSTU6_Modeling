using System;
using System.Collections.Generic;

namespace src
{
	class Conditions
	{
		static public double[] GetLeftConditions(List<double> T)
		{
			double c_plus = Functions.ApproximationPlus(Functions.c, T[0], Constants.t);
			double k_plus = Functions.ApproximationPlus(Functions.k, T[0], Constants.t);

			double K0 = Constants.h / 8 * c_plus + Constants.h / 4 * Functions.c(T[0]) + Constants.t / Constants.h * k_plus + 
					Constants.t * Constants.h / 8 * Functions.p(Constants.h / 2) + Constants.t * Constants.h / 4 * Functions.p(0);

			double M0 = Constants.h / 8 * c_plus - Constants.t / Constants.h * k_plus + Constants.t * Constants.h / 8 * Functions.p(Constants.h / 2);

			double P0 = Constants.h / 8 * c_plus * (T[0] + T[1]) + Constants.h / 4 * Functions.c(T[0]) * T[0] + 
					Constants.F0 * Constants.t + Constants.t * Constants.h / 8 * (3 * Functions.f(0) + Functions.f(Constants.h));

			double[] result = { K0, M0, P0 };
			return result;
		}

		static public double[] GetRightConditions(List<double> T)
		{
			double c_minus = Functions.ApproximationMinus(Functions.c, T[T.Count - 1], Constants.t);
			double k_minus = Functions.ApproximationMinus(Functions.k, T[T.Count - 1], Constants.t);

			double KN = Constants.h / 8 * c_minus + Constants.h / 4 * Functions.c(T[T.Count - 1]) + Constants.t / Constants.h * k_minus + 
					Constants.t * Constants.alphaN + Constants.t * Constants.h / 8 * Functions.p(Constants.l - Constants.h / 2) + 
					Constants.t * Constants.h / 4 * Functions.p(Constants.l);

			double MN = Constants.h / 8 * c_minus - Constants.t / Constants.h * k_minus + 
					Constants.t * Constants.h / 8 * Functions.p(Constants.l - Constants.h / 2);

			double PN = Constants.h / 8 * c_minus * (T[T.Count - 1] + T[T.Count - 2]) + Constants.h / 4 * Functions.c(T[T.Count - 1]) * T[T.Count - 1] + 
					Constants.t * Constants.alphaN * Constants.T0 + Constants.t * Constants.h / 4 * (Functions.f(Constants.l) + Functions.f(Constants.l - Constants.h / 2));

			double[] result = {KN, MN, PN};
			return result;
		}

		static public List<double> GetNewT(List<double> T)
		{
			double[] cond = GetLeftConditions(T);
			double K0 = cond[0], M0 = cond[1], P0 = cond[2];

			cond = GetRightConditions(T);
			double KN = cond[0], MN = cond[1], PN = cond[2];

			List<double> xi = new List<double>();
			xi.Add(0); xi.Add(-M0 / K0);

			List<double> eta = new List<double>();
			eta.Add(0); eta.Add(P0 / K0);

			double x = Constants.h, Tn, denominator, next_xi, next_eta;
			int n = 1;

			while (x + Constants.h < Constants.l)
			{
				Tn = T[n];
				denominator = (Functions.B(x, Tn) - Functions.A(Tn) * xi[n]);

				next_xi = Functions.D(Tn) / denominator;
				next_eta = (Functions.F(x, Tn) + Functions.A(Tn) * eta[n]) / denominator;

				xi.Add(next_xi);
				eta.Add(next_eta);

				n++;
				x += Constants.h;
			} 

			List<double> T_new = new List<double>();
			for (int i = 0; i < n + 1; i++)
				T_new.Add(0);
			
			T_new[n] = (PN - MN * eta[n]) / (KN + MN * xi[n]);
			
			for (int i = n - 1; i > -1; i--)
				T_new[i] = xi[i + 1] * T_new[i + 1] + eta[i + 1];

			return T_new;
		}

	}
}