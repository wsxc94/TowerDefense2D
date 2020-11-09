using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField]
    private float speed; //스피드

    public float MaxSpeed { get; set; } //최대 스피드

    private List<Debuff> debuffs = new List<Debuff>(); //디버프 리스트

    private List<Debuff> debuffsToRemove = new List<Debuff>(); //디버프 삭제 리스트

    private List<Debuff> newDebuffs = new List<Debuff>(); //새로운 디버프 리스트

    [SerializeField]
    private Element elementType; //속성 타입

    private SpriteRenderer spriteRenderer; //이미지렌더러

    private int invulnerability = 2;  //데미지 감소

    private Animator myAnimator; //애니메이터

    [SerializeField]
    private Stat health = null; //생명

    public bool Alive //살아있는지 판별하는 프로퍼티
    {
        get
        {
            return health.CurrentValue > 0;
        }
    }

    public Point GridPosition { get; set; } //좌표
  
    public bool IsActive { get; set; } 

    public Element ElementType //속성 타입 프로퍼티
    {
        get
        {
            return elementType;
        }
   
    }

    public float Speed //스피드 프로퍼티
    {
        get
        {
            return speed;
        }
        set
        {
            this.speed = value;
        }


    }

    private void Awake() //몬스터가 나오기전에 한번 실행
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxSpeed = speed;
        health.Initialize();
    }
    private void Update() //프레임마다 업데이트
    {
        HandleDebuffs(); //디버프 적용
        
    }

    public void Spawn(int health) //몬스터 스폰
    {
       
        //블루 포탈 위치에서 몬스터 스폰
        transform.position = LevelManager.Instance.BluePortal.transform.position;
        this.health.Bar.Reset();
        this.health.MaxVal = health;
        this.health.CurrentValue = this.health.MaxVal;
        //몬스터 스케일업 코루틴 실행
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false, 0f));

       // SetPath(LevelManager.Instance.Path); //몬스터 경로설정
    }
    public IEnumerator Scale(Vector3 from , Vector3 to, bool remove,float progress) 
        //몬스터 스폰 스케일업 효과
    {

        speed = 1f;
        while (progress <= 1f)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
        speed = MaxSpeed;
        IsActive = true;

        if (remove) //삭제
        {
            Release();
        }
    }
 
    private void OnTriggerEnter2D(Collider2D other) //충돌체 판별
    {
        if (other.tag == "BluePortal")
        {
            other.GetComponent<Portal>().Open(); //포탈열림
        }
        if (other.tag =="RedPortal")
        {
            //몬스터 크기가 작아지며 포탈안에 들어가 사라짐
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f),true,0f));
            
            other.GetComponent<Portal>().Open();
            
            GameManager.Instance.Lives--; //플레이어 생명 깎임
        }
        if (other.tag == "Tile")
        {
            //항상 타일 위를 걸어다니도록 sortingOrder 설정
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.Y;
        }
    }
    public void Release()
    {
        debuffs.Clear(); //디버프 제거
        Mover target = GetComponent<Mover>(); //몬스터 이동 스크립트 컴포넌트 취득
        //타겟 초기화
        target.currentIndex = 0; 
        target.currentWaypoint = LevelManager.Instance.waypoints[0];
        
        IsActive = false;

        GridPosition = LevelManager.Instance.BlueSpawn; //시작 위치
       
        GameManager.Instance.RemoveMonster(this); //몬스터 삭제

        GameManager.Instance.Pool.ReleaseObject(gameObject); // 오브젝트풀
    }
    public void TakeDamage(float damage, Element damageSource) //데미지함수
    {
        if (IsActive)
        {
            if (damageSource == ElementType) //만약 몬스터의 속성과 타워 속성이 맞으면
            {
                damage = damage / invulnerability; //발사체 데미지 감소
                
            }
            health.CurrentValue -= damage; //hp 깎임
            if (health.CurrentValue <= 0)
            {
               //SoundManager.Instance.PlaySFX("오디오파일이름")
                GameManager.Instance.Currency += (GameManager.Instance.wave); //몬스터 wave에 비례하여 돈 추가
                myAnimator.SetTrigger("Die"); //몬스터 사망 애니메이션
                IsActive = false;
                GetComponent<SpriteRenderer>().sortingOrder--; 
            }
        }
        
    }
    public void AddDebuff(Debuff debuff)
    {
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType())) 
            //몬스터 타입과 타워의 타입이 같으면 디버프가 들어가지않음
        {
            newDebuffs.Add(debuff); 
        }
       
    }
    public void RemoveDebuff(Debuff debuff) //디버프삭제
    {
 
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs() 
    {
        if (newDebuffs.Count > 0) //새로운 디버프 리스트 카운트가 0보다 크면
        {
            debuffs.AddRange(newDebuffs); //디버프에 new 디버프 추가

            newDebuffs.Clear(); //새로운 디버프 리스트 삭제 
        }
        foreach (Debuff debuff in debuffsToRemove) //디버프 삭제 리스트 순회
        {
            debuffs.Remove(debuff);
            
        }
        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs) //디버프 리스트 순회
        {
            debuff.Updata();
        }
    }
}
