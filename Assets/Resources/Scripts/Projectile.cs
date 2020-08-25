using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Monster target; //몬스터

    private Tower parent; //타워

    private Element elementType; //속성
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        MoveToTarget(); //타겟으로 이동
    }
    public void Initialized(Tower parent) //초기화
    {
        this.target = parent.Target;

        this.parent = parent;

        this.elementType = parent.ElementType;
    }
    private void MoveToTarget() //타겟 이동
    {
        if (target != null && target.IsActive)
        {
            //현재 좌표에서 몬스터가 있는 좌표로 이동
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
            //dir = 방향
            Vector2 dir = target.transform.position - transform.position;
            //날아가면서 각도 조정
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //회전값 조정
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (!target.IsActive)
        {
            //오브젝트 풀에 적재
            GameManager.Instance.Pool.ReleaseObject(gameObject);
            //Destroy(this.gameObject);
        }
    }
    private void ApplyDebuff()
    {
        if (target.ElementType != elementType) //몬스터 속성타입과 타워 속성값이 같지 않다면
        {
            float roll = Random.Range(0, 100);
            if (roll <= parent.Proc) //확률계산후
            {
                target.AddDebuff(parent.GetDebuff()); //몬스터에게 디버프 적용
            }
        }

        target.AddDebuff(parent.GetDebuff());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster") //몬스터에게 닿으면
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage,elementType); //데미지 적용
                ApplyDebuff(); //디버프 적용
                GameManager.Instance.Pool.ReleaseObject(gameObject); //오브젝트풀에 적재
            }
            
       
        }
    }
    
}
