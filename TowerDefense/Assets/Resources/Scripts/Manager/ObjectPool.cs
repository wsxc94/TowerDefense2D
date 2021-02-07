using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*오브젝트풀이란 객체를 필요로 할때 요청을 하고 반환하는 일련의 작업을 수행하는 것이며
여러 객체를 만들때 매번 새로 생성하는것이 아닌 미리 생성된 풀에서 객체를 재사용 할수 있게되며
그에 따른 이득을 볼수있다. 사라진 객체를 메모리에 적재하여 필요할때 다시 불러옴*/
public class ObjectPool : MonoBehaviour {
    [SerializeField]
    private GameObject[] objectPrefabs=null; //오브젝트풀에서 사용할 게임오브젝트

    private List<GameObject> pooledObjects = new List<GameObject>(); //오브젝트풀 리스트 생성

    public GameObject GetObject(string type) //오브젝트의 타입
    {
        foreach (GameObject go in pooledObjects) //리스트 순회
        {
            if (go.name == type && !go.activeInHierarchy) //같은 이름 같은 타입이면 적재된 객체를 불러옴
            {
                go.SetActive(true);
                return go;
            }
        }
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type) //새로운 오브젝트를 생성
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }
    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false); //게임오브젝트를 비활성화
    }
}
