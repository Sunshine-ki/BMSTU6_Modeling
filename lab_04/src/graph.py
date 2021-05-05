import matplotlib
import matplotlib.pyplot as plt


def Reader(file_name):
	arr = list()

	with open(file_name) as f:
		for line in f:
			arr.append(float(line))

	return arr


def SaveImg(xs, array_ys, name_x, name_y, file_name):
	fig, ax = plt.subplots()

	for ys in array_ys:
		ax.plot(xs, ys) #, label="I(t)")

	ax.grid()
	ax.set_xlabel(name_x)
	ax.set_ylabel(name_y)

	plt.savefig(file_name, bbox_inches="tight")


def main():
	xs = Reader("results/data/xs_2.txt")

	array_ys = list()
	for i in range(17):
		ys = Reader(f"results/data/task_2_{i}.txt")
		array_ys.append(ys)
	SaveImg(xs, array_ys, "T, к", "x, см", "results/img/2.png") 


if __name__ == '__main__':
	main()

