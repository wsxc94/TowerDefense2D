# TowerDefense
유니티 2D 타워디펜스

## 게임 설명
---
- 타워와 몬스터는 각각의 특성을 가지고 있다. ( 불 , 얼음 , 번개 , 독)
- 타워는 특성에 따른 디버프를 가지고 있으며 몬스터의 특성에 따라 디버프 확률이나 데미지에 영향이 갈 수 있다.
- 타워 설치 및 판매 그리고 업그레이드로 웨이브내의 몬스터들을 처치하여 많은 점수를 얻는다.
- 생명이 0이 되면 게임 오버가 되며 아이디 입력 및 랭킹 확인 가능
---
![시작](https://user-images.githubusercontent.com/43703023/107147664-92903c80-6992-11eb-84d0-ad1d6bb61526.PNG)
![플레이](https://user-images.githubusercontent.com/43703023/91195003-9bb68580-e733-11ea-88ab-6f739803f21f.gif)
![옵션화면](https://user-images.githubusercontent.com/43703023/91195008-9c4f1c00-e733-11ea-9228-41fb13772a24.PNG)

![gameover](https://user-images.githubusercontent.com/43703023/107147661-915f0f80-6992-11eb-9104-9cc05a8de7ac.PNG)
![id](https://user-images.githubusercontent.com/43703023/107147662-91f7a600-6992-11eb-8667-888b2f1636ce.PNG)
![rank](https://user-images.githubusercontent.com/43703023/107147663-92903c80-6992-11eb-8ad4-112b6aa32a9c.PNG)

# 사용 기술
---
- 오브젝트 풀링을 사용해 풀링 오브젝트 관리
- 싱글톤으로 매니저관리
- 몬스터 - 추상화, List를 통한 디버프 관리
- 타워 - 추상화를 통한 각 타워마다 능력 구현 , Queue를 이용한 몬스터 공격 순서 관리 
- SQL DataBase를 이용한 랭킹 시스템
- UI와 EventSystem을 이용하여 마우스와 타일 상호작용 (타워 설치 , 업그레이드 , )

# 다운로드 및 사용법
---
- 링크 https://drive.google.com/file/d/1cbRJrFa70i01SV5FX8s6rk8u2F-Jy4HA/view?usp=sharing
- 다운로드 및 압축해제 후 TowerDefense2D.exe 로 실행 


# 개발 환경
---
* Unity 2019.4.16f1 (64-bit)
* Visual Studio 2017
* SQL LITE (DataBase)
