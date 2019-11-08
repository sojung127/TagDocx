from tensorflow.keras.preprocessing.text import text_to_word_sequence

#################1. 파일 input 받기##################
'''
#input file is .txt
doc = open("C:\\P2.txt",mode='rt').read() #인풋 파일의 절대경로로 수정
'''

'''
#input file is .docx
import docx2txt
doc = docx2txt.process("C:\\Letter.docx") #인풋 파일의 절대경로로 수정
'''

#input file is .pdf
from tika import parser
doc = parser.from_file("C:\\P5.pdf") #인풋 파일의 절대경로로 수정
doc = doc["content"]

#################2. 데이터 가공##################
#숫자와 이상한 기호, 1~2글자인 단어를 정규표현식으로 없앰 처리
import re
shortword = re.compile(r'[‟“”„0-9]')
doc = shortword.sub('', doc)
shortword = re.compile(r'\W*\b\w{1,2}\b')
doc = shortword.sub('', doc)

#################3. 토큰 처리##################
tokens = text_to_word_sequence(doc)
'''
#text_to_word_sequence 말고 다른 방법으로 tokenize
from nltk.tokenize import word_tokenize
word_tokens = word_tokenize(text)
'''
#불용어 처리
from nltk.corpus import stopwords
stop_words = set(stopwords.words('english'))

result = []
for w in tokens:
    if w not in stop_words:
        result.append(w)

tokens = result

#print(tokens) : 제대로 token화 되었는지 확인

'''
#품사 태깅
from nltk.tag import pos_tag

tag = pos_tag(tokens)
print(tag)
'''

'''
#어간추출

#별로
from nltk.stem import LancasterStemmer
l=LancasterStemmer()
print([l.stem(w) for w in tokens])

#별로
from nltk.stem import PorterStemmer
s = PorterStemmer()
print([s.stem(w) for w in tokens])
'''

#표제어 추출
#괜찮 +n.lemmatize('dies', 'v')이런식으로 단어가 동사 품사라는 사실을 알려줄 수 있음.
from nltk.stem import WordNetLemmatizer
n=WordNetLemmatizer()
#print([n.lemmatize(w) for w in tokens])
tokens = [n.lemmatize(w) for w in tokens]


#중복 처리 : 중복 처리를 원하지 않을 경우(단어의 빈도수 체크) 주석을 유지하면 됨
#tokens = list(set(tokens))

#tokens 정렬하여 출력해보기
#tokens.sort()
#print(tokens)

#################4. 점수 기준이 되는 csv파일 받아오기##################
import pandas as pd

train = pd.read_csv(r'C:\\Users\\YooJin\\Desktop\\project\\mapping.csv')

#열 단위로 읽어와서 빈 행 제거 후 list로 변환
#csv파일에는 'words'와 'score' 열이 있어야 한다. 임의로 수정 가능
train_words = train['words'].dropna().tolist()
train_score= train['score'].dropna().tolist()

train ={}

#csv파일에 만든 기준 단어들을 (단어 : 점수) 로 train(dictionary)에 저장
for i in range(len(train_words)):
    a = train_words.pop()
    b = train_score.pop()
    train.update({a : b})

#최종 score
s = 0

#위에서 분석한 파일을 통해 얻은 token들이 train의 key값으로 존재할 시 점수를 score에 더함
for x in tokens:
    if x in train:
        s+=train.get(x)

#점수 출력
print(s)

