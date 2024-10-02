using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{

    /// <summary>
    /// �̹� ����ó�� �ߴ��� Ȯ���ϴ� ����
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// �̱��� ��ü
    /// </summary>
    private static T instance;

    /// <summary>
    /// �̱����� ��ü�� �б� ���� ������Ƽ
    /// </summary>
    public static T Instance
    {
        get
        {
            // ���� ó�� ���� ��
            if (isShutDown)
            {
                // ��� ���
                Debug.LogWarning($"{typeof(T).Name} �̱����� �̹� ���� ���̴�.");
                // null ��ȯ
                return null;
            }
            // instance�� ���� ��
            if (instance == null)
            {
                GameObject obj = new GameObject();  // ������Ʈ�� ����

                obj.name = $"{typeof(T).Name} Singleton";   // ��ƴ�� �߰��ϰ�

                instance = obj.AddComponent<T>();   // �̱��水ü�� �߰��Ͽ� ����

                DontDestroyOnLoad(instance.gameObject); // ���� ����� �� ���ӿ�����Ʈ�� �������� �ʰ� �ϴ� �Լ�
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
    /// ���α׷��� ����� �� ����Ǵ� �Լ�
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true;  // ���� ǥ��
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init(scene, mode);
    }

    protected virtual void Init(Scene scene, LoadSceneMode mode)
    {

    }
}
