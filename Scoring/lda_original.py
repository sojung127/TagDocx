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
path_origin = input("문서경로:")
file_list = os.listdir(path_origin)

file_count = len(file_list)

print("여기 집중")

from konlpy.corpus import kobill
from konlpy.tag import Okt
from gensim import models

docs_ko = []

for i in range(1):
    path = path_origin + file_list[i]
    if path[-3:] == 'pdf':
        contents = convert_pdf_to_txt(path)
    else:
        contents = ''
    docs_ko = []
    docs_ko.append(contents)

    from konlpy.tag import Okt;

    t = Okt()
    texts_ko = t.pos(docs_ko[0], norm=True)

    nouns = [n for n, tag in texts_ko if tag == 'Noun']

    pos = lambda d: ['/'.join(p) for p in t.pos(d, stem=True, norm=True)]
    texts_ko = [pos(doc) for doc in nouns]

    # encode tokens to integers

    from gensim import corpora

    dictionary_ko = corpora.Dictionary(texts_ko)
    dictionary_ko.save('ko.dict')  # save dictionary to file for future use

    # calulate TF-IDF

    from gensim import models

    tf_ko = [dictionary_ko.doc2bow(text) for text in texts_ko]
    tfidf_model_ko = models.TfidfModel(tf_ko)
    tfidf_ko = tfidf_model_ko[tf_ko]
    corpora.MmCorpus.serialize('ko.mm', tfidf_ko)  # save corpus to file for future use

    ntopics, nwords = 6, 5
    import numpy as np;

    np.random.seed(42)

    # LDA
    import numpy as np;

    np.random.seed(42)  # optional
    lda_ko = models.ldamodel.LdaModel(tfidf_ko, id2word=dictionary_ko, num_topics=ntopics)
    print(path)
    print(lda_ko.print_topics(num_topics=ntopics, num_words=nwords))
    print("\n")

    bow = tfidf_model_ko[dictionary_ko.doc2bow(texts_ko[0])]
    sorted(lda_ko[bow], key=lambda x: x[1], reverse=True)





