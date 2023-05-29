# :pushpin: Roomus
메타버스 아카데미 융합프로젝트 메타버스 인테리어 플랫폼

</br>

![](https://velog.velcdn.com/images/kjhdx/post/8107c8a4-ca7e-4476-90dd-8a434600b03b/image.png)


![](https://velog.velcdn.com/images/kjhdx/post/3c1b836b-266c-44ab-9291-2ee4c8c22595/image.png)


</br>

## 1. 제작 기간 & 참여 인원
- 2022년 10월 ~ 12월 (2달)
- 유니티 개발자 2명
- 서버 개발자 2명
- 3D 모델러 1명
- AI 개발자 1명

</br>

## 2. 사용 기술
 - <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white"> 
 - <img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=unity&logoColor=black"> 
 - <img src="https://img.shields.io/badge/git-F05032?style=for-the-badge&logo=git&logoColor=white">
 - <img src="https://img.shields.io/badge/photon Engine-4285F4?style=for-the-badge&logo=photon&logoColor=white">
 - <img src="https://img.shields.io/badge/Fast API-009688?style=for-the-badge&logo=fastapi&logoColor=white">
 - <img src="https://img.shields.io/badge/JSON-000000?style=for-the-badge&logo=json&logoColor=white">
 
</br>


## 3. 핵심 기능 및 상세 역할 :pushpin: [코드 확인](https://github.com/MightyChipmunk/Roomus/tree/main/Assets/Scripts)
- 유저가 가구 모델링을 서버에 업로드하고, 남들이 업로드 한 가구를 사용해 방을 배치해보고 그 방을 업로드하는 인테리어 전용 메타버스 플랫폼입니다.

- DB에 FBX 파일과 JSON 파일을 저장하고 불러오면서 가구 모델링과 방의 배치를 서버에서 관리합니다.

- FAST API를 통한 통신으로 DB에 접근하고, 그 외에 실시간 동기화가 필요한 부분은 포톤 엔진을 사용하여 구현하였습니다.



<details>
<summary><b>핵심 기능 설명 펼치기</b></summary>
<div markdown="1">

  
### 3.1 FBX 파일(가구 모델링) 업로드

- FBX 업로드
  - FBX 파일을 선택해서 세부 정보를 입력하여 가구를 업로드 할 수 있습니다.

    ![](https://velog.velcdn.com/images/kjhdx/post/ec66fafb-6952-47a6-befa-45171776dec9/image.png)
    ![](https://velog.velcdn.com/images/kjhdx/post/7dd59750-faf5-4147-bc3d-1e64a8e7040d/image.png)

### 3.2 가구 배치

- 가구 배치
  - 자신과 남들이 올린 가구를 선택하여 방에 배치해 볼 수 있습니다.
  - FBX 파일을 빌드 이후에 인스턴스화 하기 위해 Trilib2 라는 애셋을 사용해 실시간으로 FBX파일을 인스턴스화 했습니다.
   
    ![](https://velog.velcdn.com/images/kjhdx/post/4f6fd94e-1167-42de-bccf-5c0475292c30/image.png)
    ![](https://velog.velcdn.com/images/kjhdx/post/b0360127-ad45-4af9-8c32-e3728e51a758/image.png)

### 3.3 라이팅/조명 설정

- 라이팅 설정
  - 방에 조명을 밝기와 거리, 색 등을 조정하여 배치할 수 있습니다.
  
    ![](https://velog.velcdn.com/images/kjhdx/post/b985b142-8edf-4b6f-8e96-161c788c5724/image.png)
  
- 조명 설정
  - 방의 전체적인 색감 조정을 유니티의 포스트 프로세싱을 사용하여 조정 할 수 있습니다.
  
    ![](https://velog.velcdn.com/images/kjhdx/post/b9794391-aba8-472a-ae8a-2db2d8e6748a/image.png)
  
### 3.4 방 입장 및 실시간 멀티플레이

- 방 입장
  - 가구와 조명을 배치한 방을 업로드하면 SNS처럼 남들에게 공개되고, 그 방을 선택해서 입장할 수 있습니다.
  
    ![](https://velog.velcdn.com/images/kjhdx/post/3ffeed41-726f-4d08-933b-57d8a20e0323/image.png)
- 실시간 멀티플레이
  - 입장한 방에서는 다른 유저들과 실시간으로 소통할 수 있습니다.
   
    ![](https://velog.velcdn.com/images/kjhdx/post/62a022c5-a7f9-4b56-91d4-a2febf5376b7/image.png)



### 3. 상세 역할
- 한명은 UI 디자이너를 겸하며 UI의 디자인과 UI 애니메이션 및 UI 배치를 담당하였고, 저는 그 외 모든 유니티 클라이언트 상에서의 기능을 담당했습니다.

</div>
</details>

</br>

## 4. 회고 / 느낀점

- 서버를 통한 DB 접근을 해보면서 JSON의 파싱과 FAST API를 통한 통신에 익숙해졌습니다.

- 6명이라는 인원과 작업을 해본건 처음이었는데, 서버 개발자 중 한명과 지속적으로 의견 마찰이 있었습니다. 그런 상황 속에서도 어떻게든 의견을 하나로 좁혀보려고 노력하였고 지속적으로 소통하였습니다. 결국에는 서비스에 필요한 서버와 API 기능들을 모두 구현하였고 좋은 결과물을 얻었다고 생각합니다. 

</br>

## 5. 시연 영상
https://www.youtube.com/watch?v=6kUckOc9ldQ
