using System;

namespace src
{
	class Integral
	{
		public static double Trapezoidal(Func<double, double> f, double a, double b, double step = 0.05)
		{
			double result = f(a) + f(b);

			while (a + step < b)
			{
				a += step;
				result += 2 * f(a);
			}

			return result * (step / 2);
		}
	}
}