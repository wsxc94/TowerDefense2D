using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    private float tickTime; //지속데미지 간격

    private float timeSinceTick; //지속시간

    private float tickDamage; //지속데미지

    public FireDebuff(float tickDamage, float tickTime, float duration, Monster target) : base(target,duration)
    { //디버프
        this.tickDamage = tickDamage;
        this.tickTime = tickTime;
    }
    public override void Updata()
    {
        if ( target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime) //지속시간이 지속간격보다 크면
            {
                timeSinceTick = 0f;

                target.TakeDamage(tickDamage, Element.FIRE); //데미지적용
            }
        }
        

        base.Updata();
    }
}
