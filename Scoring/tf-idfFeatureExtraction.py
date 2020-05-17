


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
import docx2txt
import title_test
import Total_Scoring



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



# 폴더에서 문서 목록 읽어오기
path_origin = '../Dataset/지원서/'#input("문서경로:")
file_list = os.listdir(path_origin)

file_count = len(file_list)

from konlpy.corpus import kobill
from konlpy.tag import Okt
from konlpy.tag import Komoran
from gensim import models

docs_ko = []

for i in range(file_count):

    path = path_origin + file_list[i]
    print(path)

    title = title_test.run(path)
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
    

    # 패키지 호출 및 Komoran 객체 생성
    from PyKomoran import *

    komoran = Komoran("STABLE")

    # 명사 추출 전처리 (NNG: 일반명사  NNP: 고유명사)
    try:
        texts_ko = komoran.get_morphes_by_tags(contents, tag_list=['NNG', 'NNP','VV','VA','MM',',MA'])
    except:
        continue

    
    doc=' '.join(texts_ko)
    docs_ko.append(doc)

    from sklearn.feature_extraction.text import TfidfVectorizer

    text=['I go to my home my home is very large', # Doc[0]
    'I went out my home I go to the market',
    # Doc[1] 
    'I bought a yellow lemon I go back to home'] # Doc[2]



    tfidf_vectorizer = TfidfVectorizer(ngram_range=(1,2))
    X=tfidf_vectorizer.fit_transform(texts_ko)

    word_count = pd.DataFrame({
        '단어': tfidf_vectorizer.get_feature_names(),
        'tf-idf': X.sum(axis=0).flat
    })
    print(word_count.sort_values('tf-idf', ascending=False).head(n=5).to_string())
    


