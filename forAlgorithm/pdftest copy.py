import PyPDF2

pdf_file = open("ch06 Counting.pdf", "rb")                                       # 로컬 PC에 있는 pdf 파일도 읽을 수 있음

pdfReader = PyPDF2.PdfFileReader(pdf_file)
count = pdfReader.numPages
for i in range(count):
    page = pdfReader.getPage(i)
    print(page.extractText())
