import olefile

test_file=olefile.OleFileIO('2020년 아산다솜장학생 선발안내.hwp')

# encoded_text = test_file.openstream('PrvText').read()

# decoded_text=encoded_text.decode('UTF-16')

# print(decoded_text)

print(test_file.listdir())