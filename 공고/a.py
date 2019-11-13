# import olefile
#import hwp5

# test_file=olefile.OleFileIO('2020년 아산다솜장학생 선발안내.hwp')

# encoded_text = test_file.openstream('PrvText').read()

# lines=encoded_text.decode('UTF-16').split('\n')

# hwp5.hwp5txt('2020년 아산다솜장학생 선발안내.hwp','test.txt')

# print(lines)

fp = open('글로벌소통교육실 계약직원 채용공고.txt','r',encoding='utf-8')
lines = fp.readlines()
fp.close()
fp = open ('feature.txt','r',encoding='utf-8-sig')
features = fp.readlines()
fp.close()

scoreList=[]#10개
featureList=[]

for i in range(len(features)):
    scoreList.append(0)
    features[i]=features[i].strip()
    featureList.append(features[i].split())

print(featureList)

for i in range(len(lines)):
    lines[i]=lines[i].replace(" ","")

index=0
isFind=False

sum=0
value=0
for i in range(len(featureList)):
    if i == 0  :
        value=5
    elif i == 1  :
        value=3
    else:
        value=1
    for line in lines:
            for w in featureList[i]:
                if w in line:
                    scoreList[index]=scoreList[index]+1*value
                    print(i)
                    print(w,value)
                    print(scoreList)
                    sum=sum+value #합값생성

                    
        
    index=index+1
    isFind=False

print()
print(scoreList)
print(sum)

    

