from collections import namedtuple
from math import pow
from scipy.interpolate import InterpolatedUnivariateSpline
from numpy import arange
import matplotlib
import matplotlib.pyplot as plt

class Params:
	n_p = 1.4
	l = 0.2
	T0 = 300
	sigma = 5.668*1e-12
	F0 = 100
	alpha = 0.05
	h = 1e-4

	fst_table = ((300, 500, 800, 1100, 2000, 2400), 
			(1.36 * pow(10, -2), 1.63 * pow(10, -2), 1.81 * pow(10, -2),
			1.98 * pow(10, -2), 2.50 * pow(10, -2), 2.74 * pow(10, -2)))

	snd_table = ((293, 1278, 1528, 1677, 2000, 2400),
		(2.0 * pow(10, -2), 5.0 * pow(10, -2), 7.8 * pow(10, -2),
		1.0 * pow(10, -1), 1.3 * pow(10, -1), 2.0 * pow(10, -1)),)

params = Params()

class Functions:
	@staticmethod
	def Interpolate(x_pts, y_pts, order=1):
		return InterpolatedUnivariateSpline(x_pts, y_pts, k=order)

	@staticmethod
	def p(k_t, t, n):
		return 4 * params.n_p * params.n_p * params.sigma * k_t(t[n]) * pow(t[n], 3)

	@staticmethod
	def f(k_t, t, n):
		return 4 * params.n_p *params.n_p + params.sigma * k_t(t[n]) * pow(params.T0, 4)

	@staticmethod
	def x_right(l_t, t, n):
		return (l_t(t[n]) + l_t(t[n + 1])) / 2

	@staticmethod
	def x_left(l_t, t, n):
		return (l_t(t[n]) + l_t(t[n - 1])) / 2

	@staticmethod
	def p_right(k_t, t, n):
		return (Functions.p(k_t, t, n) + Functions.p(k_t, t, n + 1)) / 2

	@staticmethod
	def p_left(k_t, t, n):
		return (Functions.p(k_t, t, n) + Functions.p(k_t, t, n - 1)) / 2

	@staticmethod
	def f_right(k_t, t, n):
		return (Functions.f(k_t, t, n) + Functions.f(k_t, t, n + 1)) / 2

	@staticmethod
	def f_left(k_t, t, n):
		return (Functions.f(k_t, t, n) + Functions.f(k_t, t, n - 1)) / 2

	@staticmethod
	def A(l_t, t, n):
		return (l_t(t[n]) + l_t(t[n - 1])) / 2 / params.h

	@staticmethod
	def B(l_t, k_t, t, n):
		return Functions.A(l_t, t, n) + Functions.C(l_t, t, n) + 4 * params.n_p * params.n_p * params.sigma * k_t(t[n]) * pow(t[n], 3) * params.h

	@staticmethod
	def C(l_t, t, n):
		return (l_t(t[n]) + l_t(t[n + 1])) / 2 / params.h

	@staticmethod
	def D(k_t, t, n):
		return 4 * params.n_p * params.n_p + params.sigma * k_t(t[n]) * pow(params.T0, 4) * params.h


def GetRightConditions(l_t, k_t, t):
	K0 = Functions.x_right(l_t, t, 0) + pow(params.h, 2) / 8 * Functions.p_right(k_t, t, 0) + pow(params.h, 2) / 4 * Functions.p(k_t, t, 0)
	M0 = pow(params.h, 2) / 8 * Functions.p_right(k_t, t, 0) - Functions.x_right(l_t, t, 0)
	P0 = params.h * params.F0 + pow(params.h, 2) / 4 * (Functions.f_right(k_t, t, 0) + Functions.f_left(k_t, t, 0))
	return K0, M0, P0


def GetLeftConditions(k_t, l_t, t, n):
	Kn = Functions.x_left(l_t, t, n) / params.h - params.alpha - params.h * Functions.p(k_t, t, n) / 4 - params.h * Functions.p_left(k_t, t, n) / 8
	Mn = Functions.x_left(l_t, t, n) / params.h - params.h * Functions.p_left(k_t, t, n) / 8
	Pn = -(params.alpha * params.T0 + (Functions.f_right(k_t, t, n) + Functions.f_left(k_t, t, n)) / 4 * params.h)
	return Kn, Mn, Pn


def SaveImg(xs, ys, name_x, name_y, file_name):
	fig, ax = plt.subplots()
	ax.plot(xs, ys)
	ax.grid()
	ax.set_xlabel(name_x)
	ax.set_ylabel(name_y)
	plt.savefig(file_name, bbox_inches="tight")

def Main():
	l_t = Functions.Interpolate(params.fst_table[0], params.fst_table[1])
	k_t = Functions.Interpolate(params.snd_table[0], params.snd_table[1])
	t = [0 for _ in range(int(1 / params.h) + 2)]
	K0, M0, P0 = GetRightConditions(l_t, k_t, t)
	xi_list = [0]
	eta_list = [0]
	x_list = list()
	x = 0
	n = 0
	while x + params.h < 1:
		x_list.append(x)
		xi_list.append(Functions.C(l_t, t, n) / (Functions.B(l_t, k_t, t, n) - Functions.A(l_t, t, n) * xi_list[n]))
		eta_list.append((Functions.D(k_t, t, n) + Functions.A(l_t, t, n) * xi_list[n]) / (Functions.B(l_t, k_t, t, n) - Functions.A(l_t, t, n) * xi_list[n]))
		n += 1
		x += params.h
	x_list.extend([x + params.h, x + params.h * 2])
	Kn, Mn, Pn = GetLeftConditions(k_t, l_t, t, n)
	t[n] = (Pn - Mn * xi_list[n]) / (Kn + Mn * xi_list[n])
	for i in range(n - 1, -1, -1):
		t[i] = xi_list[i + 1] * t[i + 1] + eta_list[i + 1]
	SaveImg(x_list, t, "l", "T", "img_1.png")


if __name__ == "__main__":
	Main()