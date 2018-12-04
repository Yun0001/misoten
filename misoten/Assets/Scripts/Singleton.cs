using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    protected static T instance = null;

    /// <summary>
    /// シングルトンオブジェクト生成
    /// </summary>
    protected static void CreateSingletonObject(Type t)
    {
        // 新しいGameObjectを作成
        var go = new GameObject(t.ToString());

        //  T型のコンポーネントをアタッチ
        go.AddComponent<T>();

        // DontDestoryとして登録
        DontDestroyOnLoad(go);
    }


    /// <summary>
    /// インスタンス取得
    /// </summary>
    /// <returns></returns>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);
                CreateSingletonObject(t);
                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }

}
