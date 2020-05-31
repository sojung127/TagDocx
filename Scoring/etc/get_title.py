import sys
import title_test as T
import os

def get_title():
    path_origin = input("문서경로:")
    file_list = os.listdir(path_origin) #list 반환

    file_count = len(file_list)

    for i in range(file_count):
        path = path_origin + file_list[i]
        print(path)
        T.run(path)
        print('\n')   

if __name__ == '__main__':
    sys.exit(get_title())