#-*- coding: utf-8 -*-
import pandas as pd
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO
import sys
import os
import re
import hwptest
import docx2txt
import argparse
import pickle
import joblib
from sklearn.linear_model import SGDClassifier
from sklearn import *
import ContentTagging #내용태그


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

def get_tag2(filepath):
    filepath = filepath[:-1]
    name = filepath.split("\\")[-1]
    result_path = filepath[:-((len(filepath.split("\\")[-1]))+1)]

    '''
    result_path = path

    path_origin = path + "\\"

    file_list = os.listdir(path_origin)

    file_count = len(file_list)
    '''
    score = []
    
    path = filepath

    #print(path[-4:-1])
    contents = ""

    if path[-3:] == 'pdf':
        contents = convert_pdf_to_txt(path)
        shortword = re.compile("\n")
        contents = shortword.sub('', contents)
        contents = contents.replace("", " ")
    elif path[-3:] == 'txt':
        fp = open(path, 'r', encoding='utf-8')
        contents = fp.readlines()
        fp.close()
        contents = ' '.join(contents)

    elif path[-3:] == 'hwp':
        contents = hwptest.convert_hwp_to_txt(path)
    elif path[-3:] == 'docx':
        contents = docx2txt.process(path)
    else:
        return None
    # 각 문서당 내용태그를 할당한다
    
    doc = contents
    
    for word in train:
        score.append(doc.count(word))

    #지원서
    A_score = 0
    reg1 = '지\s+원\s+서'
    reg2 = '자\s+기\s+소\s+개\s+서'
    reg3 = '이\s+력\s+서'
    
    regs = [reg1, reg2, reg3]

    for reg in regs:
        result = re.findall(reg, doc)
        for i in range(len(result)):
            A_score += 1

    score.append(A_score)

    #지원서
    w = ['사진', '기자', '뉴스']

    # 괄호 안에 있는 단어들 출력
    items = re.findall('\(([^)]+)', doc)  # ()괄호 안에 있는 단어 인식
    items.append(doc[doc.find("[") + 1: doc.find("]")])  # []괄호 안에 있는 단어 인식
    # print(items)

    N_score = 0

    for item in items:
        for word in w:
            if word in item:
                N_score += 1
    
    score.append(N_score)

    #논문
    P_score = 0

    words = ['게재 결정', '결론 및 논의', '글을 마치며' '나가는 말', '논문 게재',
    '논문 발표', '들어가는 말','심사 일자', '연구 결과', '연구 방법'
    , '요약 및 결론','논문 접수', '논문 심사', '게재 확정']

    for word in words:
        if word in doc:
            P_score += 1

    reg1 = '그림\s+\d+'
    reg2 = '표\s+\d+'
    reg3 = 'Fig\s+\d+'
    reg4 = '\(대학교.+학과\)'
    reg5 = '결\s+론'
    reg6 = '서\s+론'
    reg7 = '목\s+차'
    reg8 = '차\s+례'

    regs = [reg1, reg2, reg3, reg4, reg5, reg6, reg7, reg8]

    for reg in regs:
        result = re.findall(reg, doc)
        #print(result)
        for i in range(len(result)):
            P_score += 1

    score.append(P_score)

    index = model.predict([score])[0]
    form_tag = form_tagging(index) #string 값 반환

    content_tag =  ContentTagging.content_tagging(contents, path) #list 반환
    print("<GET",result_path,"><GET",form_tag,"><GET",content_tag,"><GET",name,">")
    

def form_tagging(index):
    return {0: '기사', 1: '논문', 2: '강의자료', 3: '공고', 4: 'A'}.get(index, '기타')


def run():
    argv_list = sys.argv

    import pandas as pd
    global train
    train = pd.read_csv(r'C:\\Users\\YooJin\\Desktop\\AutomaticDocumentClassificationService\\test\\words.csv', encoding='CP949')
    #train = pd.read_csv(r'C:\capston\AutomaticDocumentClassificationService\test\words.csv', encoding='CP949')
    train_words = train['words'].dropna().tolist()
    #global train
    train = []

    for i in range(len(train_words)):
        a = train_words.pop()
        train.append(a)

    filename = './sgd_classifier.pkl'
    global model
    model = joblib.load(filename)
    '''
    folderpath = argv_list[1]


    for i in range(len(argv_list)):
        if i<1:
            pass
        else:
            #print(argv_list[i])
            ###########################
            #get_tag(folderpath, argv_list[i+1])
            get_tag2(argv_list[i])
    '''
    f = open(r"C:\Users\YooJin\Desktop\AutomaticDocumentClassificationService\filelist.txt", 'r', encoding='utf-8')
    lines = f.readlines()
    for line in lines:
        get_tag2(line)
    f.close()


def test():
    argv_list = sys.argv
    test_string = argv_list[1]
    print(test_string[:-((len(test_string.split("\\")[-1]))+1)])
    #print(test_string)

if __name__ == '__main__':
    sys.exit(run())
    #sys.exit(test())

'''
filename = '/tmp/digits_classifier.joblib.pkl'

model = joblib.load(filename)

model.predict()
'''