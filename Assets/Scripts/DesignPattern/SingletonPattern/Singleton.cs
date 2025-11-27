using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // 외부 호출용 프로퍼티, 해당 타입의 싱글톤이 없으면 찾아보고 그래도 없으면 새로 생성 후 설정
    public static T Instance
    {
        get
        {
            if( _instance == null )
            {
                // 가장 먼저 로드된 오브젝트 찾기
                _instance = FindFirstObjectByType<T>();

                if(_instance == null)
                {
                    GameObject singletonObj = new GameObject();
                    _instance = singletonObj.AddComponent<T>();
                    singletonObj.name = typeof(T).ToString();
                }
            }

            return _instance;
        }
    }

    // 중복 체크 및 연결 기능 구현
    protected virtual void Awake()
    {
        if (_instance == null )
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if ( _instance != this )
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
