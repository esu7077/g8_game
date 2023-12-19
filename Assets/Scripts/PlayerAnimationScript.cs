using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    float time;     //時間
    float timeTmp;  //時間の一時的な記憶
    PlayerMoveScript playerMoveScript;  //動きのスクリプト
    int moveNum;    //動作番号
    int pmoveNum;   //前の動作番号
    /*
     * 0:無
     * 1:歩行
     * 2:ジャンプ最大
     * 3:自由落下
     */
    int lr;         //左右
    int plr;        //前の左右
    /*
     * 0 < lr : 右
     * lr < 0 : 左
     */

    public GameObject bodyObj; //身体obj
    public GameObject legRObj; //右足obj
    public GameObject legLObj; //左足obj
    public GameObject eyeObj;  //目obj
    public GameObject finObj;  //鰭obj

    SpriteRenderer bodySprRend; //身体spriteRenderer
    SpriteRenderer legRSprRend; //右足spriteRenderer
    SpriteRenderer legLSprRend; //左足spriteRenderer
    SpriteRenderer eyeSprRend;  //目spriteRenderer
    SpriteRenderer finSprRend;  //鰭spriteRenderer

    public Sprite[] bodySprite; //身体sprite配列
    /*
     * 0:通常
     */
    public Sprite[] legSprite;  //足sprite配列
    /*
     * 0:歩行1
     * 1:歩行2
     * 2:歩行3, 通常右
     * 3:歩行4, 自由落下右
     * 4:歩行5, 通常左
     * 5:歩行6, ジャンプ左, 自由落下左
     * 6:ジャンプ右
     */
    public Sprite[] eyeSprite;  //目sprite配列
    /*
     * 0:通常
     * 1:ダメージ
     * 2:やる気
     */
    public Sprite[] finSprite;  //鰭Sprite配列
    /*
     * 0:左上, 通常
     * 1:右上
     * 2:右下
     * 3:左下
     */

    public float moveInterval;
    

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        timeTmp = 0;
        moveNum = 0;
        pmoveNum = 0;
        lr = 1;
        plr = 1;
        playerMoveScript = this.GetComponent<PlayerMoveScript>();   //動作スクリプトの取得

        bodySprRend = bodyObj.GetComponent<SpriteRenderer>();
        legRSprRend = legRObj.GetComponent<SpriteRenderer>();
        legLSprRend = legLObj.GetComponent<SpriteRenderer>();
        eyeSprRend = eyeObj.GetComponent<SpriteRenderer>();
        finSprRend= finObj.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        moveNum = playerMoveScript.moveNum;
        lr = playerMoveScript.lr;

        //動作番号0:無
        if (moveNum == 0)
        {
            //前フレームが0でなければ変更
            if (pmoveNum != 0)
            {
                bodySprRend.sprite = bodySprite[0];
                legRSprRend.sprite = legSprite[2];
                legLSprRend.sprite = legSprite[4];
                eyeSprRend.sprite = eyeSprite[0];
                finSprRend.sprite= finSprite[0];
            }
        }
        //動作番号1:歩行
        if (moveNum == 1)
        {
            //前フレームが違うなら開始時間の保存
            if (pmoveNum != 1) { timeTmp = time; }
            //要素番目の計算
            int idxR = (int)(((time - timeTmp) / moveInterval) % 6);
            int idxL = (int)(((time - timeTmp) / moveInterval + 3) % 6);
            legRSprRend.sprite = legSprite[idxR];
            legLSprRend.sprite = legSprite[idxL];
        }
        //動作番号2:ジャンプ
        if (moveNum == 2)
        {
            legRSprRend.sprite = legSprite[6];
            legLSprRend.sprite = legSprite[5];
        }
        //動作番号3:自由落下
        if (moveNum == 3)
        {
            legRSprRend.sprite = legSprite[3];
            legLSprRend.sprite = legSprite[5];
        }

        


        //右向き
        if (0 < lr)
        {
            //前が左向きなら
            if (plr <= 0)
            {
                Vector3 r = new Vector3(1f, 1f, 1f);
                bodyObj.transform.localScale = r;
                legRObj.transform.localScale = r;
                legLObj.transform.localScale = r;
                eyeObj.transform.localScale = r;
                finObj.transform.localScale = r;
            }
        }
        //左向き
        if (0 > lr)
        {
            //前が右向きなら
            if (plr >= 0)
            {
                Vector3 r = new Vector3(-1f, 1f, 1f);
                bodyObj.transform.localScale = r;
                legRObj.transform.localScale = r;
                legLObj.transform.localScale = r;
                eyeObj.transform.localScale = r;
                finObj.transform.localScale = r;
            }
        }

        //前の更新
        pmoveNum = moveNum;
        plr = lr;
    }
}
