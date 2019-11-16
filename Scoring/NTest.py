
from tensorflow.keras.preprocessing.text import text_to_word_sequence


'''
#txt
doc = open("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\P2.txt",mode='rt').read()

'''
'''
#docx
import docx2txt
doc = docx2txt.process("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\Letter.docx")
'''
'''
#doc
import Document
doc = Document.open("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\Letter.docx")
'''

'''
#pdf
from tika import parser
doc = parser.from_file("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\N1.pdf")
doc = doc["content"]
print(doc)
'''
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfpage import PDFPage
from io import StringIO

def convert_pdf_to_txt(path):
    rsrcmgr = PDFResourceManager()
    retstr = StringIO()
    codec = 'utf-8'
    laparams = LAParams()
    device = TextConverter(rsrcmgr, retstr, codec=codec, laparams=laparams)
    fp = open(path, 'rb')
    interpreter = PDFPageInterpreter(rsrcmgr, device)
    password = ""
    maxpages = 0
    caching = True
    pagenos=set()
    
    for page in PDFPage.get_pages(fp, pagenos, maxpages=maxpages, password=password,caching=caching, check_extractable=True):
        interpreter.process_page(page)
        text = retstr.getvalue()

    fp.close()
    device.close()
    retstr.close()
    return text
    
extracted_text = convert_pdf_to_txt("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\N1.pdf")
#print(type(extracted_text))

doc = extracted_text

import re

shortword = re.compile(r'[▶‟“”…„0-9]')

doc = shortword.sub('', doc)
#print(type(doc))

'''
items = re.findall('\(([^)]+)',doc)
items_2 = re.findall(r'\[[^)]*\]',doc)
print(items)
print(items_2)
'''

print(doc[doc.find("[")+1 : doc.find("]")])
 

#doc.find('인쇄')
#if '['

'''
'''
shortword = re.compile(r'\W*\b\w{1,2}\b')
doc = shortword.sub('', doc)
'''

tokens = text_to_word_sequence(doc)

tokens.sort()

#print(tokens)

from konlpy.tag import Okt
okt=Okt()
okt = okt.nouns(doc)
okt.sort()
#print(okt)

import pandas as pd

train = pd.read_csv(r'C:\\Users\\YooJin\\Desktop\\project\\mapping.csv')

#train.sort()

train_words = train['words'].dropna().tolist()
train_score= train['score'].dropna().tolist()

train ={}

for i in range(len(train_words)):
    a = train_words.pop()
    b = train_score.pop()
    train.update({a : b})


s = 0

for x in tokens:
    
    if x in train:
        #print(x)
        s+=train.get(x)

print(s)


'''