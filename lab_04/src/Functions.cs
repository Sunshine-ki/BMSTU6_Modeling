using System;

namespace src
{
	class Functions
	{
		static public double k(double T)
		{
			return Constants.a1 * (Constants.b1 + Constants.c1 * Math.Pow(T, Constants.m1));
		}

		static public double c(double T)
		{
			return Constants.a2 + Constants.b2 * Math.Pow(T, Constants.m2) - (Constants.c2 / Math.Pow(T, 2));
		}
		
		static public double alpha(double x)
		{
			double d = (Constants.alphaN * Constants.l) / (Constants.alphaN - Constants.alpha0);
			double c = -Constants.alpha0 * d;
			return c / (x - d);
		}

		static public double p(double x)
		{
			return alpha(x) * 2 / Constants.R;
		}

		static public double f(double x)
		{
			return alpha(x) * 2 * Constants.T0 / Constants.R;
		}

		static public double ApproximationPlus(Func<double, double> f, double n, double step)
		{
			return (f(n) + f(n + step)) / 2;
		}

		static public double ApproximationMinus(Func<double, double> f, double n, double step)
		{
			return (f(n) + f(n - step)) / 2;
		}

		static public double A(double T)
		{
			return Constants.t / Constants.h * ApproximationMinus(k, T, Constants.t);
		}

		static public double D(double T)
		{
			return Constants.t / Constants.h * ApproximationPlus(k, T, Constants.t);
		}

		static public double B(double x, double T)
		{
			return A(T) + D(T) + Constants.h * c(T) + Constants.h * Constants.t * p(x);
		}

		static public double F(double x, double T)
		{
	    	return Constants.h * Constants.t * f(x) + T * Constants.h* c(T);
		}


	}
}