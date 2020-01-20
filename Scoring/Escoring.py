from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
import re

#######주의 : 공고는 일단 feature 단어의 등장마다 1점씩만 더함######
def Escoring(doc):

        tokens = text_to_word_sequence(doc)

        #print("length", length)

        tokens = list(set(tokens))
        tokens.sort()
        #print(tokens)

        train = pd.read_csv(r'./Emapping.csv', encoding='CP949')
        train_words = train['words'].dropna().tolist()
        train_score= train['score'].dropna().tolist()

        train ={}

        for i in range(len(train_words)):
            a = train_words.pop()
            b = train_score.pop()
            train.update({a : b})

        score = 0
        
        for x in tokens:
            if x in train:
                #print(x)
                score+=train.get(x)

        return score
    
