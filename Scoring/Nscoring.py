from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
import re

def Nscoring(doc):
    score = 0

    w = ['사진', '기자', '뉴스']

    # 괄호 안에 있는 단어들 출력
    items = re.findall('\(([^)]+)', doc)  # ()괄호 안에 있는 단어 인식
    items.append(doc[doc.find("[") + 1: doc.find("]")])  # []괄호 안에 있는 단어 인식
    # print(items)

    for item in items:
        for word in w:
            if word in item:
                score += 5
                            # print(word)

    tokens = text_to_word_sequence(doc)
    # 중복처리한 토큰들
    tokens_NP = list(set(tokens))

    train = pd.read_csv(r'.\Nmapping.csv')

    train_words = train['words'].dropna().tolist()  # words 열 읽기
    train_score = train['score'].dropna().tolist()  # score 열 읽기

    train = {}  # dictionary

    for i in range(len(train_words)):
        a = train_words.pop()
        b = train_score.pop()
        train.update({a: b})

    for x in tokens_NP:
        if x in train:
            # print(x)
            score += train.get(x)

    return score
