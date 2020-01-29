from tensorflow.keras.preprocessing.text import text_to_word_sequence
import pandas as pd
import re

def Ascoring(doc):
    score = 0

    '''
    words = ['게재 결정', '결론 및 논의', '글을 마치며' '나가는 말', '논문 게재',
         '논문 발표', '들어가는 말','심사 일자', '연구 결과', '연구 방법'
         , '요약 및 결론','논문 접수', '논문 심사', '게재 확정']

    for word in words:
        if word in doc:
            score += 1
            #print(word)
    '''
    reg1 = '지\s+원\s+서'
    reg2 = '자\s+기\s+소\s+개\s+서'
    reg3 = '이\s+력\s+서'
    

    regs = [reg1, reg2, reg3]

    for reg in regs:
        result = re.findall(reg, doc)
        for i in range(len(result)):
            score += 5
    

#######################################

    tokens = text_to_word_sequence(doc)

    tokens.sort()
    length = len(tokens)
    #print("length", length)

    tokens = list(set(tokens))

    #print(tokens)
     
    train = pd.read_csv(r'./Amapping.csv', encoding='CP949')

    #train.sort()

    train_words = train['words'].dropna().tolist()
    #train_score= train['score'].dropna().tolist()

    train ={}

    #주의! 여기서는 1점씩만 올림
    for i in range(len(train_words)):
        a = train_words.pop()
        #b = train_score.pop()
        train.update({a : 1})

    for x in tokens:
        if x in train:
            #print(x)
            score+=train.get(x)
            

    return score
    #return(s/length)
