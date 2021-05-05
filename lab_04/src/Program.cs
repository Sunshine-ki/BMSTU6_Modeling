using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace src
{
    class Program
    {
        static void SaveData(List<double> res, string fileName)
        {
			FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			StreamWriter writer = new StreamWriter(file);

            foreach (var elem in res)
            {
				writer.WriteLine(elem);
            }

			writer.Close();
			file.Close();
        }

        static void Task()
        {
			List<double> T = Enumerable.Range(1, (int)(Constants.l / Constants.h) + 1).Select(x => (double)Constants.T0).ToList();
			List<double> TNew = Enumerable.Range(1, (int)(Constants.l / Constants.h) + 1).Select(x => 0d).ToList();
			List<double> TPrev = new List<double>();

			double currentMax = 1d, ti = 0;

			bool epsilonCondition = true;
			int file_i = 0;
            
            SaveData(T, $"results/data/task_1_{file_i++}.txt");

			while (epsilonCondition)
            {
				TPrev = T;
                currentMax = 1d;

                while (currentMax >= 1)
                {
                    TNew = Conditions.GetNewT(T);
					currentMax = Math.Abs((T[0] - TNew[0]) / TNew[0]);

                    var arr_d = T.Zip(TNew, (T_i, Tnew_i) => Math.Abs(T_i - Tnew_i) / Tnew_i);
					currentMax = arr_d.Max();

					TPrev = TNew;
				}

    			SaveData(TNew, $"results/data/task_1_{file_i++}.txt");

				ti += Constants.t;

				epsilonCondition = false;
                int flag = T.Zip(TNew, (T_i, Tnew_i) => Math.Abs(T_i - Tnew_i) / Tnew_i).Count(elem => elem > Constants.eps);
                if (flag != 0)
                    epsilonCondition = true;

				T = TNew;

    			// List<double> ys = new List<double>();
			    // for (double s = Constants.l / 3d; s > 0 ; s -= 0.1)
                // {
				// 	Console.WriteLine(s);
				// 	ys.Add(TNew[(int)(s / Constants.h)]);
				// }
    			// SaveData(ys, $"results/data/task_2_{file_i++}.txt");
			}
		}

        static void Main(string[] args)
        {
			// Output with point.
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Task();
		}
    }
}
