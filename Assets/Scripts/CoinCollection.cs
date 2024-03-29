using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int coin = 0;
   private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "coin")
        {   
            coin +=1;
            Debug.Log(coin);
            SaveManager.instance.money += 100;
            SaveManager.instance.Save();
            Destroy(other.gameObject);
        }
    }
}
