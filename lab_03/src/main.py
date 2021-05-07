import numpy as np
from scipy import integrate
from scipy.interpolate import InterpolatedUnivariateSpline
import matplotlib.pyplot as plt

class Params:
    F0 = 100
    T0 = 300
    l = 0.2
    h = 1e-4
    np_coef = 1.4
    alpha = 0.05
    sigma = 5.668e-12
    tl_tab = ((300, 500, 800, 1100, 2000, 2400),
        (1.36e-2, 1.63e-2, 1.81e-2, 1.98e-2, 2.50e-2, 2.74e-2))
    tk_tab = ((293, 1278, 1528, 1677, 2000, 2400),
        (2.0e-2, 5.0e-2, 7.8e-2, 1.0e-1, 1.3e-1, 2.0e-1))

params = Params()
get_lambda = InterpolatedUnivariateSpline(params.tl_tab[0], params.tl_tab[1], k=1)
get_k = InterpolatedUnivariateSpline(params.tk_tab[0], params.tk_tab[1], k=1)

def p(T):
    return 0

def f(T):
    return -4 * params.np_coef**2 * params.sigma * get_k(T) * \
                 (pow(T, 4)-pow(params.T0, 4))

def LeftConditions(ts):
    h2 = params.h * params.h
    lm = (get_lambda(ts[0]) + get_lambda(ts[1])) / 2
    fm = (f(ts[0]) + f(ts[1])) / 2
    K0 = lm
    M0 = -lm
    P0 = params.h * params.F0 + (h2 / 4) * (f(ts[0]) + fm)
    return K0, M0, P0

def RightConditions(ts):
    KN = get_lambda(ts[-1])
    MN = params.alpha * params.h + get_lambda(ts[-1])
    PN = params.alpha * params.T0 * params.h
    return KN, MN, PN

def RunMethod(A, B, C, D, K0, M0, P0, KN, MN, PN): 
    n = len(A)
    xi = [-M0/K0]
    eta = [P0/K0]
    for i in range(len(A)):
        x = C[i]/(B[i]-A[i] * xi[i])
        e = (D[i]+A[i] * eta[i])/(B[i]-A[i] * xi[i])
        xi.append(x)
        eta.append(e)
    y = [(PN-KN * eta[n])/(MN+KN * xi[n])]
    for i in range(len(A), -1, -1):
        y_i = xi[i] *  y[0]+eta[i]
        y.insert(0, y_i)
    return np.array(y)

def CalcCoefficients(ts):
    K0, M0, P0 = LeftConditions(ts)
    KN, MN, PN = RightConditions(ts)
    N = int(params.l/params.h)
    A, B, C, D = [0]*(N-1), [0]*(N-1), [0]*(N-1), [0]*(N-1)
    for n in range(1, N):
        lam_n_m_1 = get_lambda(ts[n-1])
        lam_n = get_lambda(ts[n])
        lam_n_p_1 = get_lambda(ts[n+1])
        A[n-1] = (lam_n_m_1+lam_n)/2/params.h
        C[n-1] = (lam_n+lam_n_p_1)/2/params.h
        B[n-1] = A[n-1]+C[n-1]+p(ts[n]) * params.h
        D[n-1] = f(ts[n]) * params.h
    return A, B, C, D, K0, M0, P0, KN, MN, PN

def f1(T):
    return params.F0-params.alpha * (T[-1]-params.T0)

def f2 (ts):
    return  4 * params.np_coef**2 * params.sigma * integrate.trapezoid([get_k(ti) * \
             (pow(ti, 4) - pow(params.T0, 4)) for ti in ts], np.arange(0, params.l + params.h, params.h))

def SimpleIterations(alpha, eps1, eps2, maxIter):
    N = int(params.l/params.h)
    cnt = 0 
    T_sp = np.array([params.T0] * (N+1), dtype=float)
    A, B, C, D, K0, M0, P0, KN, MN, PN = CalcCoefficients(T_sp)
    T_s = RunMethod(A, B, C, D, K0, M0, P0, KN, MN, PN)
    while np.max(abs((f1(T_s)-f2(T_s))/f1(T_s))) > eps2 and np.max(abs((T_s-T_sp)/T_s)) > eps1 and cnt < maxIter:
        T_sp = T_s
        A, B, C, D, K0, M0, P0, KN, MN, PN = CalcCoefficients(T_sp)
        new_Ts_norelax = RunMethod(A, B, C, D, K0, M0, P0, KN, MN, PN)
        T_s = T_sp+params.alpha * (new_Ts_norelax-T_sp)
        cnt += 1
    print(f"Кол-во итераций: {cnt}")
    print(f"Максимальная абсолютная разность решений: {np.max(abs((T_s-T_sp)/T_s))}")
    print(f"Баланс энергии (|(f2-f1)/f1|): {np.max(abs((f1(T_s)-f2(T_s))/f1(T_s)))}")
    print(f"f1 = {f1(T_s)}")
    print(f"f2 = {f2(T_s)}")
    return T_s

def SaveImg(xs, ys, name_x, name_y, file_name):
    fig, ax = plt.subplots()
    ax.plot(xs, ys)
    ax.grid()
    ax.set_xlabel(name_x)
    ax.set_ylabel(name_y)

    # params.alpha = 0.3
    # x = np.arange(0, params.l+params.h, params.h)
    # T = SimpleIterations(0.25, 1e-5, 1e-3, 100)
    # ax.plot(x, T,  color = 'green')

    plt.savefig(file_name, bbox_inches="tight") # color = 'green'

def main():
    x = np.arange(0, params.l+params.h, params.h)
    T = SimpleIterations(0.25, 1e-5, 1e-3, 300)
    SaveImg(x, T, '', '', 'tmp2.png')
    # plt.plot(x, T, label='$T_{\\params.alpha}$')

    #params.alpha *= 3
    #T = SimpleIterations(0.25, 1e-5, 1e-3, 100)
    #plt.plot(x, T, label='$T_{3 \\params.alpha}$')

    # plt.ylabel('T, K')
    # plt.xlabel('x, см')
    # plt.legend()
    # plt.show()

if __name__ == "__main__":
    main()