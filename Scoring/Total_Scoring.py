from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO
import os
import re
import Nscoring as N
import Pscoring as P

# 한글이 포함되어 있는 PDF 읽기
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

def scoring(doc):
    # 점수
    N_score = 0
    P_score = 0
    L_score = 0
    E_score = 0

    # 정규표현식으로 특수문자 및 숫자 없앰
    shortword = re.compile(r'[→%‘’▶‟“”…„]')
    doc = shortword.sub('', doc)

    # 토큰화_1차
    tokens = text_to_word_sequence(doc)

    # flag
    N_flag = 1
    P_flag = 1
    L_flag = 1
    E_flag = 1


    N_score = 0
    P_score = 0
    L_score = 0
    E_score = 0

    Length = len(tokens)

    ### 문서가 3000자 이상일 경우 뉴스일 가능성을 제외한다.###
    if (Length > 3000): N_flag = 0
    # print('length , flag:',Length, N_flag)

    # 논문 scoring
    if (P_flag == 1):
        P_score = P.Pscoring(doc=doc)
        '''
        #############이 코드는 논문이 영어일때만 고려!!!########
        shortword = re.compile(r'\W*\b\w{1,2}\b')
        doc = shortword.sub('', doc)

        # 토큰화_2차
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

        P_score = NP_scoring(r'.\StandardWords\mapping.csv')
        P_score = NP_scoring(r'..\forAlgorithm\Pmapping.csv')
        print("P_score: ", P_score)
        '''

    else:
        P_score = 0

    # 뉴스 scoring
    if (N_flag == 1):
        N_score = N.Nscoring(doc=doc)
    else:
        N_score = 0

    class_doc = doc.replace(" ", "")

    # 강의자료 scoring
    if (L_flag == 1):
        # 강의자료 특성을 작성한 txt파일을 불러와서 L_score 계산한다.
        fp = open(r'.\ClassFeature.txt', 'r', encoding='utf-8-sig')
        features = fp.readlines()
        fp.close()

        scoreList = []
        featureList = []

        for i in range(len(features)):
            scoreList.append(0)
            features[i] = features[i].strip()
            featureList.append(features[i].split())

        index = 0

        sum = 0
        value = 1

        for i in range(len(featureList)):
            for w in featureList[i]:
                if w in class_doc.upper():
                    scoreList[index] = scoreList[index] + 1 * value
                    sum = sum + value
            index = index + 1
        #total_score_list.append(scoreList)
        L_score = max(scoreList)
        #print("L_score: ", L_score)
    else:
        L_score = 0

    # 공고문 scoring
    if (E_flag == 1):
        '''
        path=input()
        lines=[]
        print(path[-3:]=='pdf')
        if path[-3:]=='pdf':
            contents = convert_pdf_to_txt(path)

        else:
            fp = open(path,'r',encoding='utf-8')
            contents=fp.readlines()
            fp.close()
            '''
        fp = open('./NoticeFeature.txt', 'r', encoding='utf-8-sig')
        features = fp.readlines()
        fp.close()

        scoreList = []  # 10개
        featureList = []

        for i in range(len(features)):
            scoreList.append(0)
            features[i] = features[i].strip()
            featureList.append(features[i].split())

        # print(featureList)

        if path.find('슈퍼루키') >= 0:
            #print(doc.count('\n'))
            doc = doc.replace('\n', '')
        #print(contents.count('\n'))
        index = 0
        #isFind = False

        sum = 0
        value = 0

        for i in range(len(featureList)):
            value = 1
            for w in featureList[i]:
                if path[-3:] == 'txt':
                    for content in doc:
                        scoreList[index] += content.count(w)
                        sum += content.count(w)
                else:
                    scoreList[index] += doc.count(w)
                    sum += doc.count(w)
                '''
                if w in contents:
                    scoreList[index]=scoreList[index]+1*value
                    print(i)
                    print(w,value)
                    print(scoreList)
                    #합값생성
                    '''

            index = index + 1
            #isFind = False

        E_score = sum
    else:
        E_score = 0

    Score_list = [P_score, N_score, L_score, E_score]
    # print('문서 저장위치 = ', document_pdf_source)
    print(Score_list)
    
    
path_origin = input("문서경로:")
file_list = os.listdir(path_origin)
#print(file_list)

for i in range(5):
    path = path_origin + file_list[i]
    print(path)
    #lines = []
    print(path[-3:] == 'pdf')
    if path[-3:] == 'pdf':
        contents = convert_pdf_to_txt(path)

    else:
        fp = open(path, 'r', encoding='utf-8')
        contents = fp.readlines()
        fp.close()

    doc = contents

    #
    scoring(doc=doc)
    
    total_score_list = []
    # 검사하는 문서를 불러오는 코드 -txt version
    '''
    #txt
    document_txt_source = "C:\\capston\\AutomaticFileCategorizeService\\공고\\E1.txt"
    doc = open(document_txt_source,mode='rt', encoding="utf-8").read()

    '''

    
