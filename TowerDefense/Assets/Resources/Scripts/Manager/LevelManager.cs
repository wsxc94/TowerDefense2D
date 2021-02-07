using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager> { // 싱글턴 오브젝트화
    //serializeFiel란 private만 사용할시엔 외부로 접근할수 없을 뿐만이 아니라 inspector창에서도 값을 지정해 줄수없다 하지만 이걸 사용하면 inspector창에서는 보이게 된다. 개발자만 접근할수 있게 해준다.
    [SerializeField]  
    private GameObject[] tilePrefabs=null; //타일 프리팹 오브젝트 배열

    public Waypoint[] waypoints;

    private Point waypointSpawn;

    [SerializeField]
    private CameraMovement cameraMovement = null; //카메라 이동

    [SerializeField]
    private Transform map =null; //타일들을 쑤셔넣을 map

    private Point blueSpawn,redSpawn; //레드포탈 스폰할 좌표

    [SerializeField]
    private GameObject bluePortalPrefab=null; //블루 포탈 프리팹

    [SerializeField]
    private GameObject redPortalPrefab=null; //레드 포탈 프리팹

    public Portal BluePortal { get; set; } //블루 포탈 프로퍼티

    private Point mapSize; //맵크기

    public Dictionary<Point,TileScript> Tiles { get; set; } //타일을 추가 검색 삭제 인덱스 접근을 할수있도록 딕셔너리화

    public float TileSize //타일 사이즈 프로퍼티
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
}

    public Point BlueSpawn //블루 포탈 스폰 프로퍼티
    {
        get
        {
            return blueSpawn;
        }

    }

    public Point WaypointSpawn
    {
        get
        {
            return waypointSpawn;
        }

     
    }

    private void Awake()
    {
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Screen.SetResolution(300, 600, false);
    }
    // Use this for initialization
    void Start () {
        CreateLevel(); //타일 렌더링
	}
	
	// Update is called once per frame
	void Update () {

	}
    private void CreateLevel() //타일 렌더링
    {
        Tiles = new Dictionary<Point, TileScript>(); //타일 딕셔너리 선언

        string[] mapData = ReadLevelText(); //텍스트에 있는 문자들을 맵데이터 문자열에 넣음

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);  //맵사이즈는 텍스트 x축길이와 y축 텍스트길이로 설정

        int mapX = mapData[0].ToCharArray().Length; //맵데이터 문자길이로 mapX설정
        int mapY = mapData.Length; // y길이

        Vector3 maxTile = Vector3.zero; //maxtile = vector3(0,0,0);

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)); //화면의 높이

        for (int y = 0; y < mapY; y++) //x,y 길이를 순회하며 타일 렌더링
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(),x,y,worldStart);
            }
        }
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position; //최대타일 좌표 설정

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize)); //카메라 이동 설정 셋

        SpawnPortals(); //포탈 생성
    }
    private void PlaceTile(string tileType,int x,int y,Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType); //문자열을 숫자로 변환
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();  //새로운타일은 타일스크립트 컴포넌트를 취득한 타일프리팹의 인덱스
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0),map); //새로운 타일을 설치할 좌표 계산

    }
    private string[] ReadLevelText() //레벨 텍스트 읽기
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }
    private void SpawnPortals() //포탈 생성
    {
        WayPointsSpawn();
        blueSpawn = new Point(1, 1); //  블루포탈이 생성될 좌표 x = 1 , y = 1 에 포탈 생성
        GameObject tmp = Instantiate(bluePortalPrefab, Tiles[BlueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity); //게임오브젝트 tmp는 블루포탈생성 오브젝트
        BluePortal = tmp.GetComponent<Portal>(); //블루 포탈은 포탈스크립트의 컴포넌트를 취득한 tmp
        BluePortal.name = "BluePortal"; //네임 셋
        redSpawn = new Point(6, 9);   // 레드포탈이 생성될 좌표 x = 6 , y = 9
        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
    public bool InBounds(Point position)
    {
        //좌표를 받고 오퍼레이터로 비교해 반환
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }
   
    private void WayPointsSpawn()
    {
        waypointSpawn = new Point(6, 1);
        Waypoint way1 = Instantiate(waypoints[0], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[0].transform.position = way1.transform.position;

        waypointSpawn = new Point(6, 3);
        Waypoint way2 =Instantiate(waypoints[1], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[1].transform.position = way2.transform.position;

        waypointSpawn = new Point(1, 3);
        Waypoint way3 =Instantiate(waypoints[2], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[2].transform.position = way3.transform.position;

        waypointSpawn = new Point(1, 5);
        Waypoint way4 =Instantiate(waypoints[3], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[3].transform.position = way4.transform.position;

        waypointSpawn = new Point(6, 5);
        Waypoint way5 =Instantiate(waypoints[4], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[4].transform.position = way5.transform.position;

        waypointSpawn = new Point(6, 7);
        Waypoint way6 =Instantiate(waypoints[5], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[5].transform.position = way6.transform.position;

        waypointSpawn = new Point(1, 7);
        Waypoint way7 = Instantiate(waypoints[6], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[6].transform.position = way7.transform.position;

        waypointSpawn = new Point(1, 9);
        Waypoint way8 =Instantiate(waypoints[7], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[7].transform.position = way8.transform.position;

        waypointSpawn = new Point(6, 9);
        Waypoint way9 = Instantiate(waypoints[8], Tiles[WaypointSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        waypoints[8].transform.position = way9.transform.position;
    }
}
