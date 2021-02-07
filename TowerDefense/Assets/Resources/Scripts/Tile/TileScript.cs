using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {
    [SerializeField]
    private string tiletype=null; //타일 타입

    public Point GridPosition { get; private set; } //타일 포지션

    public bool IsEmpty { get; set; } //비어있나 비어있지않나 확인

    private Tower myTower; //설치된 타워

    private Color32 fullColor = new Color32(255, 118, 118, 255); //빨간색

    private Color32 emptyColor = new Color32(96, 255, 90, 255); // 비어있는 색

    private SpriteRenderer spriteRenderer;

    public bool WalkAble { get; set; } //가동중인지

    public bool Debugging { get; set; }

    public Vector2 WorldPosition //타일을 설치할 월드포지션
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
        set { }
    }
        // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>(); //타일이미지 컴포넌트 취득
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Setup(Point gridPos,Vector3 worldPos,Transform parent)
    {
        //레벨매니저에서 사용할 타일 포지션 set up 
        WalkAble = true;
 
        IsEmpty = true;

        this.GridPosition = gridPos;

        transform.position = worldPos;

        transform.SetParent(parent);

        LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    private void OnMouseOver() //타워를 선택했을때 마우스에 대한 함수
    {
        
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null && tiletype != "monsterTile")
            //마우스 포인터가 사용되는중이며 클릭버튼이 비어있지않으며 타일의 타입이 몬스터가 지나가는 타일이 아니면 진행
        {
            
            if (IsEmpty && !Debugging )
            {
                ColorTile(emptyColor); //초록색
            }
            if(!IsEmpty && !Debugging )
            {
                ColorTile(fullColor); //빨간색
            }

            else if (Input.GetMouseButtonDown(0)) //마우스 왼쪽버튼을 누르면
            {
                PlaceTower(); //타워 설치 
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        { // 위에꺼 반대 자신이 설치한 타워

            if (myTower != null) 
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
        
    }
    private void OnMouseExit() //타워 선택을 하지않고 타일에 마우스 포인터를 올리면
    {
        if (!Debugging) //아무색으로 변하지않음
        {
            ColorTile(Color.white);
        }
        
    }
    private void PlaceTower() //타워 설치
    {
        //타워 생성
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        //타워 렌더러의 sortingOrder를 타일 y 포지션에 비례하여 설정
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        //타워를 타일의 자식으로 만듬
        tower.transform.SetParent(transform);

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
        //타일이 비어있지않음 설정
        IsEmpty = false;

        ColorTile(Color.white);
        //설치된 타워 가격 설정
        myTower.Price = GameManager.Instance.ClickedBtn.Price;
        //타워 구매 함수 호출
        GameManager.Instance.BuyTower();

        WalkAble = false;
    }
    public void ColorTile(Color newColor) //타일 색깔 설정
    {
        spriteRenderer.color = newColor;
    }
}
