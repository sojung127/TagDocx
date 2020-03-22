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
import Nscoring as N
import Pscoring as P
import Escoring as E
import Ascoring as A
import hwptest
import docx2txt
import Total_Scoring #형식태그
import ContentTagging #내용태그
from db_utils import *
import argparse


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

def get_tag(path):
    # 폴더에서 문서 목록 읽어오기
    # 입력 예시   ../Dataset/기사/0전체폴더/
    #path_origin = input("문서경로:")
    path_origin = path + "\\"

    file_list = os.listdir(path_origin)

    file_count = len(file_list)

    print("여기 집중")

    for i in range(file_count):

        path = path_origin + file_list[i]
        print(path)

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
        content_tag =  ContentTagging.content_tagging(contents, path) #list 반환
        print(content_tag)

        doc = contents

        # 각 문서당 형식태그를 할당한다
        type_score = Total_Scoring.scoring(doc=doc, path=path)
        index = type_score.index(max(type_score))

        #    Score_list = [P_score, N_score, L_score, E_score, A_score]
        form_tag = form_tagging(index) #string 값 반환
        print(form_tag)

        # # create_db()
        # create_table_content()
        # create_table_document()
        # insert_document_tag(path, form_tag)
        # insert_content_tag(path, content_tag)

def run():
    argv_list = sys.argv

    for i in range(len(argv_list)-1):
        get_tag(argv_list[i+1])
    print(argv_list)

# print(index)
def form_tagging(index):
    return {0: '논문', 1: '기사', 2: '강의자료', 3: 'E', 4: 'A'}.get(index, '기타')

if __name__ == '__main__':
    sys.exit(run())