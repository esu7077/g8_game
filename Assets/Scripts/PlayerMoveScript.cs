using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    Vector2 pos;    //位置
    Rigidbody2D rb; //RigidBody2D
    Vector2 vel;    //速度
    float time;     //時間

    bool canMove;   //動けるか

    bool isGround;  //地面に立ってるか

    public float accel;         //加速度
    public float deccel;        //減速度
    public float maxMoveSpeed;  //最高移動速度

    float jumpStep;             //ジャンプ段階
    public float jumpPower;     //ジャンプ力
    public float maxJumpTime;   //最高ジャンプ力時間
    float jumpTime;

    bool leftMoveKey;   //左移動キー
    bool rightMoveKey;  //右移動キー
    bool jumpKey;       //ジャンプキー

    public int moveNum; //動作番号
    /*
     * 0:無
     * 1:歩行
     * 2:ジャンプ最大
     * 3:自由落下
     */
    public int lr;      //左右
    /*
     * 0 < lr : 右
     * lr < 0 : 左
     */

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        canMove = true;
        moveNum = 0;
        lr = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //位置と速度を取得
        pos = this.transform.position;
        vel = rb.velocity;
        //時間を進める
        time += Time.deltaTime;

        //キー入力
        leftMoveKey = Input.GetKey(KeyCode.LeftArrow);
        rightMoveKey = Input.GetKey(KeyCode.RightArrow);
        jumpKey = Input.GetKey(KeyCode.UpArrow);

        //動けるなら
        if (canMove)
        {
            //左右移動
            {
                //右が押されたなら
                if (rightMoveKey)
                {
                    //加速
                    vel.x += accel * Time.deltaTime;
                    //最高速度以下に
                    if (vel.x > maxMoveSpeed) { vel.x = maxMoveSpeed; }

                    //ジャンプ中でなければ動作番号の変更
                    if (jumpStep == 0) { moveNum = 1; }
                    lr = 1;     //左右変更
                }
                else
                {
                    //右移動中なら
                    if (vel.x > 0)
                    {
                        //減速
                        vel.x -= deccel * Time.deltaTime;
                        if (vel.x < 0) { vel.x = 0; }
                    }
                }
                //左が押されたなら
                if (leftMoveKey)
                {
                    //加速
                    vel.x -= accel * Time.deltaTime;
                    //最高速度以下に
                    if (vel.x < -maxMoveSpeed) { vel.x = -maxMoveSpeed; }

                    //ジャンプ中でなければ動作番号の変更
                    if (jumpStep == 0) { moveNum = 1; }
                    lr = -1;    //左右変更
                }
                else
                {
                    //左移動中なら
                    if (vel.x < 0)
                    {
                        //減速
                        vel.x += deccel * Time.deltaTime;
                        if (vel.x > 0) { vel.x = 0; }
                    }
                }
                //どちらも押されてないなら
                if (!rightMoveKey && !leftMoveKey)
                {
                    //ジャンプ中でなければ動作番号の変更
                    if (jumpStep == 0) { moveNum = 0; }
                }
            }

            //ジャンプ
            {
                //ジャンプ段階0：ジャンプできる状態
                if (jumpStep == 0)
                {
                    //地面に触れていてジャンプキーが押されたなら
                    if (jumpKey && isGround)
                    {
                        jumpStep = 1;       //段階移行
                        vel.y = jumpPower;  //ジャンプ
                        jumpTime = time;    //ジャンプ開始時間

                        //動作番号の変更
                        moveNum = 2;
                    }
                    //地面に触れていないなら
                    if (!isGround)
                    {
                        //動作番号の変更
                        moveNum = 3;
                    }
                }
                //ジャンプ段階1：長押しジャンプ中
                else if (jumpStep == 1)
                {
                    vel.y = jumpPower;  //最高ジャンプ力を維持
                    //ジャンプキーが押されたではないまたは最高ジャンプ力時間が過ぎたら
                    if (!jumpKey || time > jumpTime + maxJumpTime)
                    {
                        jumpStep = 2;       //段階移行
                    }
                }
                //ジャンプ段階2：自由落下
                else if (jumpStep == 2)
                {
                    //地面に触れたらジャンプできるように
                    if (isGround) { jumpStep = 0; }
                }
            }

            //速度適応
            rb.velocity = vel;
            
        }

    }

    public void setIsGround(bool b)
    {
        isGround = b;
    }
}
