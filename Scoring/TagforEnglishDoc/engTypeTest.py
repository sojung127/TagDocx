from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO
import os
import re

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
folder_list=['C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/영어공고tr/']
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
        print(file_list[i])
        if os.path.isdir(path):
            pass
        if path[-3:] == 'pdf':
            contents = convert_pdf_to_txt(path)
        elif path[-3:] == 'txt':
            fp = open(path, 'r', encoding='utf-8')
            contents = fp.readlines()
            fp.close()
            contents = ' '.join(contents)
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

    import nltk



    #from konlpy.tag import Kkma
    #pos_tagger=Kkma()
    posts_tokens=[]
    posts_tokens = [' '.join(nltk.pos_tag(sentence)) for sentence in posts]

    X_train=vectorizer.fit_transform(posts_tokens)
    filename='data'+ str(cnt)+'.bin'
    with open(filename,'wb') as f:
        pickle.dump(X_train,f)
        pickle.dump(vectorizer,f)
    print('\n'+filename+' created!\n')
    cnt=cnt+1