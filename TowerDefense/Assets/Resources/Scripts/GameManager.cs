using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void CurrencyChanged(); //가격을 바꿔주는 이벤트 델리게이트

public class GameManager : Singleton<GameManager> //게임매니저 싱글턴 오브젝트화
{
    public event CurrencyChanged Changed; // 가격 바꿔주는 트리거변수

    public TowerBtn ClickedBtn { get; set; } //타워UI버튼

    private int currency; //돈

    public int wave = 0; //몬스터 웨이브

    private int lives; // 유저 hp
    [SerializeField]
    private Text livesTxt=null; //hp 텍스트

    [SerializeField]
    private Text waveTxt=null; //wave 텍스트

    [SerializeField]
    private GameObject waveBtn=null; // wave 버튼

    [SerializeField]
    private Text currencyTxt=null; // 돈텍스트

    private bool gameOver = false; // 게임오버가 아님으로 시작

    private int health = 15; // 몬스터 hp

    [SerializeField]
    private GameObject gameOverMenu=null; //게임 오버 메뉴
    [SerializeField]
    private GameObject upgradePanel=null; //타워 업그레이드 UI 패널
    [SerializeField]
    private GameObject statsPanel=null; //타워 스텟 UI 패널
    
    [SerializeField]
    private Text statText=null; //타워 스텟 UI 텍스트 
    [SerializeField]
    private Text sellText=null; // 타워 판매 텍스트
    [SerializeField]
    private Text upgradePriceText=null; //업그레이드 가격 텍스트

    private Tower selectTower; //타워 셀렉

    public ObjectPool Pool { get; set; } // 오브젝트풀 프로퍼티
    [SerializeField]
    private GameObject inGameMenu=null; //인게임메뉴 오브젝트
    [SerializeField]
    private GameObject OptionMenu=null; //옵션메뉴 오브젝트

    public List<Monster> activeMonsters = new List<Monster>(); //몬스터 리스트

    public int Currency //유저 돈 프로퍼티
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value; 
            this.currencyTxt.text = value.ToString() + "<color=lime>$</color>"; //가격텍스트에 색을 더함

            OnCurrencyChanged(); //돈에 따라 타워UI의 색을 바꿔주는 함수 호출
        }
    }

    public int Lives //유저 hp 프로퍼티
    {
        get { return lives; }
        set
        {
            this.lives = value;
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
            
            livesTxt.text = value.ToString(); //현재 상수값을 문자열화 시켜 텍스트에 출력
        }
    }

    public bool WaveActive //wave 가 실행되고 있는지 아닌지 판별
    {
        get
        {
            return activeMonsters.Count > 0;
        }
    }

    private void Awake() // 게임 시작전 1번 호출되는 유니티엔진 기본함수
    {
        Pool = GetComponent<ObjectPool>();
    }
    // Use this for initialization
    void Start() // 게임 시작후 1번 호출되는 유니티엔진 기본함수
    {
        Lives = 10;
        Currency = 10;
    }

    
    void Update() // 프레임마다 계속 실행되는 유니티엔진 기본함수
    {
        HandlerEscape();
    }
    public void PickTower(TowerBtn towerBtn) // 타워 픽함수
    {
        if (Currency >= towerBtn.Price) // 현재 금액이 타워 금액보다 높고 웨이브가 진행중이지 않을때
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }

    }
    public void BuyTower() // 타워구매함수
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();  // 타워 선택을 안했을때로 돌아감
        }

    }
    public void OnCurrencyChanged() // 타워 UI색깔을 바꿔주는 함수
    {
        if (Changed != null)
        {
            Changed();

        }
    }
    public void SelectTower(Tower tower) // 타워 골랐을때 호출되는 함수
    {
        if (selectTower != null)
        {
            selectTower.Select();
        }
        selectTower = tower;
        selectTower.Select();
        sellText.text = "+ " + (selectTower.Price / 2).ToString() + " $";
        upgradePanel.SetActive(true);
    }
    public void DeselectTower() // 타워를 고르지 않았을때 호출되는 함수
    {
        if (selectTower != null)
        {
            selectTower.Select();
        }
        selectTower = null;
        upgradePanel.SetActive(false);
    }
    private void HandlerEscape() // 타워를 고르고 esc를 누르면 해제됨
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();  // 타워 선택을 안했을때로 돌아감
            ShowIngameMenu(); //일시정지 메뉴를 불러옴
        }
        if (Input.GetMouseButtonDown(1)) // 오른쪽마우스 클릭시
        {                       
            if (Hover.Instance.IsVisible) // 타워이미지가 있을때는
            {
                DropTower(); //타워선택 취소
            }
            else if (selectTower != null) // 타워선택중일땐
            {
                DeselectTower(); //타워 선택 취소
            }
            
        }
    }
    public void StartWave() //웨이브 시작함수
    {

        wave++; 

        waveTxt.text = string.Format("Wave : <color=lime>{0}</color>", wave); //웨이브 텍스트 초기화

        StartCoroutine(SpawnWave()); //몬스터 스폰 코루틴 호출

        waveBtn.SetActive(false); //웨이브 버튼 비활성화
    }
    private IEnumerator SpawnWave() 
        // 몬스터인덱스의 0번부터 3번까지 랜덤으로 고른후 타입을 판별하여 스폰
    {
        
        for (int i = 0; i < wave; i++) //웨이브 반복
        {
            int monsterIndex = Random.Range(0, 4); //몬스터 인덱스를 0~3 까지 랜덤으로 정함

            string type = string.Empty;

            switch (monsterIndex)

            { //몬스터인덱스의 string 타입으로 몬스터 판별
                case 0: type = "BlueMonster"; break;
                case 1: type = "RedMonster"; break;
                case 2: type = "GreenMonster"; break;
                case 3: type = "PurpleMonster"; break;    
                default:
                    break;
            }
            //오브젝트풀에서 몬스터를 불러옴
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();

            monster.Spawn(health);

            if (wave % 3 == 0) //웨이브 3개가 지날때마다 hp가 5씩 회복
            {
                health += 5;  
            }

            activeMonsters.Add(monster); //현재 활성화된 몬스터리스트에 몬스터 추가

            yield return new WaitForSeconds(1f); //1초 딜레이
        }
        

    }
    public void RemoveMonster(Monster monster) //몬스터 삭제함수
    {
        activeMonsters.Remove(monster); //리스트에서 몬스터 삭제
        if (!WaveActive && !gameOver) //웨이브가 진행중이 아니며 게임오버가 아닐시에
        {
            waveBtn.SetActive(true); //웨이브 시작버튼 활성화
        }
    }
    public void GameOver() // 게임오버 함수
    {
        if (!gameOver) //게임오버가 아닐시
        {
            gameOver = true; //게임오버 활성화
            gameOverMenu.SetActive(true); //게임오버 메뉴 활성화
        }
    }
    public void Restart() // 게임 재시작 함수
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame() //게임 메인화면 이동 함수
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void SellTower() // 타워 판매 함수
    {
        if (selectTower != null) // 타워가 선택되었을때
        {
            Currency += selectTower.Price / 2; //현재 타워 가격 / 2 의 가격으로 되팜
            selectTower.GetComponentInParent<TileScript>().IsEmpty = true; //타워가 있던 타일이 비어있음을 알림
            Destroy(selectTower.transform.parent.gameObject); //타워삭제
            DeselectTower(); //타워선택취소
        }
    }

    public void ShowStats() //타워 기본 스텟 UI
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
       
    }
    public void ShowSelectedTowerStats() //현재 선택 타워 스텟 UI
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTip();
    }
        
    public void SetTooltipText(string txt) //툴팁텍스트 세팅
    {
        statText.text = txt;

    }
    public void UpdateUpgradeTip()  //타워 판매,업그레이드 툴팁 UI
    {
        if (selectTower != null)
        {
            sellText.text = "+ " + (selectTower.Price / 2).ToString() + " $";
            SetTooltipText(selectTower.GetStats());
            if (selectTower.NextUpgrade != null)
            {
                upgradePriceText.text = selectTower.NextUpgrade.Price.ToString() + " $";
            }
            else
            {
                upgradePriceText.text = " MAX";
            }
        }
    }
    public void UpgradeTower() //타워 업그레이드
    {
        if (selectTower.Level <= selectTower.Upgrades.Length && Currency >= selectTower.NextUpgrade.Price)
        {
            selectTower.Upgrade();
        }
    }
    public void ShowIngameMenu() //인게임 일시정지 메뉴
    {
        if (OptionMenu.activeSelf)
        {
            ShowMain();
        }
        else
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf); 
            //일시정지중엔 게임이 진행되지 않음
            if (!inGameMenu.activeSelf)
            {
                Time.timeScale = 1;

            }
            else
            {
                Time.timeScale = 0;
            }
        }
        
    }
    public void DropTower() //타워 선택취소
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }
    public void ShowOptions() //옵션출력
    {
        inGameMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }
    public void ShowMain() //기본 메뉴로 돌아오기
    {
        inGameMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }
}
