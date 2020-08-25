using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower     //독타워 
{
    [SerializeField]
    private float tickTime; //표식 생성 간격

    [SerializeField]
    private PoisonSplash splashPrefab=null; //표식 프리팹

    [SerializeField]
    private int splashDamage; //표식데미지

    public int SplashDamage //표식데미지 프로퍼티
    {
        get
        {
            return splashDamage;
        }

    }

    public float TickTime
    {                      //표식 생성 간격
        get
        {
            return tickTime;
        }

    }

    private void Start()
    {
        ElementType = Element.POISON; //속성 타입 
        Upgrades = new TowerUpgrade[] //업그레이드
        {
            //가격, 데미지, 지속시간, 확률, 나오는간격, 표식데미지
            new TowerUpgrade(2,1,1,1,0,3),
            new TowerUpgrade(4,3,1,1,0,6),
            new TowerUpgrade(6,5,1,1,0,9),
            new TowerUpgrade(8,5,1,1,0,12),
            new TowerUpgrade(10,5,1,1,0,15)
        };
    }
    public override Debuff GetDebuff() //디버프
    {
        return new PoisonDebuff(SplashDamage,tickTime,splashPrefab,DebuffDuration,Target);
    }
    public override string GetStats() //타워 스텟
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ff00ff>{0}</color>{1}\n표식데미지: {2}<color=#00ff00ff>+{4}</color>", "<size=15>독타워</size>", base.GetStats(), SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }
        return string.Format("<color=00ff00ff>{0}</color>{1}\n표식데미지: {2}", "<size=15>독타워</size>", base.GetStats(), Damage ,SplashDamage);
    }
    public override void Upgrade() //업그레이드
    {
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime -= NextUpgrade.TickTime;
        base.Upgrade();
    }
}
