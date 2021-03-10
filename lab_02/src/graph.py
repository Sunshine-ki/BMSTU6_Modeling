import matplotlib
import matplotlib.pyplot as plt

a = [1, 2]
b = [3, 4]

fig, ax = plt.subplots()

ax.plot(a, b, label="I(t)")

ax.grid()
ax.set_xlabel("t")
ax.set_ylabel("I")

plt.savefig("name.png", bbox_inches="tight")

print(a)