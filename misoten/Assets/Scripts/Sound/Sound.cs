using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound:MonoBehaviour
{
    /// SEチャンネル数
    readonly int SE_CHANNEL = 23;

    /// サウンド種別
    enum eType
    {
        Bgm, // BGM
        Se,  // SE
    }

    // シングルトン
    static Sound _singleton = null;
    // インスタンス取得
    public static Sound GetInstance()
    {
        return _singleton ?? (_singleton = new Sound());
    }

    // サウンド再生のためのゲームオブジェクト
    GameObject _object = null;
    // サウンドリソース
    AudioSource _sourceBgm = null; // BGM
    [SerializeField]
    AudioSource _sourceSeDefault = null; // SE (デフォルト)
    [SerializeField]
    AudioSource[] _sourceSeArray; // SE (チャンネル)

   // List<AudioSource> seArray;
    //List<_Data2> data2Array;
                                  // BGMにアクセスするためのテーブル
    Dictionary<string, _Data> _poolBgm = new Dictionary<string, _Data>();
    // SEにアクセスするためのテーブル 
    Dictionary<string, _Data> _poolSe = new Dictionary<string, _Data>();

    // BGMのフェードフラグ(多重化防止)
    bool isFadingBgm = false;

    /// 保持するデータ
    class _Data
    {
        /// アクセス用のキー
        public string Key;
        /// リソース名
        public string ResName;
        /// AudioClip
        public AudioClip Clip;

        /// コンストラクタ
        public _Data(string key, string res)
        {
            Key = key;
            ResName = "Sounds/" + res;
            // AudioClipの取得
            Clip = Resources.Load(ResName) as AudioClip;
        }
    }

    class _Data2
    {
        public string key;
        public int element;

        public _Data2(string _key, int _element)
        {
            key = _key;
            element = _element;
        }
    }

    /// コンストラクタ
    public Sound()
    {
        // チャンネル確保
        _sourceSeArray = new AudioSource[SE_CHANNEL];
     //   seArray = new List<AudioSource>();
    //    data2Array = new List<_Data2>();
    }

    public AudioSource GetAudioSource(int channel)
    {
        return _sourceSeArray[channel];
    }

    /// AudioSourceを取得する
    AudioSource _GetAudioSource(eType type, int channel = -1)
    {
        if (_object == null)
        {
            // GameObjectがなければ作る
            _object = new GameObject("Sound");
            // 破棄しないようにする
            GameObject.DontDestroyOnLoad(_object);
            // AudioSourceを作成
            _sourceBgm = _object.AddComponent<AudioSource>();
            _sourceSeDefault = _object.AddComponent<AudioSource>();
       //     for (int i = 0; i < SE_CHANNEL; i++)
           // {
          //      seArray.Add(new AudioSource());
          //      seArray[i]= _object.AddComponent<AudioSource>();
       //     }
            for (int i = 0; i < SE_CHANNEL; i++)
            {
                _sourceSeArray[i] = _object.AddComponent<AudioSource>();
            }
        }

        if (type == eType.Bgm)
        {
            // BGM
            return _sourceBgm;
        }
        else
        {
            // SEのAudioSoruce数ループ
        //    for (int i = 0; i < seArray.Count; i++)
         //   {
                // 再生中でないAudioSoruceがあったらそのAudioSoruceを返す
          //      if (!seArray[i].isPlaying)
          //      {

          //          return seArray[i];
          //      }
        //    }

            // seArrayのAudioSoruceが全て再生中だったならば
            // AudioSourceを追加
          //  seArray.Add(new AudioSource());
           // seArray[seArray.Count - 1] = _object.AddComponent<AudioSource>();
          //  return seArray[seArray.Count - 1];

            // SE
            if (0 <= channel && channel < SE_CHANNEL)
            {
                // チャンネル指定
                return _sourceSeArray[channel];
            }
            else
            {

                // デフォルト
                return _sourceSeDefault;
            }

        }
    }

    // サウンドのロード
    // ※Resources/Soundsフォルダに配置すること
    public static void LoadBgm(string key, string resName)
    {
        GetInstance()._LoadBgm(key, resName);
    }
    public static void LoadSe(string key, string resName)
    {
        GetInstance()._LoadSe(key, resName);
    }
    void _LoadBgm(string key, string resName)
    {
        if (_poolBgm.ContainsKey(key))
        {
            // すでに登録済みなのでいったん消す
            _poolBgm.Remove(key);
        }
        _poolBgm.Add(key, new _Data(key, resName));
    }
    void _LoadSe(string key, string resName)
    {
        if (_poolSe.ContainsKey(key))
        {
            // すでに登録済みなのでいったん消す
            _poolSe.Remove(key);
        }
        _poolSe.Add(key, new _Data(key, resName));
    }

    /// BGMの再生
    /// ※事前にLoadBgmでロードしておくこと
    public static bool PlayBgm(string key)
    {
        return GetInstance()._PlayBgm(key);
    }
    bool _PlayBgm(string key)
    {
        if (_poolBgm.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // いったん止める
        _StopBgm();

        // リソースの取得
        var _data = _poolBgm[key];

        // 再生
        var source = _GetAudioSource(eType.Bgm);
        source.loop = true;
        source.clip = _data.Clip;
        source.Play();

        return true;
    }
    /// BGMの停止
    public static bool StopBgm()
    {
        return GetInstance()._StopBgm();
    }
    bool _StopBgm()
    {
        _GetAudioSource(eType.Bgm).Stop();

        return true;
    }

    /// SEの再生
    /// ※事前にLoadSeでロードしておくこと
    public static bool PlaySe(string key, int channel = -1)
    {
        return GetInstance()._PlaySe(key, channel);
    }
    bool _PlaySe(string key, int channel = -1)
    {
        if (_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.clip = _data.Clip;
            source.Play();
        }
        else
        {
            // デフォルトで再生
            var source = _GetAudioSource(eType.Se);
            source.PlayOneShot(_data.Clip);
        }

        return true;
    }

    /// SEの停止
    public static bool StopSe(string key, int channel)
    {
        return GetInstance()._StopSe(key, channel);
    }
    bool _StopSe(string key, int channel)
    {
        if (_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.Stop();
        }
        else
        {
            return false;
        }

        return true;

    }

    // BGMのボリュームの変更
    public static bool SetVolumeBgm(string key, float vol)
    {
        return GetInstance()._SetVolumeBgm(key, vol);
    }
    bool _SetVolumeBgm(string key, float vol)
    {
        if (_poolBgm.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolBgm[key];

        // ボリューム変更
        var source = _GetAudioSource(eType.Bgm);
        source.volume = vol;

        return true;
    }

    // SEのボリューム変更
    public static bool SetVolumeSe(string key, float vol, int channel)
    {
        return GetInstance()._SetVolumeSe(key, vol, channel);
    }
    bool _SetVolumeSe(string key, float vol, int channel)
    {
        if (_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.volume = vol;
        }
        else
        {
            return false;
        }

        return true;
    }

    // BGMのフェード
    public static IEnumerator FadeBgm(string key, float startVol, float endVol, float fadeTime)
    {
        return GetInstance()._FadeBgm(key, startVol, endVol, fadeTime);
    }
    IEnumerator _FadeBgm(string key, float startVol, float endVol, float fadeTime)
    {
        if (isFadingBgm)
        {
            yield break;
        }

        isFadingBgm = true;

        if (_poolBgm.ContainsKey(key) == false)
        {
            // 対応するキーがない
            isFadingBgm = false;
            yield break;
        }

        // リソースの取得
        var _data = _poolBgm[key];

        // 一時停止
        var source = _GetAudioSource(eType.Bgm);

        // フェードのインかアウトかを決める
        bool fadeInFlg = false, fadeOutFlg = false;
        if (startVol == endVol)
        {
            isFadingBgm = false;
            yield break;
        }
        else if (startVol > endVol)
        {
            fadeOutFlg = true;
        }
        else
        {
            fadeInFlg = true;
        }

        // 再生開始時のボリュームセット
        source.volume = startVol;

        // 再生されていない場合再生
        if (!source.isPlaying)
        {
            source.Play();
        }

        // フェードの処理
        float nowTime = 0.0f;           // 割合計算用 時間
        float nowVolume = startVol;     // フェードの割合で計算した大きさ

        // フェードイン処理
        if (fadeInFlg)
        {
            nowTime = 0.0f;
            while (nowTime <= fadeTime)
            {
                nowTime += Time.deltaTime;
                nowVolume = nowTime / fadeTime * endVol;
                source.volume = nowVolume;
                yield return true;
            }
            source.volume = endVol;
        }
        // フェードアウト処理
        else if (fadeOutFlg)
        {
            nowTime = fadeTime;
            while (nowTime >= 0)
            {
                nowTime -= Time.deltaTime;
                nowVolume = nowTime / fadeTime * startVol;
                source.volume = nowVolume;
                yield return true;
            }
            source.volume = endVol;

            // 再生する意味がないので停止
            if (source.volume == 0.0f)
            {
                source.Stop();
            }
        }

        isFadingBgm = false;

    }

    // BGMの一時停止
    public static bool PauseBgm(string key)
    {
        return GetInstance()._PauseBgm(key);
    }
    bool _PauseBgm(string key)
    {
        if (_poolBgm.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolBgm[key];

        // 一時停止
        var source = _GetAudioSource(eType.Bgm);
        source.Pause();

        return true;
    }

    // BGMの一時停止解除
    public static bool UnPauseBgm(string key)
    {
        return GetInstance()._UnPauseBgm(key);
    }
    bool _UnPauseBgm(string key)
    {
        if (_poolBgm.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolBgm[key];

        // 一時停止解除
        var source = _GetAudioSource(eType.Bgm);
        source.UnPause();

        return true;
    }

    // SEの一時停止
    public static bool PauseSe(string key, int channel)
    {
        return GetInstance()._PauseSe(key, channel);
    }
    bool _PauseSe(string key, int channel)
    {
        if (_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.Pause();
        }
        else
        {
            return false;
        }

        return true;
    }

    // SEの一時停止解除
    public static bool UnPauseSe(string key, int channel)
    {
        return GetInstance()._UnPauseSe(key, channel);
    }
    bool _UnPauseSe(string key, int channel)
    {
        if (_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.UnPause();
        }
        else
        {
            return false;
        }

        return true;
    }

    // SEのループ設定
    public static bool SetLoopFlgSe(string key, bool flg, int channel)
    {
        return GetInstance()._SetLoopFlgSe(key, flg, channel);
    }
    bool _SetLoopFlgSe(string key, bool flg, int channel)
    {
        if (_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.loop = flg;
        }
        else
        {
            return false;
        }

        return true;
    }

    public int GetChannelNum() => SE_CHANNEL;

}

