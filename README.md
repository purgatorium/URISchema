# URISchema
웹페이지에서 URI스키마를 사용한 외부 프로세스 실행
https://msdn.microsoft.com/en-us/library/aa767914(v=vs.85).aspx

라이센스: [MLPL](https://msdn.microsoft.com/ko-kr/cc300389.aspx)

2015.12.01
- 회사 작업중 웹에서 exe파일을 실행해야 할 일이 생겼습니다.
예전에 게임회사 다닐때 개발했던 웹런처(ActiveX)를 사용하려했는데 IE만 지원되는 단점이 있네요.
- 각 브라우저별로 플러그인을 만들어야 하나 고민했는데 잘 생각해보니 mailto 같은걸 이미 잘 사용하고 있잖아요...
역시 검색해보니 내맘대로 만들어서 작업할 수 있네요.

- 프로젝트 두개와 샘플 html이 있습니다.
 1. RegisterURISchema: mytro.RecPlayer 라는 스키마를 레지스트리(HKLM)에 등록하는 프로그램
 2. CustomURISchemaTest: 전달받은 파라미터를 URLDecode해서 화면에 보여주는 테스트 프로그램
 3. uri_test.html: 테스트 웹페이지

- 윈도우 XP는 지원안합니다 (.NET 4.0)


```
파라미터 한개 전달 (46dc669b-bc52-4ab9-b7b8-439488e05274)
<a href='mytro.RecPlayer:"뷁46dc669b-bc52-4ab9-b7b8-439488e05274"'>클릭시 플레이어가 실행됩니다.</a>

파라미터 세개 전달 (둘리, 또치, 마이콜)
<a href='mytro.RecPlayer:"뷁둘리 또치 마이콜"'>클릭시 플레이어가 실행됩니다.</a>
```

테스트한 브라우저
- [ ] 마소 IE 9
- [ ] 마소 IE 10
- [x] 마소 IE 11
- [x] 마소 엣지
- [x] 구글 크롬
- [ ] 불여우
- [ ] 사파리

테스트한 환경
- [x] 윈도우 7
- [ ] 윈도우 8
- [ ] 윈도우 8.1
- [x] 윈도우 10
