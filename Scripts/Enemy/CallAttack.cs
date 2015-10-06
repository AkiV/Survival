using UnityEngine;
using System.Collections;

public class CallAttack : MonoBehaviour 
{
    void OnAttack()
    {
        transform.parent.GetComponent<Enemy>().SendMessage("OnAttack");
    }
}
