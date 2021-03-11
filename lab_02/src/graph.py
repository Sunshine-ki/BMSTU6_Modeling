import matplotlib
import matplotlib.pyplot as plt


def Reader(file_name):
	arr = list()

	with open(file_name) as f:
		for line in f:
			arr.append(float(line))

	return arr


def SaveImg(xs, ys, name_x, name_y, file_name):
	fig, ax = plt.subplots()

	ax.plot(xs, ys, label="I(t)")

	ax.grid()
	ax.set_xlabel(name_x)
	ax.set_ylabel(name_y)

	plt.savefig(file_name, bbox_inches="tight")


def main():
	t = Reader("results/data/t.txt")
	I = Reader("results/data/I.txt")
	U = Reader("results/data/U.txt")

	SaveImg(t, I, "t", "I", "results/img/I.png")
	SaveImg(t, U, "t", "U", "results/img/U.png")


if __name__ == '__main__':
	main()

