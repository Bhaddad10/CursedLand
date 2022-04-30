using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantNpcController : NpcController
{
    public void actionAfterDialog()
    {
        GameManager.Instance.LoadScene("feature_Shop");
    }
}
