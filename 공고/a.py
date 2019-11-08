
fp = open('겨울방학 현장실습 지원.txt','r',encoding='UTF8')
lines = fp.readlines()

scoreList=[]#10개

for i in range(10):
    scoreList.append(0)

# for i in range(len(lines)):
#     lines[i]=lines[i].replace(" ","")

wordList=[
    ['공고','모집'],
    ['기간','일정','접수','마감일'],
    ['선정자발표'], # 발표 류의 단어 더 추가하기
    ['자격요건','대상','응시자격'],
    ['제출서류'],
    ['지원방법','선발방법'],
    ['유의사항','문의','참고사항','주의사항'],
    ['수신'],
    ['경유'],
    ['제목'],
    ['붙임','첨부'],
    ['귀사'],
    ['무궁한발전을기원'],
]
index=0
isFind=False
for word in wordList:
    for line in lines:
            for w in word:
                if w in line:
                    scoreList[index]=scoreList[index]+1

                    
        
    index=index+1
    isFind=False

print(scoreList)

    

#print(lines)