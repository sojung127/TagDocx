import olefile
test_file=olefile.OleFileIO('2020년 아산다솜장학생 선발안내.hwp')
encoded_text = test_file.openstream('PrvText').readlines()
#lines=encoded_text.decode('UTF-16le').split('\r \n')
for line in encoded_text:
    print(line.decode('utf-16le'))
