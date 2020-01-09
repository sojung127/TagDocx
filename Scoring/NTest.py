
from tensorflow.keras.preprocessing.text import text_to_word_sequence


'''
#txt
doc = open("C:\\Users\\YooJin\\Desktop\\project\\dataSet\\P11.txt",mode='rt', encoding='utf-8').read()
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
    
extracted_text = convert_pdf_to_txt("C:\\Users\\YooJin\\Documents\\GitHub\\AutomaticFileCategorizeService\\강의자료\\dataset\\B10.pdf")
print(extracted_text)


doc = extracted_text


import re

shortword = re.compile(r'[→%"‘’▶◆‟“”…„0-9]')

doc = shortword.sub('', doc)
#print(type(doc))

w = ['기자', '뉴스']

items = re.findall('\(([^)]+)',doc)
items.append(doc[doc.find("[")+1 : doc.find("]")])
#print(items)

s = 0

for item in items:
    for word in w:
        #print(type(item))
        if word in item:
            print(word)
            s += 5


print("s :", s)


#한자인식을 가능하게 해서 한자가 나올수록 점수를 더한다든가,,,
#doc.find('인쇄')

tokens = text_to_word_sequence(doc)

tokens.sort()
length = len(tokens)
print("length", length)

tokens = list(set(tokens))

#print(tokens)

'''
train = open("C:\\Users\\YooJin\\Desktop\\project\\Nmapping.txt",mode='rt', encoding='utf-8').read()
train = text_to_word_sequence(train)
#print(train)
'''

import pandas as pd
 
train = pd.read_csv(r'C:\\Users\\YooJin\\Desktop\\project\\Nmapping.csv')


#train.sort()

train_words = train['words'].dropna().tolist()
train_score= train['score'].dropna().tolist()

train ={}

for i in range(len(train_words)):
    a = train_words.pop()
    b = train_score.pop()
    train.update({a : b})



for x in tokens:
    
    if x in train:
        print(x)
        s+=train.get(x)


        
print(s/length)

