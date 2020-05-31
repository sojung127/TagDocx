# 패키지 호출 및 Komoran 객체 생성
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
from konlpy.corpus import kobill
from gensim import corpora, models
import numpy as np;
import title_test
import sys
import nltk
#nltk.download('wordnet')

from nltk.corpus import wordnet as wn
from nltk.corpus import stopwords

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

    #print("여기 집중")

    for i in range(file_count):
        
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
        content_tag =  content_tagging(contents, path) #list 반환


from gensim import corpora

def content_tagging(text, path):
    
    tokens = [tok for tok in text.split()]
    stop = set(stopwords.words('english'))
    clean_tokens = [tok for tok in tokens if len(tok.lower())>1 and (tok.lower() not in stop)]
    
    tagged = nltk.pos_tag(clean_tokens)
    allnoun = [word for word, pos in tagged if pos in ['NN','NNP']]
    
    pos = lambda d: [d]
    allnoun = [pos(doc) for doc in allnoun]

    #print(allnoun)

    dictionary = corpora.Dictionary(allnoun)  # initialize a dictionary
    dictionary.save('en.dict')  # save dictionary to file for future use

    # calulate TF-IDF

    tf_ko = [dictionary.doc2bow(text) for text in allnoun]
    tfidf_model_ko = models.TfidfModel(tf_ko)
    tfidf_ko = tfidf_model_ko[tf_ko]
    corpora.MmCorpus.serialize('ko.mm', tfidf_ko)  # save corpus to file for future use

    ntopics, nwords = 5, 4

    np.random.seed(42)

    # Train Topic Model
    # LDA

    np.random.seed(42)  # optional
    lda_ko = models.ldamodel.LdaModel(tfidf_ko, id2word=dictionary, num_topics=ntopics)
    final_result_list = []

    # 제목에서 명사 분리
    title = title_test.run(path)

    if type(title) is "<class 'str'>":
        final_result_list = komoran.get_morphes_by_tags(title, tag_list=['NNG', 'NNP'])

    # 가장 확률 높은 토픽의 단어 출력
    topic_words = lda_ko.show_topics(num_topics=1, num_words=5, formatted=False)
    # 각 토픽들의 단어 출력
    word_topics = lda_ko.show_topics(num_topics=5, num_words=1, formatted=False)

    # 보기 좋게 바꾸기
    words_list = []

    # 각 리스트의 단어들 words_list에 모음
    for i in range(5):
        words_list.append(topic_words[0][1][i][0])
        words_list.append(word_topics[i][1][0][0])

    # 중복 제거
    words_list = list(set(words_list))
    
    i = 0
    while len(final_result_list) <= 10 and i < len(words_list):
        final_result_list.append(words_list[i])
        i += 1

    final_result_list = list(set(final_result_list))

    print(final_result_list)

def run():
    argv_list = sys.argv

    for i in range(len(argv_list)-1):
        get_tag(argv_list[i+1])

# print(index)
def form_tagging(index):
    return {0: '논문', 1: '기사', 2: '강의자료', 3: '공고', 4: 'A'}.get(index, '기타')

if __name__ == '__main__':
    sys.exit(run())