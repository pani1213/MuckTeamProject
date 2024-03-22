
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{
    public static EffectFactory Instance { get; private set; }

    // (생성할) 아이템 프리팹들
    public List<GameObject> EffectPrefabs;

    // 공장의 창고
    private List<EffectFactory> _effectPool;
    public int PoolCount = 10;

    public ItemType ItemType;
    

    public void Awake()
    {

        // 싱글톤 인스턴스를 설정
        Instance = this;

        // 아이템 풀을 초기화
        _effectPool = new List<EffectFactory>();


        // PoolCount 만큼 아이템을 생성하여 풀에 저장
        for (int i = 0; i < PoolCount; ++i)             // 10번
        {
            foreach (GameObject prefab in EffectPrefabs)  // 3개
            {
                // 1. 만들고
                GameObject item = Instantiate(prefab);
                // 2. 창고에 넣는다.
                item.transform.SetParent(this.transform);
                _effectPool.Add(item.GetComponent<EffectFactory>());
                // 3. 비활성화
                item.SetActive(false);
            }

        }
    }

    // Get 메소드는 주어진 아이템 타입에 해당하는 아이템 오브젝트를 아이템 풀에서 찾아 반환
    private EffectFactory Get(ItemType itemType) // 창고 뒤지기 
    {
        foreach (EffectFactory itemObject in _effectPool) // 창고를 뒤진다.
        {
            if (itemObject.gameObject.activeSelf == false && itemObject.ItemType == itemType)
            // activeSelf: GameObject가 현재 활성화되어 있는지 아닌지 bool값 
            {
                return itemObject;
            }
        }

        return null;
    }

    // MakePercent 메소드: 확률 생성 (공장아! 랜덤박스 주문할게!)
    public void MakePercent(Vector3 position)
    {
        int percentage = UnityEngine.Random.Range(0, 100);
        if (percentage <= 20)
        {
            //Make(ItemType.Health, position);
        }
        else if (percentage <= 40)
        {
            //Make(ItemType.Stamina, position);
        }
        else if (percentage <= 50)
        {
            //Make(ItemType.Bullet, position);
        }
    }

    // Make 메소드: 기본 생성 (공장아! 내가 원하는거 주문할게!)
    public void Make(ItemType itemType, Vector3 position)
    {
        // 아이템 풀에서 아이템 오브젝트를 가져옴
        //ItemObject itemObject = Get(itemType);

        /*if (itemObject != null)
        {
            // 아이템의 위치를 설정하고 아이템을 활성화
            itemObject.transform.position = position;
            itemObject.Init();
            itemObject.gameObject.SetActive(true);
        }*/
    }

    // 어떤 효과를 나오게 할 것인가 이후에 쓸 수 있는 것들
    //private IEnumerator Effect_Coroutine()
    //{
    /* while (_progress < 0.8f)
     {
         // 진행도를 누적하는 시간을 계산하고, 현재 위치를 갱신합니다.
         _progress += Time.deltaTime / TRACE_DURATION;
         transform.position = Vector3.Slerp(_startPosition, _player.position, _progress);

         // 다음 프레임까지 대기한다.
         yield return null;
     }

     // 진행도가 0.8 이상이면 아이템을 추가하고 오브젝트를 비활성화합니다.
     ItemManager.Instance.AddItem(ItemType);
     gameObject.SetActive(false);

 }*/
}
