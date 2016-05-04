from rikeripsum import rikeripsum
import random

#alan, ward, martin
if __name__ == '__main__':
	names = ['Ward','Alan','Martin']
	with open('bigtweets.txt','w+') as f:
		for i in range(1,1000000):
			t = rikeripsum.generate_sentence()
			f.write("{}> {}\n".format(random.choice(names), t))
			
		
