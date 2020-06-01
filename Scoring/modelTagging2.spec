# -*- mode: python ; coding: utf-8 -*-

block_cipher = None


a = Analysis(['modelTagging2.py'],
             pathex=['C:\\Users\\소정\\Desktop\\졸업프로젝트\\AutomaticDocumentClassificationService\\Scoring'],
             binaries=[],
             datas=[("C:/Temp/Anaconda3/envs/tensorflow/Lib/site-packages/py4j/","./py4j"),("C:/Temp/Anaconda3/envs/tensorflow//Lib/site-packages/konlpy/", "./konlpy"), ("C:/Temp/Anaconda3/envs/tensorflow//Lib/site-packages/konlpy/java/", "./konlpy/java"), ("C:/Temp/Anaconda3/envs/tensorflow//Lib/site-packages/konlpy/data/tagset/*", "./konlpy/data/tagset"),],
             hiddenimports=['sklearn.utils._cython_blas', 'sklearn.neighbors.typedefs', 'sklearn.neighbors.quad_tree', 'sklearn.tree._utils'],
             hookspath=[],
             runtime_hooks=[],
             excludes=[],
             win_no_prefer_redirects=False,
             win_private_assemblies=False,
             cipher=block_cipher,
             noarchive=False)
pyz = PYZ(a.pure, a.zipped_data,
             cipher=block_cipher)
exe = EXE(pyz,
          a.scripts,
          a.binaries,
          a.zipfiles,
          a.datas,
          [],
          name='modelTagging2',
          debug=False,
          bootloader_ignore_signals=False,
          strip=False,
          upx=True,
          upx_exclude=[],
          runtime_tmpdir=None,
          console=True )
