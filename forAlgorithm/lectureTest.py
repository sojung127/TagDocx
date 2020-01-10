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


from konlpy import jvm

jvm.init_jvm()
pdf_file = "C:/Users/pyj/MyWorks/AutomaticDocumentClassificationService/Dataset/강의자료/경영10/1차시 PR의 정의와 유형PR의 4모형.pdf"                                     # 로컬 PC에 있는 pdf 파일도 읽을 수 있음
contents = convert_pdf_to_txt(pdf_file)

from konlpy.corpus import kobill

from konlpy.tag import Okt
t=Okt()
tokens_ko=t.morphs(contents)

print(len(ko.tokens))       # returns number of tokens (document length)
print(len(set(ko.tokens)))  # returns number of unique tokens
ko.vocab()  

ko.plot(50) 