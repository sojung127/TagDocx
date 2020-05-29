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

## 폴더리스트 수정하기
folder_list=["C:/Users/소정/Desktop/졸업프로젝트/test2/","C:/Users/소정/Desktop/졸업프로젝트/test1/"]
from sklearn.feature_extraction.text import CountVectorizer
vectorizer = CountVectorizer(min_df=1)
from konlpy.tag import Kkma
pos_tagger=Kkma()
label=[]
# 트레이닝2, 테스트2개 폴더에 맞춘 코드입니다! 트레이닝,테스트, 트레이닝,테스트 순으로 FOLDERLIST만들어주세요               
posts=[]

for i in range(2):
    path_origin=folder_list[i]
    file_list = os.listdir(path_origin) #list 반환

    cnt=0
    for j in range(len(file_list)):
        path = path_origin + file_list[j]
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
                #문서를 읽어서 벡터를 만드는 과정에 오류가 없는지 확인하는 과정
                ' '.join(pos_tagger.morphs(contents))
                vectorizer=CountVectorizer(min_df=1)
                X_train=vectorizer.fit_transform(post)
                # 위의 코드들이 정상 실행되었을때만 벡터에 포함시킴
                posts.append(contents)
                cnt=cnt+1
            except Exception as ex:
                print('error',ex)
                pass
    label.append(cnt)
        


from konlpy.tag import Kkma
pos_tagger=Kkma()
posts_tokens=[]

posts_tokens = [' '.join(pos_tagger.morphs(sentence)) for sentence in posts]

X_train=vectorizer.fit_transform(posts_tokens).toarray()
    
## !! 자기 폴더 길이에 맞게 수정해주세용!! 
namelist=folder_list[i][74:-1].split('/')
name=''.join(namelist)
# python 실행하는 폴더에파일이 생깁니다(cmd창 현재 경로)
filename='vectorData'+'.bin'
with open(filename,'wb') as f:
    pickle.dump(X_train,f)
    pickle.dump(label,f)
    pickle.dump(vectorizer,f)
print('\n'+filename+' created!\n')




 