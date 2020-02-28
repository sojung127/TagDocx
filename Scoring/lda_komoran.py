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


score = 0
current = 0
# 폴더에서 문서 목록 읽어오기
path_origin = input("문서경로:")
file_list = os.listdir(path_origin)

file_count = len(file_list)

print("여기 집중")

from konlpy.corpus import kobill
from konlpy.tag import Okt
from konlpy.tag import Komoran
from gensim import models

docs_ko = []

for i in range(file_count):

    path = path_origin + file_list[i]
    print(path)
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
    docs_ko = []
    docs_ko.append(contents)

    '''
    from konlpy.tag import Okt; t=Okt()
    texts_ko = t.pos(docs_ko[0], norm=True)
    
    from konlpy.tag import Komoran;

    t = Komoran()
    '''
    # 패키지 호출 및 Komoran 객체 생성
    from PyKomoran import *
    komoran = Komoran("STABLE")
    
    # 명사 추출 전처리 (NNG: 일반명사  NNP: 고유명사)
    texts_ko = komoran.get_morphes_by_tags(docs_ko[0], tag_list=['NNG', 'NNP'])

    pos = lambda d: [d]
    texts_ko = [pos(doc) for doc in texts_ko]

    from gensim import corpora

    dictionary_ko = corpora.Dictionary(texts_ko)     # initialize a dictionary
    dictionary_ko.save('ko.dict')     # save dictionary to file for future use

    # calulate TF-IDF

    from gensim import models

    tf_ko = [dictionary_ko.doc2bow(text) for text in texts_ko]
    tfidf_model_ko = models.TfidfModel(tf_ko)
    tfidf_ko = tfidf_model_ko[tf_ko]
    corpora.MmCorpus.serialize('ko.mm', tfidf_ko)  # save corpus to file for future use

    ntopics, nwords = 5, 4
    import numpy as np;

    np.random.seed(42)

    #Train Topic Model
    # LDA
    import numpy as np;

    np.random.seed(42)  # optional
    lda_ko = models.ldamodel.LdaModel(tfidf_ko, id2word=dictionary_ko, num_topics=ntopics)
    lda_list = lda_ko.print_topics(num_topics=ntopics, num_words=nwords)
    '''
    # LSI
    ntopics, nwords = 5, 4
    lsi_ko = models.lsimodel.LsiModel(tfidf_ko, id2word=dictionary_ko, num_topics=ntopics)
    lsi_list =lsi_ko.print_topics(num_topics=ntopics, num_words=nwords)

    #HDP
    import numpy as np;
    np.random.seed(42)  # optional
    hdp_ko = models.hdpmodel.HdpModel(tfidf_ko, id2word=dictionary_ko)
    print(hdp_ko.print_topics(topics=ntopics, topn=nwords))
    '''
    # 보기 좋게 바꾸기
    import re

    reg = "[\'\"][^\'\"]+[\'\"]"
    
    final_result_list = []
    print("Train in HDP ")
    for t in lda_list:
        # splits = t[1].split
        result = re.findall(reg, t[1])
        final_result_list.append(result[0])
        print(result[0])
    print("\n")

    '''
    #Scoring document
    bow = tfidf_model_ko[dictionary_ko.doc2bow(texts_ko[0])]
    l = sorted(lda_ko[bow], key=lambda x: x[1], reverse=True)
    index = l[0][0]
    result_list = lda_ko.print_topics(num_topics=ntopics, num_words=nwords)[index]

    result = re.findall(reg, result_list[1])
    for i in result:
        final_result_list.append(i)

    final_result_list = set(final_result_list)

    print(final_result_list)
    print(' ')
    '''