#-*-coding:utf-8-*-
def convert_hwp_to_txt(path):
    import subprocess
    # 4번째 부분에 파일 경로 넣으면 됨
    # pyhwp 설치 필요
    cmd=['hwp5proc','cat','--vstreams',path,'BodyText/Section0.xml']
    fd_popen=subprocess.Popen(cmd,stdout=subprocess.PIPE).stdout
    data=fd_popen.read().strip()
    fd_popen.close()

    from bs4 import BeautifulSoup
    bs=BeautifulSoup(data,'xml',from_encoding='utf-8')
    text=''
    tag=bs.findAll('Text')
    for textElement in tag:
        text+=textElement.text+'\n'
    return text
