using UnityEngine;
using System.Collections;

public class CameraRotator : MonoBehaviour
{

    public Transform target;
    public float spinSpeed = 1f;
    float distance = 3f;
    public float LateMove = 1f;
    public float diffR = 0.01f;

    Vector3 nowPos;
    Vector3 pos = Vector3.zero;
    public Vector2 mouse = Vector2.zero;

    Transform currentTarget;
    Vector3 currentTargetCenter;

    void Start()
    {
        // 初期位置の取得
        nowPos = transform.position;
    }

    void Update()
    {
        // マウスの移動の取得
        if (Input.GetMouseButton(0))
        {
            mouse += new Vector2(Input.mousePosition.x, Input.mousePosition.y) * Time.deltaTime * spinSpeed;
        }

        mouse.y = Mathf.Clamp(mouse.y, 0f, 1f);

        // 変更...ズーム設定部分の位置を「球面座標系変換」よりも手前に移動（この変更はあまり重要ではありません）
        // 実際の動作にほとんど違いはないでしょうが、「ズームの設定」でdistanceを変更し「球面座標系変換」で新しいdistanceを使用する...という流れの方がロジック的に自然かな?と思ったためです
        // また、QWキーを「else if」で排他的にするよりも、同時押しを許容する方が個人的に好みだったので、勝手ながら変えてしまいました...
        //ズームの設定
        if (Input.GetKey(KeyCode.Q))
        {
            distance += diffR * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            distance -= diffR * Time.deltaTime;
        }

        // 追加...お節介かもしれませんが、distanceを適当な範囲にクランプしてやるといいかもしれません
        // 特に、distanceがマイナスになるとカメラがオブジェクトを貫通してしまうことになるので、少なくともdistanceの最小値は設けてやるのがいいかと思います
        distance = Mathf.Max(distance, 1.0f); // distanceが1以下にならないようにする（最小値のみ設定する）場合
        // distance = Mathf.Clamp(distance, 1.0f, 10.0f); // distanceを1〜10におさめる（最小値と最大値を設定する）場合

        // 球面座標系変換
        pos.x = distance * Mathf.Sin(mouse.y * Mathf.PI) * Mathf.Cos(mouse.x * Mathf.PI);
        pos.y = -distance * Mathf.Cos(mouse.y * Mathf.PI);
        pos.z = -distance * Mathf.Sin(mouse.y * Mathf.PI) * Mathf.Sin(mouse.x * Mathf.PI);

        pos *= nowPos.z;
        pos.y += nowPos.y;

        UpdateCurrentTarget();

        // 変更...カメラ平行移動を「座標の更新」よりも手前に移動し、操作対象をlocalPositionではなくcurrentTargetCenterにしました
        // さらに、マウス操作やズームは移動量にTime.deltaTimeを掛けてフレームレート非依存にしているので、こちらもそれらに合わせてTime.deltaTimeを掛けるようにしています
        // また、上下左右キーも同時押しを許容するように変えました
        //カメラの平行移動
        Vector2 cameraTranslation = Vector2.zero; // カメラの移動量
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraTranslation.y += LateMove * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraTranslation.y -= LateMove * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cameraTranslation.x -= LateMove * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cameraTranslation.x += LateMove * Time.deltaTime;
        }
        // transform.rightでカメラ右方向、transform.upでカメラ上方向が得られるので、currentTargetCenterをその方向に移動
        currentTargetCenter += transform.right * cameraTranslation.x + transform.up * cameraTranslation.y;

        // さらにもうひとつお節介を加え、スペースキー押下で強制的に中心点再計算を行い、currentTargetCenterをターゲット中心に戻すようにしました
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RecalculateCurrentTargetCenter();
        }

        // 座標の更新
        transform.position = pos + currentTargetCenter;
        transform.LookAt(currentTargetCenter);
    }


    // ターゲットが変わった場合、中心点を再計算するメソッド
    void UpdateCurrentTarget() // 変更...メソッドの機能の観点から、メソッド名を変更
    {
        if (target != currentTarget)
        {
            currentTarget = target;

            RecalculateCurrentTargetCenter(); // 変更...中心点再計算部分を別メソッドに分離
        }
    }

    // 追加...中心点再計算部分を別メソッドに分離
    void RecalculateCurrentTargetCenter()
    {
        if (currentTarget == null)
        {
            currentTargetCenter = Vector3.zero; // ターゲットがnullなら(0, 0, 0)を中心とする
        }
        else
        {
            Renderer[] renderers = currentTarget.GetComponentsInChildren<Renderer>();
            int rendererCount = renderers.Length;
            if (rendererCount > 0)
            {
                Bounds unitedBounds = renderers[0].bounds;

                for (int i = 1; i < rendererCount; i++)
                {
                    unitedBounds.Encapsulate(renderers[i].bounds);
                }

                currentTargetCenter = unitedBounds.center; // 結合したバウンディングボックスの中心を新たな中心点とする
            }
            else
            {
                currentTargetCenter = currentTarget.position; // レンダラーがないオブジェクトの場合、positionを中心とする
            }
        }
    }
}