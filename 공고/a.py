import olefile

# test_file=olefile.OleFileIO('2020년 아산다솜장학생 선발안내.hwp')

# encoded_text = test_file.openstream('PrvText').readlines()
# #print(encoded_text)
# print(len(encoded_text))
# lines=encoded_text.decode('UTF-16').split('\n')

# print(lines)

# fp = open('아산장학생.txt','r',encoding='utf-8-sig')
# lines = fp.readlines()
# fp.close()
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
for word in featureList:
    for line in lines:
            for w in word:
                if w in line:
                    scoreList[index]=scoreList[index]+1

                    
        
    index=index+1
    isFind=False

print(scoreList)

    

#print(lines)