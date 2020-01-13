#-*-coding:utf-8-*-
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO

#한글이 포함되어 있는 PDF 읽기
def convert_pdf_to_txt(path):
    rsrcmgr = PDFResourceManager()
    retstr = StringIO()
    codec = 'utf-8'
    laparams = LAParams()
    device = TextConverter(rsrcmgr, retstr, laparams=laparams)
    fp = open(path, 'rb')
    interpreter = PDFPageInterpreter(rsrcmgr, device)
    password = ""
    maxpages = 0
    caching = True
    pagenos = set()

    for page in PDFPage.get_pages(fp, pagenos, maxpages=maxpages, password=password, caching=caching,
                                  check_extractable=True):
        interpreter.process_page(page)
        text = retstr.getvalue()

    fp.close()
    device.close()
    retstr.close()
    return text
pdf_file = "C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/강의자료/경영10/1차시 PR의 정의와 유형PR의 4모형.pdf"                                     # 로컬 PC에 있는 pdf 파일도 읽을 수 있음
path=input()
contents = convert_pdf_to_txt(path)
lines=contents.split('/n')
'''
print(type(contents))
print(contents)
exit()
'''
scoreList=[]
featureList=[]

fp=open('C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/forAlgorithm/ClassFeature.txt','r',encoding='utf-8-sig')
features = fp.readlines()

fp.close()

for i in range(len(features)):
    scoreList.append(0)
    features[i]=features[i].strip()
    featureList.append(features[i].split())



index=0

sum=0
value=1
import re
for i in range(len(featureList)):
    for line in lines:
        for j in range(1,len(featureList[i])):
            p=re.compile(featureList[i][j])
            print(line)
            if (p.search(line.upper())):
                print('match')
                scoreList[index]=scoreList[index]+1*int(featureList[i][0])
                sum=sum+value
    index=index+1

#pdf_file.close()

print(scoreList)
print(max(scoreList))