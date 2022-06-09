using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantNpcController : NpcController
{
    public void actionAfterDialog()
    {
        GameManager.Instance.LoadScene(GameManager.SHOP_SCENE_NAME);
    }
}
