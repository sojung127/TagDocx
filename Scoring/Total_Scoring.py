from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd

'''
#txt
doc = open("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\E3.txt",mode='rt', encoding="utf-8").read()

'''
'''
#docx
import docx2txt
doc = docx2txt.process("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\Letter.docx")
'''
'''
#doc
import Document
doc = Document.open("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\Letter.docx")
'''
'''
#pdf
from tika import parser
doc = parser.from_file("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\N1.pdf")
doc = doc["content"]
print(doc)
'''

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

#기사 Scoring
def NP_scoring(path):
    global tokens_NP

    #r'C:\\capston\\test\\Nmapping.csv'
    #기사 mapping 기준 단어 csv파일 불러오기
    train = pd.read_csv(path)

    train_words = train['words'].dropna().tolist() #words 열 읽기
    train_score = train['score'].dropna().tolist() #score 열 읽기

    train = {} #dictionary

    score = 0

    for i in range(len(train_words)):
        a = train_words.pop()
        b = train_score.pop()
        train.update({a: b})

    for x in tokens_NP:
        if x in train:
            #print(x)
            score += train.get(x)

    return score
   # print(s / Length)



#검사하는 문서를 불러오는 코드 - pdf version
document_pdf_source = "Dataset\논문\공학\개인정보 노출에 대한 인터넷 사용자의 태도에 관한 연구.pdf"
extracted_text = convert_pdf_to_txt(document_pdf_source)
#print(type(extracted_text))
doc = extracted_text

total_score_list=[]
#검사하는 문서를 불러오는 코드 -txt version
'''
#txt
document_txt_source = "C:\\capston\\AutomaticFileCategorizeService\\공고\\E1.txt"
doc = open(document_txt_source,mode='rt', encoding="utf-8").read()

'''



#점수
N_score = 0
P_score = 0
L_score = 0
E_score = 0

import re

#정규표현식으로 특수문자 및 숫자 없앰
shortword = re.compile(r'[→%"‘’▶‟“”…„0-9]')
doc = shortword.sub('', doc)

#토큰화_1차
tokens = text_to_word_sequence(doc)

#flag
N_flag = 1
P_flag = 1
L_flag = 1

N_score = 0
P_score = 0
L_score = 0
E_score = 0


Length = len(tokens)

### 문서가 3000자 이상일 경우 뉴스일 가능성을 제외한다.###
if (Length > 3000) : N_flag = 0
#print('length , flag:',Length, N_flag)


# 논문 scoring
if(P_flag == 1):
    #############이 코드는 논문이 영어일때만 고려!!!########
    shortword = re.compile(r'\W*\b\w{1,2}\b')
    doc = shortword.sub('', doc)

    #토큰화_2차
    tokens_NP = text_to_word_sequence(doc)
    
    # 불용어 처리
    from nltk.corpus import stopwords

    stop_words = set(stopwords.words('english'))

    result = []
    for w in tokens_NP:
        if w not in stop_words:
            result.append(w)

    tokens_NP = result

    # 표제어 추출
    # 괜찮 +n.lemmatize('dies', 'v')이런식으로 단어가 동사 품사라는 사실을 알려줄 수 있음.
    from nltk.stem import WordNetLemmatizer

    n = WordNetLemmatizer()
    tokens_NP = [n.lemmatize(w) for w in tokens_NP]

    P_score = NP_scoring(r'Scoring\StandardWords\mapping.csv')
    P_score = NP_scoring('forAlgorithm\Pmapping.xlsx')
    print("P_score: ", P_score)

else:
    P_score = 0

#뉴스 scoring
if(N_flag == 1):
    w = ['사진', '기자', '뉴스']


    #괄호 안에 있는 단어들 출력
    items = re.findall('\(([^)]+)', doc)  # ()괄호 안에 있는 단어 인식
    items.append(doc[doc.find("[") + 1: doc.find("]")])  # []괄호 안에 있는 단어 인식
    #print(items)

    for item in items:
        for word in w:
            if word in item:
                N_score += 5
                #print(word)

    #중복처리한 토큰들
    tokens_NP = list(set(tokens))

    N_score += NP_scoring(r'Scoring\StandardWords\mapping.csv')
    N_score += NP_scoring('C:\\capston\\test\\Nmapping.csv')
    print("N_score: ", N_score)
   
    
    print(N_score)
else:
    N_score = 0

class_doc = doc.replace(" ","")

# 강의자료 scoring
if (L_flag == 1):
    # 강의자료 특성을 작성한 txt파일을 불러와서 L_score 계산한다.
    fp=open('Scoring\ClassFeature.txt','r',encoding='utf-8-sig')
    features = fp.readlines()
    fp.close()

    scoreList=[]
    featureList=[]

    for i in range(len(features)):
        scoreList.append(0)
        features[i]=features[i].strip()
        featureList.append(features[i].split())


    index=0

    sum=0
    value=1

    for i in range(len(featureList)):
        for w in featureList[i]:
                if w in class_doc.upper():
                    scoreList[index]=scoreList[index]+1*value
                    sum=sum+value
        index=index+1
    total_score_list.append(scoreList)
    L_score = max(scoreList)
    print("L_score: ", L_score)
else:
    pass

# 공고문 scoring
fp = open ('../forAlgorithm/NoticeFeature.txt','r',encoding='utf-8')
features = fp.readlines()
fp.close()
#print("notice start")

scoreList=[]#10개
featureList=[]

for i in range(len(features)):
    scoreList.append(0)
    features[i]=features[i].strip()
    featureList.append(features[i].split())

#print(featureList)

### 각 문서의 형식에 부합하는 지 계산한 점수를 출력하는 코드 ###
index=0


sum=0
value=0
for i in range(len(featureList)):
    if i == 0:
        value=5
    elif i == 1:
        value=3
    else:
        value=1
    
    for w in featureList[i]:
        if w in class_doc:
            scoreList[index]=scoreList[index]+1*value
            sum=sum+value #합값생성
    index=index+1
total_score_list.append(scoreList)
E_score = sum
print("E_score: ", E_score)


Score_list = [P_score, N_score, L_score, E_score]
print('문서 저장위치 = ', document_pdf_source)
print(Score_list)




