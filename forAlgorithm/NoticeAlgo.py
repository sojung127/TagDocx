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

path=input()
lines=[]
print(path[-3:]=='pdf')
if path[-3:]=='pdf':
    contents = convert_pdf_to_txt(path)
   
else:
    fp = open(path,'r',encoding='utf-8')
    contents=fp.readlines()
    fp.close()
fp = open ('forAlgorithm/NoticeFeature.txt','r',encoding='utf-8-sig')
features = fp.readlines()
fp.close()


scoreList=[]#10개
featureList=[]

for i in range(len(features)):
    scoreList.append(0)
    features[i]=features[i].strip()
    featureList.append(features[i].split())

print(featureList)

if path.find('슈퍼루키')>=0:
    print(contents.count('\n'))
    contents=contents.replace('\n','')
print(contents.count('\n'))
index=0
isFind=False

sum=0
value=0

for i in range(len(featureList)):
    value=1
    for w in featureList[i]:
        scoreList[index]+=contents.count(w)
        sum=sum+value
        '''
        if w in contents:
            scoreList[index]=scoreList[index]+1*value
            print(i)
            print(w,value)
            print(scoreList)
             #합값생성
            '''
        
    index=index+1
    isFind=False

print()
print(scoreList)
print(sum)

    

