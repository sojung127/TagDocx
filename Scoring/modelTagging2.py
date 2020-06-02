#-*- coding: utf-8 -*-

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
from sklearn import feature_extraction
import ContentTagging #내용태그
import numpy


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

from konlpy.tag import Kkma
def get_tag(folderpath, filename):

    result_path = folderpath
    '''
    result_path = path

    path_origin = path + "\\"

    file_list = os.listdir(path_origin)

    file_count = len(file_list)
    '''
    score = []
    
    path = folderpath+"\\"+filename

    if path[-3:] == 'pdf':
        contents = convert_pdf_to_txt(path)
        shortword = re.compile("\n")
        contents = shortword.sub('', contents)
        contents = contents.replace(" ", " ")
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
        pass
    # 각 문서당 내용태그를 할당한다

    doc = contents

    post=[]
    post.append(contents)
    if (contents!=''):
        try:
            #문서를 읽어서 벡터를 만드는 과정에 오류가 없는지 확인하는 과정
            ' '.join(pos_tagger.morphs(contents))
            # 위의 코드들이 정상 실행되었을때만 벡터에 포함시킴
            #posts.append(contents)
        except Exception as ex:
            index=5
        pos_tagger=Kkma()
        posts_tokens=[]

        posts_tokens = [' '.join(pos_tagger.morphs(sentence)) for sentence in post]
        X_test=vectorizer.transform(posts_tokens).toarray()

        index = model.predict(X_test)
        #print(int(index))
    form_tag = form_tagging(int(index)) #string 값 반환

    content_tag =  ContentTagging.content_tagging(contents, path) #list 반환
    print("<GET",result_path,"><GET",form_tag,"><GET",content_tag,"><GET",filename,">")

            #print(form_tag,"+",content_tag)
            #print(score)
            #results.append((paths.index(p), score))

def form_tagging(index):
    return {0: '공고', 1: '기사', 2: '논문', 3: '지원서'}.get(index, '기타')


def run():
    argv_list = sys.argv

    import pandas as pd
    

    filename = './sgdClassifier.pkl'
    global model
    model = joblib.load(filename)
    global vectorizer
    filename='./vectorizer.pkl'
    with open(filename,'rb') as f:
        vectorizer=pickle.load(f)

    folderpath = "./testData/"
    file_list=os.listdir(folderpath);
    #get_tag(folderpath, file_list[i+1])
    folderpath = argv_list[1]

    for i in range(len(argv_list)):
        if i<2:
            pass
        else:
        #print(argv_list[i])
        #get_tag(folderpath, file_list[i])
            get_tag(folderpath, argv_list[i])

def test():
    argv_list = sys.argv
    test_string = argv_list[1]
    for argv in argv_list:
        print(argv)

from konlpy import jvm
import jpype
if __name__ == '__main__':
    sys.exit(run())

    #sys.exit(test())

'''
filename = '/tmp/digits_classifier.joblib.pkl'

model = joblib.load(filename)

model.predict()
'''
