using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour { // 타워 버튼 스크립트
    [SerializeField]
    private GameObject towerPrefab = null; //타워 프리팹
    [SerializeField]
    private Sprite sprite=null; //스프라이트 이미지
    [SerializeField]
    private int price=0; // 가격
    [SerializeField]
    private Text priceText=null; //가격 텍스트

    public GameObject TowerPrefab // 타워프리팹 프로퍼티
    {
        get
        {
            return towerPrefab;
        }

    }

    public Sprite Sprite //스프라이트 이미지 프로퍼티
    {
        get
        {
            return sprite;
        }

    }

    public int Price //가격 프로퍼티
    {
        get
        {
            return price;
        }

    }

    // Use this for initialization
    private void Start () {

        priceText.text = Price + "$"; //가격표 세팅

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck); // 타워 이미지셋 인스턴스 호출

	}
	private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency) //타워를 살돈이 있으면 타워UI이미지가 하얀색
        {
            GetComponent<Image>().color = Color.white;
            priceText.color = Color.white;

        }
        else //타워를 살돈이 없으면 타워UI이미지가 어두운색이됨 
        {
            GetComponent<Image>().color = Color.grey;
            priceText.color = Color.grey;
        }
    }
    public void ShowInfo(string type) //타워 UI 타워소개 UI tooltip
    {
        string tooltip = string.Empty; //툴팁 초기화
        switch (type) //타워 타입별로 타워소개 tooltip이 달라짐
        {
            case "Fire":
                FireTower fire = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#ffa500ff><size=1>불타워</size></color>\n공격력: {0} \n공격속도 : {1} \n디버프확률 : {2}% \n디버프 지속시간 : {3}초 \n지속데미지 : {4} \n디버프효과 : 지속시간 동안 적에게 지속데미지를 입힘",fire.Damage,fire.ProjectileSpeed,fire.Proc,fire.DebuffDuration,fire.TickDamage);
                break;
            case "Ice":
                IceTower ice = towerPrefab.GetComponentInChildren<IceTower>();
            
                tooltip = string.Format("<color=#00ffffff><size=1>얼음타워</size></color>\n공격력: {0} \n공격속도 : {1} \n디버프확률 : {2}% \n디버프 지속시간 : {3}초 \n둔화율 : {4}% \n디버프효과 : 지속시간 동안 적에게 둔화효과 적용", ice.Damage,ice.ProjectileSpeed, ice.Proc, ice.DebuffDuration, ice.SlowingFactor);
                break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#00ff00ff><size=1>독타워</size></color>\n공격력: {0} \n공격속도 : {1} \n디버프확률 : {2}% \n표식개수(몬스터당) : {3}개 \n표식데미지 : {4} \n디버프효과 : 피해를 입은 몬스터가 지속시간동안 \n밟으면 데미지를 입히는 표식을 뿌림", poison.Damage, poison.ProjectileSpeed,poison.Proc, poison.DebuffDuration, poison.SplashDamage);
                break;
            case "Storm":
                StormTower storm = towerPrefab.GetComponentInChildren<StormTower>();
                tooltip = string.Format("<color=#add8e6ff><size=1>번개타워</size></color>\n공격력: {0} \n공격속도 : {1} \n디버프확률 : {2}% \n스턴 지속시간 : {3}초 \n디버프효과 : 지속시간 동안 적에게 스턴효과 적용 ", storm.Damage, storm.ProjectileSpeed, storm.Proc, storm.DebuffDuration);
                break;
            default:
                break;
        }
        GameManager.Instance.SetTooltipText(tooltip); // 문자열 타입을 받아 text 정정
        GameManager.Instance.ShowStats(); // 화면에 보이도록 active set
    }
}
