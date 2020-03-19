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
    docs_ko = []
    docs_ko.append(contents)

    # 패키지 호출 및 Komoran 객체 생성
    from PyKomoran import *

    komoran = Komoran("STABLE")

    # 명사 추출 전처리 (NNG: 일반명사  NNP: 고유명사)
    texts_ko = komoran.get_morphes_by_tags(docs_ko[0], tag_list=['NNG', 'NNP'])

    pos = lambda d: [d]
    texts_ko = [pos(doc) for doc in texts_ko]

    from gensim import corpora

    dictionary_ko = corpora.Dictionary(texts_ko)  # initialize a dictionary
    dictionary_ko.save('ko.dict')  # save dictionary to file for future use

    # calulate TF-IDF

    from gensim import models

    tf_ko = [dictionary_ko.doc2bow(text) for text in texts_ko]
    tfidf_model_ko = models.TfidfModel(tf_ko)
    tfidf_ko = tfidf_model_ko[tf_ko]
    corpora.MmCorpus.serialize('ko.mm', tfidf_ko)  # save corpus to file for future use

    ntopics, nwords = 5, 4
    import numpy as np;

    np.random.seed(42)

    # Train Topic Model
    # LDA
    import numpy as np;

    np.random.seed(42)  # optional
    lda_ko = models.ldamodel.LdaModel(tfidf_ko, id2word=dictionary_ko, num_topics=ntopics)
    lda_list = lda_ko.print_topics(num_topics=ntopics, num_words=nwords)
    final_result_list=[]
    # 제목에서 명사 분리
    if title!=1:
        final_result_list=komoran.get_morphes_by_tags(title, tag_list=['NNG', 'NNP'])
    

    #가장 확률 높은 토픽의 단어 출력
    topic_words=lda_ko.show_topics(num_topics=1,num_words=5,formatted=False)
    #각 토픽들의 단어 출력
    word_topics=lda_ko.show_topics(num_topics=5,num_words=1,formatted=False)
    
    # 보기 좋게 바꾸기
    words_list=[]
    #각 리스트의 단어들 words_list에 모음
    for i in range(5):
        words_list.append(topic_words[0][1][i][0])
        words_list.append(word_topics[i][1][0][0])

    #중복 제거
    words_list=list(set(words_list))
    # words_list가 원래 lda로 뽑은 10개 단어
    # 여기서 2가지 방법 모두에서 골고루 단어를 어떻게 뽑을지?
    i=0
    while len(final_result_list)<=10 :
        final_result_list.append(words_list[i])
        i+=1

    print(set(final_result_list))


    doc = contents
    #형식태그 출력
    type_score=Total_Scoring.scoring(doc=doc,path=path)

    print(type_score.index(max(type_score)))

    index = type_score.index(max(type_score))
    def form_tag(index): return {0: '논문', 1: '기사', 2: '강의자료', 3: 'E', 4: 'A'}.get(index, '기타')
    print(form_tag(index))