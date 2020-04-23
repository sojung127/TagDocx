#-*- coding: utf-8 -*-
from tensorflow.keras.preprocessing.text import text_to_word_sequence
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


def get_tag():
    # 폴더에서 문서 목록 읽어오기
    # 입력 예시   ../Dataset/기사/0전체폴더/
    #path_origin = input("문서경로:")
    #paths = ["../Dataset/기사/0전체폴더"]
    paths = ["../Dataset/기사/0전체폴더","../Dataset/논문/0전체","../Dataset/공고","../Dataset/지원서"]
    paths = ["../Dataset/기사/기사test","../Dataset/논문/논문test2","../Dataset/지원서test"]

    import pandas as pd
    train = pd.read_csv(r'C:\\Users\\YooJin\\Desktop\\AutomaticDocumentClassificationService\\test\\words.csv', encoding='CP949')
    train_words = train['words'].dropna().tolist()
    train =[]

    for i in range(len(train_words)):
        a = train_words.pop()
        train.append(a)

    results = []

    #폴더마다
    for p in paths:
        path_origin = p + "\\"
        file_list = os.listdir(path_origin)
        file_count = len(file_list)

        #폴더의 파일마다
        for i in range(file_count):
            
            score = []
            
            name = file_list[i]
            path = path_origin + name
            #print(path)

            if os.path.isdir(path):
                continue
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
            elif path[-4:] == 'docx':
                contents = docx2txt.process(path)

            else:
                contents = ''
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
            '''
            reg9 = ".+\(\d+\),\s?.+,\s?.+,\s?.+,\s?.+쪽"
            reg10 = ".+\.\s?\(\d+\)\.\s?.+,\s?.+권.+,\s?pp\."
            '''

            regs = [reg1, reg2, reg3, reg4, reg5, reg6, reg7, reg8]

            for reg in regs:
                result = re.findall(reg, doc)
                #print(result)
                for i in range(len(result)):
                    P_score += 1

            score.append(P_score)

            results.append((paths.index(p), score))
            #print((paths.index(p), score))
            #print(score)
    f = open('test.pickle', 'wb')
    pickle.dump(results, f)
    f.close()
    quit()
    #print(results)

# print(index)
def form_tagging(index):
    return {0: '논문', 1: '기사', 2: '강의자료', 3: '공고', 4: 'A'}.get(index, '기타')

def get_pickle():
    f = open('test.pickle', 'rb')
    my_list = pickle.load(f)
    print(my_list)
    f.close()

if __name__ == '__main__':
    #sys.exit(get_tag())
    sys.exit(get_pickle())