using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower {

    [SerializeField]
    private float tickTime; //지속데미지가 들어가는 간격

    [SerializeField]
    private float tickDamage; //지속데미지

    public float TickTime //지속데미지간격 프로퍼티
    {
        get
        {
            return tickTime;
        }

    }

    public float TickDamage //지속데미지 프로퍼티
    {
        get
        {
            return tickDamage;
        }

    }

    private void Start()
    {
        ElementType = Element.FIRE;  //이 타워 타입 설정
        Upgrades = new TowerUpgrade[] //업그레이드
        {
            //가격, 데미지, 지속시간, 확률, 간격, 지속데미지
            new TowerUpgrade(2,2,0.5f,1,-0.1f,1),
            new TowerUpgrade(3,4,0.5f,1,-0.1f,2),
            new TowerUpgrade(4,6,0.5f,1,-0.1f,3),
            new TowerUpgrade(5,8,0.5f,1,-0.1f,4),
            new TowerUpgrade(6,10,0.5f,1,-0.1f,5),
            
        };
    }
    public override Debuff GetDebuff() //디버프 설정
    {
        return new FireDebuff(TickDamage,TickTime,DebuffDuration,Target);
    }
    public override string GetStats() //타워 스텟 설정
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1}\n데미지 간격: {2}초<color=#00ff00ff>-{4}</color>\n도트데미지: {3} <color=#00ff00ff>+{5}</color>", "<size=15>불타워</size> ", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }
        return string.Format("<color=#ffa500ff>{0}</color>{1}\n데미지 간격: {2}초\n지속데미지: {3}", "<size=15>불타워</size> ", base.GetStats(), TickTime, TickDamage);
    }
    public override void Upgrade() //업그레이드 적용
    {
        this.tickTime -= NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
