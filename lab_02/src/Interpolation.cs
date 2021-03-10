using System;

namespace src
{
	class Interpolation
	{
		public static double LinearInterpolation(double[] xs, double[] ys, double x)
		{
			int left, right;

			if (x < xs[0])
			{
				left = 0;
				right = 1;
			}
			else if (x > xs[xs.Length - 1])
			{
				left = xs.Length - 2;
				right = xs.Length - 1;
			}
			else
			{
				// Find the border.
				int i = 0;
				while (x > xs[i])
					i++;

				left = i - 1;
				right = i;
			}

			return ys[left] + (ys[right] - ys[left]) * (x - xs[left]) / (xs[right] - xs[left]);
		}
	}
}