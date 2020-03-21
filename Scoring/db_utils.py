import pymysql
# <usage>
# from db_utils import *
# create_db()
# create_table()
# insert_student('my email', 'password')
# select_student('my eamil')
# update_email('new email', 'old email')
# delete_student('my email')


def db_query(db, sql, params):
    # Connect to MySQL
    conn = pymysql.connect(
        host='localhost',
        user='dev_yeen',
        # password='gracejo0227',
        password='*196B33199603EFD95395F1F4AB4DCEDB82A56479', # 1045 error ;;;;;도대체 해결책이 뭘까낭
        charset='utf8',
        db=db
    )
    try:
        # create Dictionary Cursor
        with conn.cursor() as cursor:
            sql_query = sql
            # excute SQL
            cursor.execute(sql_query, params)
        # commit data

        conn.commit()
    finally:
        conn.close()


def create_db():
    # CREATE adcs DB
    sql = 'CREATE DATABASE adcs'
    print('db creating process')
    db_query(db=None, sql=sql, params=None)

 # def insert_document(path, content_tag, form_tag ):
        #     sql = 'INSERT INTO student (path, content_tag, form_tag) VALUES (%s, %s)'
        #     params = (path, content_tag, form_tag)
        #     db_query(db='adcs', sql=sql, params=params)

def create_table_content():
    # CREATE content table
    sql = '''
        CREATE TABLE content (
            ID BIGINT NOT NULL, -- 문서 고유 번호 
            CONTENT_TAG VARCHAR(20) NOT NULL, -- 내용태그 
            PRIMARY KEY(ID,CONTENT_TAG),
            CONSTRAINT ‘fk_id’ 
            FOREIGN KEY (ID) REFERENCES DOCUMENT(ID) 
            ON DELETE CASCADE 
        ) ; 
    '''
    db_query(db='adcs', sql=sql, params=None)

def create_table_document():
    # CREATE document table
    sql = '''
        CREATE TABLE IF NOT EXISTS document (
        ID  BIGINT NOT NULL PRIMARY KEY,
        NAME VARCHAR(255) NOT NULL,
        TYPE_TAG VARCHAR(15) NOT NULL,
        PATH VARCHAR(1000) NOT NULL
         ) ;
    '''
    db_query(db='adcs', sql=sql, params=None)
# def create_table():
#     # CREATE student table
#     sql = '''
#         CREATE TABLE student (
#             id int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
#             email varchar(255) NOT NULL,
#             password varchar(255) NOT NULL
#         ) ENGINE=InnoDB DEFAULT CHARSET=utf8
#     '''
#     db_query(db='adcs', sql=sql, params=None)


# def insert_student(email, password):
#     sql = 'INSERT INTO student (email, password) VALUES (%s, %s)'
#     params = (email, password)
#     db_query(db='adcs', sql=sql, params=params)


def insert_content_tag(path, content_tag ):
    sql = 'INSERT INTO content (path, content_tag, form_tag) VALUES (%s, %s)'
    params = (path, content_tag)
    db_query(db='adcs', sql=sql, params=params)

def insert_document_tag(path, form_tag ):
    sql = 'INSERT INTO document (path, content_tag, form_tag) VALUES (%s, %s)'
    params = (path, form_tag)
    db_query(db='adcs', sql=sql, params=params)

#def select_document(email):
def select_document(form_tag):
    conn = pymysql.connect(
        host='localhost',
        user='root',
        password='ewhayeeun',
        charset='utf8',
        db='adcs'
    )
    # sql = 'SELECT * FROM document WHERE email = %s'
    sql = 'SELECT * FROM document WHERE email = %form_tag '
    params = (form_tag,)

    try:
        with conn.cursor() as cursor:
            cursor.execute(sql, params)
            result = cursor.fetchone()
            print(result)
        conn.commit()
    finally:
        conn.close()
def select_content(email):
    conn = pymysql.connect(
        host='localhost',
        user='root',
        password='ewhayeeun',
        charset='utf8',
        db='adcs'
    )
    # sql = 'SELECT * FROM document WHERE email = %s'
    sql = 'SELECT * FROM content WHERE email = %content_tag '
    params = (email,)

    try:
        with conn.cursor() as cursor:
            cursor.execute(sql, params)
            result = cursor.fetchone()
            print(result)
        conn.commit()
    finally:
        conn.close()

def update_email(new, old):
    sql = 'UPDATE document SET email = %s WHERE email = %s'
    params = (new, old)
    db_query(db='adcs', sql=sql, params=params)


def delete_student(email):
    sql = 'DELETE FROM document WHERE email = %s'
    params = (email,)
    db_query(db='adcs', sql=sql, params=params)

def show_student():
    sql = 'SELECT * from document ORDER BY id ASC'
    params = ()
    db_query(db='document', sql=sql, params=params)