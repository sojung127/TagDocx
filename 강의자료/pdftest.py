
'''
from pdfminer.pdfinterp import PDFResourceManager, process_pdf
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from io import StringIO
from io import open
from urllib.request import urlopen

def read_pdf_file(pdfFile):

    rsrcmgr = PDFResourceManager()
    retstr = StringIO()
    laparams = LAParams()
    device = TextConverter(rsrcmgr, retstr, laparams=laparams)

    process_pdf(rsrcmgr, device, pdfFile)
    device.close()

    content = retstr.getvalue()
    retstr.close()
    return content

# logging.propagate = False 
# logging.getLogger().setLevel(logging.ERROR)

pdf_file = open("ch06 Counting.pdf", "rb")                                       # 로컬 PC에 있는 pdf 파일도 읽을 수 있음
contents = read_pdf_file(pdf_file)
lines=contents.split('\n')

'''
# 좀 더 짧은 코드로 pdf 파일을 읽을 수 있음
# 파일을 페이지 별로 읽음 
# 위의 방법으로 읽히지 않는 pdf파일 중 몇개의 파일이 읽힘
import PyPDF2

pdf_file = open("ch06 Counting.pdf", "rb")

pdfReader = PyPDF2.PdfFileReader(pdf_file)
count = pdfReader.numPages
for i in range(count):
    page = pdfReader.getPage(i)
    print(page.extractText())


fp=open('ClassFeature.txt','r',encoding='utf-8-sig')
features = fp.readlines()
fp.close()

scoreList=[]
featureList=[]

for i in range(len(features)):
    scoreList.append(0)
    features[i]=features[i].strip()
    featureList.append(features[i].split())

for i in range(len(lines)):
    lines[i]=lines[i].replace(" ","")

index=0

sum=0
value=1

for i in range(len(featureList)):
    for line in lines:
        for w in featureList[i]:
            if w in line.upper():
                scoreList[index]=scoreList[index]+1*value
                sum=sum+value
    index=index+1

pdf_file.close()

print(scoreList)
print(max(scoreList))