using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {STORM,FIRE,ICE,POISON,NONE }
public abstract class Tower : MonoBehaviour {
    [SerializeField]
    private string projectileType = null;  //발사체 타입
    [SerializeField]
    private float projectileSpeed = 0;   //발사체 속도
    [SerializeField]
    private float debuffDuration;   //디버프 지속시간
    [SerializeField]
    private float proc;             //디버프가 걸릴 확률
    private Animator myAnimator;    //타워 애니메이터
    [SerializeField]
    private int damage;              //타워 데미지
    public int Price { get; set; }    //가격
    public Element ElementType { get; protected set; } //속성 타입
    public int Damage { get { return damage; } } //데미지 프로퍼티
    public int Level { get; protected set; } //타워 현재 레벨
    
                                                                      
    public float ProjectileSpeed //발사체 속도 프로퍼티
    {
        get
        {
            return projectileSpeed;
        } 
    }

    private SpriteRenderer mySpriteRenderer; //이미지 렌더러

    public Monster Target //공격하는 타겟 프로퍼티
    { get; private set; }

    public float DebuffDuration  //디버프 지속시간 프로퍼티
    {
        get
        {
            return debuffDuration;
        }

        set
        {
            this.debuffDuration = value;
        }
    }

    public float Proc //확률 프로퍼티
    {
        get
        {
            return proc;
        }

        set
        {
            proc = value;
        }
    }
    public TowerUpgrade NextUpgrade //다음 업그레이드 플로퍼티
    {
        get
        {
            if (Upgrades.Length > Level -1)
            {
                return Upgrades[Level - 1];
            }
            return null;
        }
       
    }
    private Queue<Monster> monsters = new Queue<Monster>();  //타워 공격 우선순위를 위한 몬스터 큐리스트 선언

    private bool canAttack = true; //공격을 할수있는지 없는지 판별

    public TowerUpgrade[] Upgrades { get; protected set; } //타워 업그레이드 프로퍼티

 
    private float attackTimer; //어택 딜레이

    [SerializeField]
    private float attackCooldown = 0.0f; //어택 쿨타임
	// Use this for initialization
	void Awake ()
    {
        
        myAnimator = transform.parent.GetComponent<Animator>(); //애니메이터 컴포넌트 취득

        mySpriteRenderer = GetComponent<SpriteRenderer>(); //이미지렌더러 취득

        Level = 1; //레벨 초기화

    }
  
    // Update is called once per frame
    void Update () {
        AttackDelay();
        Attack(); //공격
	}
    public void Select() //선택
    {
        //타워를 선택했을때 툴팁이 나옴
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;  
        GameManager.Instance.UpdateUpgradeTip();

    }
    private void AttackDelay()
    {
        if (!canAttack) //공격을 할수없을시에
        {
            attackTimer += Time.deltaTime; //어택타이머는 시간의 흐름에 비례

            if (attackTimer >= attackCooldown) //타이머가 쿨타임보다 크면
            {
                canAttack = true; //공격 가능
                attackTimer = 0; //타이머 초기화
            }

        }

    }
    private void Attack()
    {

        foreach (Monster item in monsters)
        {
            Debug.Log(item);
        }
            if (Target != null && Target.IsActive) //타겟몬스터가 활성화됬을시에
            {
                if (canAttack)
                {
                    Shoot(); //타워 공격

                    myAnimator.SetTrigger("Attack"); //공격 애니메이션으로 변경

                    canAttack = false;
                }

            }

            if ((Target == null && monsters.Count > 0)) //공격 타겟이 사라지고 , 몬스터카운트가 0보다 크며 몬스터가 픽된경우
            {
                Target = monsters.Dequeue(); //몬스터 큐에서 뺌       
            }

            else if ((Target != null && !Target.IsActive))
            {
            //타겟이 죽은경우
           
                Target = null;

            }

        if (monsters.Count != 0 && (!monsters.Peek().IsActive || !monsters.Peek().InRange)) monsters.Dequeue();
        
    }

    private void Shoot() //발사체 발사
    {
        //발사체에 오브젝트풀 적용
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        projectile.transform.position = transform.position; //발사할 좌표 설정

        projectile.Initialized(this);  //발사체 초기화
    }

    public virtual string GetStats() //타워 스텟 툴팁
    {
        if (NextUpgrade != null)
        {
            return string.Format("\n레벨: {0} \n데미지: {1} <color=#00ff00ff> +{4}</color> \n확률: {2}% <color=#00ff00ff>+{5}%</color>\n지속시간: {3}초 <color=#00ff00ff>+{6}</color>", Level, Damage, Proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }
        else
        {
            return string.Format("\n레벨: {0} \n데미지: {1} \n디버프확률: {2}% \n지속시간: {3}초", Level, Damage, Proc, DebuffDuration);
        }
    }
    
        
    public virtual void Upgrade() //업그레이드
    {
        GameManager.Instance.Currency -= NextUpgrade.Price; //돈빠져나감
        Price += NextUpgrade.Price; //가격올라감
        this.damage += NextUpgrade.Damage; //데미지 올라감
        this.proc += NextUpgrade.ProcChance; //확률 올라감
        this.DebuffDuration += NextUpgrade.DebuffDuration; //디버프 지속시간 올라감
        Level++; //레벨 올라감
        GameManager.Instance.UpdateUpgradeTip(); //업그레이드 툴팁 업데이트
    }
    private void OnTriggerEnter2D(Collider2D other) //충돌체 트리거 함수
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<Monster>().InRange = true;
            monsters.Enqueue(other.GetComponent<Monster>());
           
        }
    }
   
    private void OnTriggerExit2D(Collider2D other) //충돌체 트리거 함수
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<Monster>().InRange = false;
            Target = null; 
        }
    }

    public abstract Debuff GetDebuff(); //디버프 적용
}
