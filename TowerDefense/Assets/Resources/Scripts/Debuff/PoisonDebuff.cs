using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff {

    
    private float timeSinceTick; //지속시간

    private float tickTime;  //표식 생성 간격

    private PoisonSplash splashPrefab; //표식 프리팹

    private int splashDamage; //표식 데미지
    public PoisonDebuff(int splashDamage, float tickTime, PoisonSplash splashPrefab,float duration, Monster target) : base(target,duration)
    {    //독타워 디버프
        this.splashDamage = splashDamage;
        this.tickTime = tickTime;
        this.splashPrefab = splashPrefab;
    }
    public override void Updata() //디버프 업데이트
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;
            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                Splash();
            }
        }
        base.Updata();
        
    }
    private void Splash() //표식
    {
        
       
        PoisonSplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity); //표식 생성

        tmp.Damage = splashDamage;  //표식 데미지
        
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>()); //생성될때 충돌 무시
    }
}
