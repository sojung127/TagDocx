#-*-coding:utf-8-*-

from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO

#한글이 포함되어 있는 PDF 읽기
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

from konlpy.corpus import kobill
files_ko=kobill.fileids()
doc_ko=kobill.open('1809890.txt').read()

from konlpy.tag import Hannanum
from konlpy import jvm
hannanum=Hannanum()
tokens_ko=hannanum.morphs(doc_ko)
exit()
pdf_file = "C:/Users/소정/Desktop/졸업프로젝트/AutomaticDocumentClassificationService/Dataset/강의자료/경영10/1차시 PR의 정의와 유형PR의 4모형.pdf"                                     # 로컬 PC에 있는 pdf 파일도 읽을 수 있음
contents = convert_pdf_to_txt(pdf_file)

import nltk
from nltk.tokenize import word_tokenize
from nltk.tokenize import sent_tokenize
text="""Hello Mr. Smith, how are you doing today? The weather is great, and city is awesome.
The sky is pinkish-blue. You shouldn't eat cardboard"""
tokenized_text=word_tokenize(contents)
tokenized_setn=sent_tokenize(contents)
print(len(tokenized_text))
from nltk.probability import FreqDist
fdist=FreqDist(tokenized_text)
print(fdist.most_common(2))

from nltk.corpus import stopwords
stop_words=set(stopwords.words('english'))
filtered_sent=[]
for w in tokenized_setn:
    if w not in stop_words:
        filtered_sent.append(w)
#print("tokenized:",tokenized_text)
print("filteredD:",filtered_sent)

