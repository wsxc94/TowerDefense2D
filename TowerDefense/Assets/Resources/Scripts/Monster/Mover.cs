using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

    private Monster target; //몬스터
    
    public Waypoint currentWaypoint; //현재 웨이포인트

    public int currentIndex = 0; //현재 인덱스

    private Animator myAnimator;

    private void Awake()
    {
        target = GetComponent<Monster>(); //몬스터
        myAnimator = GetComponent<Animator>(); //애니메이터

    }

    void Start()
    {

        if (LevelManager.Instance.waypoints.Length > 0) //초기화
        {
            currentWaypoint = LevelManager.Instance.waypoints[0];
        }
    }

    void Update()
    {
        {
            //프레임마다 비교
            if (currentWaypoint != null && currentIndex != LevelManager.Instance.waypoints.Length)
            {
                MoveTowardsWaypoint();
            }
            
        }

    }

        private void MoveTowardsWaypoint()
        {
        //몬스터가 가는 방향 별로 몬스터 이동애니메이션 변경
        if (currentIndex == 1 || currentIndex == 3 || currentIndex == 5 || currentIndex == 7) 
        { // 아래
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        else if (currentIndex == 2||currentIndex==6)
        { // 왼쪽
            myAnimator.SetInteger("Horizontal", -1);
            myAnimator.SetInteger("Vertical", 0);
        }
        else if (currentIndex == 0 || currentIndex == 4 || currentIndex == 8)
        { // 오른쪽
            myAnimator.SetInteger("Horizontal", 1);
            myAnimator.SetInteger("Vertical", 0);
        }

            // 현재 움직이는 물체의 좌표
            Vector3 currentPosition = this.transform.position;

            // 웨이포인트 좌표 
            Vector3 targetPosition = currentWaypoint.transform.position;

            // 이동중인 물체가 경유지와 가깝지 않은 경우
            if (Vector3.Distance(currentPosition, targetPosition) > .1f)
            {

                // 방향 파악 및 표준화
                Vector3 directionOfTravel = targetPosition - currentPosition;
                directionOfTravel.Normalize();

                //벡터로 각축 이동
                this.transform.Translate(
                    directionOfTravel.x * target.Speed * Time.deltaTime,
                    directionOfTravel.y * target.Speed * Time.deltaTime,
                    directionOfTravel.z * target.Speed * Time.deltaTime,
                    Space.World
                );
            
        }
        else
        {

        NextWaypoint();

        }
    }

    private void NextWaypoint()
        {
        if (currentIndex +1 >= LevelManager.Instance.waypoints.Length) //현재 웨이포인트 인덱스와 웨이포인트 최대 길이 비교
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
            
            
            currentWaypoint = LevelManager.Instance.waypoints[currentIndex];
        
        }
    
}