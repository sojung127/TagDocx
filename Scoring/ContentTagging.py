# 패키지 호출 및 Komoran 객체 생성
from konlpy.corpus import kobill
from konlpy.tag import Komoran
from PyKomoran import *
from gensim import corpora, models
import numpy as np;
import title_test

def content_tagging(text, path):

    komoran = Komoran("STABLE")

    # 명사 추출 전처리 (NNG: 일반명사  NNP: 고유명사)
    texts_ko = komoran.get_morphes_by_tags(text, tag_list=['NNG', 'NNP'])

    pos = lambda d: [d]
    texts_ko = [pos(doc) for doc in texts_ko]

    dictionary_ko = corpora.Dictionary(texts_ko)  # initialize a dictionary
    dictionary_ko.save('ko.dict')  # save dictionary to file for future use

    # calulate TF-IDF

    tf_ko = [dictionary_ko.doc2bow(text) for text in texts_ko]
    tfidf_model_ko = models.TfidfModel(tf_ko)
    tfidf_ko = tfidf_model_ko[tf_ko]
    corpora.MmCorpus.serialize('ko.mm', tfidf_ko)  # save corpus to file for future use

    ntopics, nwords = 5, 4

    np.random.seed(42)

    # Train Topic Model
    # LDA

    np.random.seed(42)  # optional
    lda_ko = models.ldamodel.LdaModel(tfidf_ko, id2word=dictionary_ko, num_topics=ntopics)
    final_result_list = []

    # 제목에서 명사 분리
    title = title_test.run(path)
    final_result_list = komoran.get_morphes_by_tags(title, tag_list=['NNG', 'NNP'])

    # 가장 확률 높은 토픽의 단어 출력
    topic_words = lda_ko.show_topics(num_topics=1, num_words=5, formatted=False)
    # 각 토픽들의 단어 출력
    word_topics = lda_ko.show_topics(num_topics=5, num_words=1, formatted=False)

    # 보기 좋게 바꾸기
    words_list = []

    # 각 리스트의 단어들 words_list에 모음
    for i in range(5):
        words_list.append(topic_words[0][1][i][0])
        words_list.append(word_topics[i][1][0][0])

    # 중복 제거
    words_list = list(set(words_list))
    
    i = 0
    while len(final_result_list) <= 10 and i < len(words_list):
        final_result_list.append(words_list[i])
        i += 1

    print(set(final_result_list))