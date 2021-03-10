using System;

namespace src
{
	class Runge
	{
		public static Tuple<double, double> Runge_Kutta(
											Func<double, double, double, double> f,
											Func<double, double, double, double> g,
											double xn, double yn, double zn,
											double h)
		{
			double k1, k2, k3, k4;
			double p1, p2, p3, p4;

			k1 = h * f(xn, yn, zn);
			p1 = h * g(xn, yn, zn);

			k2 = h * f(xn + h / 2, yn + k1 / 2, zn + p1 / 2);
			p2 = h * g(xn + h / 2, yn + k1 / 2, zn + p1 / 2);

			k3 = h * f(xn + h / 2, yn + k2 / 2, zn + p2 / 2);
			p3 = h * g(xn + h / 2, yn + k2 / 2, zn + p2 / 2);

			k4 = h * f(xn + h, yn + k3, zn + p3);
			p4 = h * g(xn + h, yn + k3, zn + p3);

			double y_next = yn + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
			double z_next = zn + (p1 + 2 * p2 + 2 * p3 + p4) / 6;

			return Tuple.Create(y_next, z_next);
		}

	}

}