using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{

    /// <summary>
    /// 이미 종료처리 했는지 확인하는 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 싱글톤 객체
    /// </summary>
    private static T instance;

    /// <summary>
    /// 싱글톤의 객체를 읽기 위한 프로퍼티
    /// </summary>
    public static T Instance
    {
        get
        {
            // 종료 처리 했을 때
            if (isShutDown)
            {
                // 경고 출력
                Debug.LogWarning($"{typeof(T).Name} 싱글톤은 이미 삭제 중이다.");
                // null 반환
                return null;
            }
            // instance가 없을 때
            if (instance == null)
            {
                GameObject obj = new GameObject();  // 오브젝트만 만들어서

                obj.name = $"{typeof(T).Name} Singleton";   // 이틈에 추가하고

                instance = obj.AddComponent<T>();   // 싱글톤객체에 추가하여 생성

                DontDestroyOnLoad(instance.gameObject); // 씬이 사라질 때 게임오브젝트가 삭제되지 않게 하는 함수
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 프로그램이 종료될 때 실행되는 함수
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;  // 종료 표시
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init(scene, mode);
    }

    protected virtual void Init(Scene scene, LoadSceneMode mode)
    {

    }
}
