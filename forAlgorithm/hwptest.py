#-*-coding:utf-8-*-
import subprocess
# 4번째 부분에 파일 경로 넣으면 됨
# pyhwp 설치 필요
cmd=['hwp5proc','cat','--vstreams','C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/지원서/입사지원서+양식3.hwp','BodyText/Section0.xml']
fd_popen=subprocess.Popen(cmd,stdout=subprocess.PIPE).stdout
data=fd_popen.read().strip()
fd_popen.close()

from bs4 import BeautifulSoup
bs=BeautifulSoup(data,'xml',from_encoding='utf-8')
# 그냥 출력해도 되는데 파일출력으로 해봤음
fp=open('C:/Users/소정/output.txt','w',encoding='utf-8')
for textElement in bs.findAll('Text'):
    fp.writelines(textElement.text)
fp.close()
