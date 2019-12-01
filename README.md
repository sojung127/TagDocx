# AutomaticDocumentCategorizeService

## 1. 프로젝트 소개
  이 프로젝트를 통해 해결/완화하고자하는 문제에 대해 기술
  과제나 프로젝트 진행 시에 생성하거나 다운로드한 파일들을 미리 잘 저장해두지 않으면 나중에 따로 분류하기 위해서 시간을 들여야하고, 정확한 제목과 위치를 알지 못하면 필요한 파일들을 찾을 때 어려움이 생긴다.

#### 1. 파일 검색 시에 생기는 불편함
  - 제목을 정확히 알지 못함
  - 저장된 위치를 정확히 알지 못함
  - 검색 소요 시간이 너무 오래걸림
  
#### 2. 파일 저장,다운로드 시 생기는 불편함 
  - 자동 저장된 폴더 위치 알지 못함
  - 크롬의 경우 저장시에 파일 이름을 사용자가 지정하지 못함
  - 크롬/카카오톡/의 경우 파일이 저장되는 폴더위치 정하지 못하여 사용자가 경로를 찾기 힘든 경우가 많음
  - 하위 폴더 수가 많아서 정리와 탐색 시간이 오래 걸림
  - 매번 폴더를 지정하는것 불편함
  
## 2. 프로젝트 구성
  사용자가 직접 태그를 붙이거나 파일의 이름을 정하지 않더라도 파일의 내용을 분석하여 태그를 할당한다. 파일에 태그를 설정할 수 있게 된다면, 파일의 태그를 이용하여 태그별로 파일을 분류하게 되어 확장자가 아닌 내용의 연관성을 따질 수 있게 된다. 또한, 파일을 찾을 때는 파일의 내용은 대략적으로 알지만 제목을 모를때, 혹은 제목을 내용과는 상관없는 것으로 지었을 때 태그를 이용하여 효율적으로 파일을 찾을 수 있다.,이메일 서비스와 결합하여 이메일을 쓸 때 이메일의 내용을 분석하여 파일을 첨부해야 할 경우에 분석한 내용을 토대로 첨부할 파일을 추천한다.구현할 기능은 다음과 같다.
  
    1. 자동으로 파일 태깅 : 태그가 생성되지 않은 파일들을 분석하여 태그를 달아주는 기능
    2. 검색 : 파일을 검색하고 싶지만 파일의 제목이 기억이 나지 않거나 파일의 경로, 내용을 모르지만 키워드는 알고 있을 때 파일을 쉽게 찾을 수 있게 하기 위하여 지원하는 검색 기능
    2-1.추가 기능
      1) 검색결과로 나온 파일들의 목록에서 파일을 더블클릭하면 파일이 열린다.
      2) 검색결과로 나온 파일들의 목록에서 파일에 마우스를 가져다 대고 오른쪽 버튼을 클릭하면 파일의 경로를 확인할 수 있다.
    3. 분류 : 파일들의 태그를 이용하여 유사한 내용, 주제를 가진 파일끼리 폴더에 분류하여 저장경로를 이동하는 기능
    
## 3. 구현 방법

  ### 1. 플랫폼
-데스크톱 프로그램으로 제작
-후에 웹 어플리케이션, 앱으로 제작할 것을 고려

  ### 2. 구현 언어
1) C++(Visual Studio 2017)
  윈도우즈 데스크탑 응용 프로그램 개발
2) python 
  - 문서 검색하는 기능에서 태그끼리의 유사도를 확인하여 여러 문서를 한 대표 태그로 묶어주는 기능
  - 문서를 분석하여 키워드를 추출하는 모델

  ### 3. 구체적인 기능 구현 방법

    #### 문서 태깅 - 태그 할당 방식은 형식 태그와 내용 태그로 구분한다.
      [1] 형식 태그
        A. 지도학습 알고리즘 사용
          i. 지도학습 알고리즘을 사용하여 다중 카테고리 문장 분류를 구현할 것이다. 논문, 기사, 공문, 강의자료 등 주제별로 25개의 데이터를 수집하여 각 문서의 내용을 라벨링한 데이터인 train data를 이용하여 모델을 학습시킬 예정이다.
          ii. 방법
            1) 텍스트 전체를 분석하여 각 카테고리별로 Featuremap을 만드는 방법을 이용한다. 분석 순서는 문서명-확장자 순으로 할 계획이다. 
              - 문서명 (단, 문서명에 분류하고 싶은 카테고리의 이름이 명확히 나와있을 때 Featuremap을 만들지 않고 바로 태그를 만들어 할당 할 수 있게 한다.)
              - 확장자 : 파일의 확장자가 pptx일 경우 파일이 발표자료이거나 강의자료일 확률이 더 높아지도록 한다.

              - Featuremap: 문서를 분석하여 형식으로 추출될 수 있는 특징을 뽑아 글의 형식일 법한 확률을 각 카테고리마다 계산한 특징 추출 맵.
              - Feature Map에 반영될 각 카테고리 별 특징:  기사 (‘~기자’ 문구가 존재), 논문( ‘~저자’ 문구가 존재), 공문 (기관명이 명시 되어 있음), 강의자료 (학교 명, 교수 명, 과목 명, ‘chapter’), 발표자료, 지원서/자기소개서(‘지원서’ 문구가 존재), 과제(성능 확인 필요), 보고서(‘학번’ ‘과’ 문구가 존재), 회의록(‘~회의록’ 문구가 존재), 알수없음(에세이 등)
              
            2) 각 train data 문서별로 라벨링을 하여 텍스트에 대한 특징을 뽑지 않고 해당 문서가 어떤 분류에 속하는 지를 라벨을 붙여서 모델을 학습시킨다.
              - 전처리 과정을 거친 후 모델이 학습하게 하여 새로운 문서를 분석할 때 기존 라벨링한 텍스트의 정보를 토대로 새로운 문서의 형식이 어떤 카테고리에 속하는지 판단한다.
        B. 딥러닝 사용
          - 분류하고 싶은 카테고리 별로 자료를 100개씩 모아 딥러닝 알고리즘을 사용하여 각 카테고리별 특징을 추출한다.
          - 특징을 추출함으로써 새로운 문서를 분석하였을 때 문서의 특징을 추출하여 문서가 어떤 카테고리에 해당하는지 판단할 수 있게 한다.
          - 조사한 자료 예시
          [brightmart/text_classification](https://github.com/brightmart/text_classification)
          [Text-Classification-Pytorch](https://github.com/prakashpandey9/Text-Classification-Pytorch)

        C. Decision Tree 사용
          - 전처리 과정을 거친 문서는 feature map을 만들어낸다.
          - decision tree는 feature map을 사용하여 모델을 학습시키고. 학습된 모델은 문서의 형식을 판단한다. 

      [2] 내용 태그
        비지도 학습 알고리즘 사용 예정

  ### 4. 인터페이스 
    - 메인화면: 3가지 기능(태그달기, 문서검색, 문서 분류)을 수행하는 화면으로 넘어가기 위해 버튼을 선택할 수 있는 화면
    - 태그 달기 화면: 문서 태그할당 기능을 수행할 화면
    - 문서 검색 화면: 사용자한테 입력값을 받거나 보기에서 원하는 태그를 선택하고 문서 검색 기능을 실행하는 화면
    - 문서 분류 화면: 원하는 폴더 내에 있는 문서들을 키워드끼리 클러스터링 해주고 폴더 내에 하위 폴더를 새로 생성해서 분류를 하는 화면
    *인터페이스 구현 상황을 캡처하여 비고에 첨부하였음* 

  ### 5. 코드 및 버전 관리
    1) Github
    2) Visual Studio Code

## 4. 인터페이스
  ![main](https://user-images.githubusercontent.com/29905149/69696918-7a812400-1124-11ea-80cb-b1e608300f53.PNG)
  ![set_tag](https://user-images.githubusercontent.com/29905149/69696953-9ab0e300-1124-11ea-9886-6dbe258d8f74.PNG)
  ![search_doc](https://user-images.githubusercontent.com/29905149/69696981-b0260d00-1124-11ea-93a9-893dfcac5bb7.PNG)
  ![classifi_doc](https://user-images.githubusercontent.com/29905149/69696991-b7e5b180-1124-11ea-8320-55a2d12074ca.PNG)
  
    인터페이스 시연 영상 
    <iframe width="640" height="360" src="https://www.youtube.com/watch?v=qPtYTAtOxzg" frameborder="0" gesture="media" allowfullscreen=""></iframe>

  
## 5. 구현 영상









