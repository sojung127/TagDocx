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
import Escoring as E
import Ascoring as A
import hwptest

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
    A_score = 0

    # 정규표현식으로 특수문자 및 숫자 없앰
    shortword = re.compile(r'[→%‘’▶‟“”…„]')
    doc = shortword.sub('', doc)

    # 토큰화_1차
    tokens = text_to_word_sequence(doc)

    # flag
    N_flag = 1 #notice
    P_flag = 1 #papaer
    L_flag = 1 #lecture
    E_flag = 1 #공고
    A_flag = 1 #all


    N_score = 0
    P_score = 0
    L_score = 0
    E_score = 0
    A_score = 0

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
    if (E_flag == 1):
        if path[-3:] == 'pdf':
            shortword = re.compile("\n")
            doc = shortword.sub('', doc)
            doc = doc.replace("", " ")
        elif path[-3:] == 'txt':
            pass

        E_score = E.Escoring(doc=doc)
    else:
        E_score = 0

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
    if (A_flag == 1):
        A_score = A.Ascoring(doc=doc)
    else:
        A_score = 0

    Score_list = [P_score, N_score, L_score, E_score, A_score]
    # print('문서 저장위치 = ', document_pdf_source)
    #print("논문","기사","수업자료","공고", "지원서")
    #print(Score_list)
    global scan
    global score
    print(Score_list.index(max(Score_list)))

    if Score_list.index(max(Score_list))==scan:
        score+=1




#테스트 해볼 폴더의 분류 입력 [논문:0,기사:1,수업자료:2,공고:3,지원서:4]
scan=int(input('입력 [논문:0,기사:1,수업자료:2,공고:3,지원서:4]'))
score=0
path_origin = input("문서경로:")
file_list = os.listdir(path_origin) #list 반환
#list = ['공10.pdf', '공11.pdf', '공12.pdf', '공13.pdf', '공14.pdf', '공15.pdf', '공16.pdf', '공17.pdf', '공19.pdf', '공2.pdf', '공20.pdf', '공21.pdf', '공22.pdf', '공23.pdf', '공24.pdf', '공3.pdf', '공6.pdf', '공7.pdf', '공8.pdf', '공9.pdf']

file_count = len(file_list)
print("여기 집중")
print(file_count)

for i in range(file_count):
    path = path_origin + file_list[i]
    #print(path)
    #lines = []
    #print(path[-3:] == 'pdf')
    if path[-3:] == 'pdf':
        contents = convert_pdf_to_txt(path)
    elif path[-3:] == 'txt':
        fp = open(path, 'r', encoding='utf-8')
        contents = fp.readlines()
        fp.close()
        contents = ' '.join(contents)

    elif path[-3:] == 'hwp':
        contents = hwptest.convert_hwp_to_txt(path)

    else:
        pass

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
print(score/file_count*100,'%')
