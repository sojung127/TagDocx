from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
import re

def Pscoring(doc):
    score = 0
    
    words = ['게재 결정', '결론 및 논의', '글을 마치며' '나가는 말', '논문 게재',
         '논문 발표', '들어가는 말','심사 일자', '연구 결과', '연구 방법'
         , '요약 및 결론','논문 접수', '논문 심사', '게재 확정']

    for word in words:
        if word in doc:
            score += 1
            #print(word)

    reg1 = '그림\s+\d+'
    reg2 = '표\s+\d+'
    reg3 = 'Fig\s+\d+'
    reg4 = '\(대학교.+학과\)'
    reg5 = '결\s+론'
    reg6 = '서\s+론'
    reg7 = '목\s+차'
    reg8 = '차\s+례'
    reg9 = ".+\(\d+\),\s?.+,\s?.+,\s?.+,\s?.+쪽"
    reg10 = ".+\.\s?\(\d+\)\.\s?.+,\s?.+권.+,\s?pp\."


    regs = [reg1, reg2, reg3, reg4, reg5, reg6, reg7, reg8, reg9, reg10]

    for reg in regs:
        result = re.findall(reg, doc)
        for i in range(len(result)):
            score += 1

#######################################

    tokens = text_to_word_sequence(doc)

    tokens.sort()
    length = len(tokens)
    #print("length", length)

    tokens = list(set(tokens))

    #print(tokens)
     
    train = pd.read_csv(r'./Pmapping.csv', encoding='CP949')

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
            #print(x)
            score+=train.get(x)
            

    return score
    #return(s/length)
