using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegColliderScript : MonoBehaviour
{
    PlayerMoveScript playerMoveScript;  //動きのスクリプト

    // Start is called before the first frame update
    void Start()
    {
        playerMoveScript = this.transform.parent.GetComponent<PlayerMoveScript>();   //動作スクリプトの取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //地面から離れたら
        if (collision.transform.tag == "Ground") { playerMoveScript.setIsGround(false); }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //地面についているなら
        if (collision.transform.tag == "Ground") { playerMoveScript.setIsGround(true); }
    }
}
