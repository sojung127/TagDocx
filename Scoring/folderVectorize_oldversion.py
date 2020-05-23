from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO
import os
import re

import hwptest
import docx2txt
from sklearn.feature_extraction.text import CountVectorizer
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
'''
folder_list=[ "C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/공고/",
               "C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/기사/0전체폴더/",
                "C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/지원서/"]
'''

#folder_list=['C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/논문/0전체/']
folder_list=['C:/capston/AutomaticDocumentClassificationService/Dataset/논문/0전체/']
#folder_list=['C:/Users/user/Desktop/dataset/']
cnt=3
from konlpy.tag import Kkma
pos_tagger=Kkma()
                
for folder in folder_list:
    path_origin=folder
    file_list = os.listdir(path_origin) #list 반환

    posts=[]
    cnt=0
    for i in range(len(file_list)):
        path = path_origin + file_list[i]
        print(path)
        if os.path.isdir(path):
            pass
        if path[-3:] == 'pdf':
            contents = convert_pdf_to_txt(path)
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
        post=[]
        post.append(contents)
        if (contents!=''):
            try:
                ' '.join(pos_tagger.morphs(contents))
                vectorizer=CountVectorizer(min_df=1)
                X_train=vectorizer.fit_transform(post)
                posts.append(contents)
            except Exception as ex:
                print('error',ex)
                pass
        
        #posts.append(contents)


        from sklearn.feature_extraction.text import CountVectorizer
        vectorizer = CountVectorizer(min_df=1)


        from konlpy.tag import Kkma
        pos_tagger=Kkma()
        posts_tokens=[]
        posts_tokens = [' '.join(pos_tagger.morphs(sentence)) for sentence in posts]

        X_train=vectorizer.fit_transform(posts_tokens)
        # 디렉토리 생성
        #dirname = 'C:/capston/AutomaticDocumentClassificationService/Dataset/VectorResult/'
        dirname = folder + 'VectorResult/'
        try:
            if not(os.path.isdir(dirname)):
                os.makedirs(os.path.join(dirname))
        except OSError as e:
            if e.errno != errno.EEXIST:
                print("Failed to create directory!")
                raise
        # pickle 파일에 학습결과 저장
        filename='data'+ str(cnt)+'.bin'
        filepath = os.path.join(dirname, filename)
        with open(filepath,'wb') as f:
            pickle.dump(X_train,f)
            f.close()
        print('\n'+filename+' created!\n')
        cnt=cnt+1
    '''
        # pickle 파일에 학습 결과 출력
        with open(filepath,'rb') as fr:
            data = pickle.load(fr)
            #print(type(data)+'\n')
            print(filepath+'\n')
            fr.close()
    '''
    '''
    new_post= convert_pdf_to_txt("C:/Users/소정/Desktop/졸업프로젝트/공고2/연세대학교 생명시스템대학 계약직원 모집.pdf")
    new_post_tokens=' '.join(pos_tagger.morphs(new_post))

    new_post_vec = vectorizer.transform([new_post_tokens])


    # 거리를 구하는 dist_raw 함수 정의. 최단 거리 계산
    import numpy as np
    def dist_raw(v1, v2):
        delta = v1 - v2
        return np.linalg.norm(delta.toarray())

    import sys
    best_dist = sys.maxsize
    best_doc = None
    best_i = None
    d_array=[]
    # 이전 게시물과 기존 게시물의 거리를 계산하여 출력. 유사도가 가장 높은 게시물을 찾음
    for i, post in enumerate(posts):
        post_vec = X_train.getrow(i)
        d = dist_raw(post_vec, new_post_vec)
        d_array.append(d)
        
    sum=0
    for d in d_array:
        sum+=d

    print(sum/len(d_array))
    '''
   


 