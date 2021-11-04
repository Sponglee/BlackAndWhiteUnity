using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HPSystem : MonoBehaviour
{
    public int Hp = 10;

    public GameObject hpUIPref;

    public bool IsDead = false;

    public UnityEvent<float, float> OnHpChange;


    private int MaxHp;

    private void Start()
    {
        MaxHp = Hp;
        UIHpElement tmpUI = Instantiate(hpUIPref, GameObject.FindGameObjectWithTag("Canvases").transform).GetComponent<UIHpElement>();
        tmpUI.InitializeUIHP(this);
    }

    public void DecreaseHp(int amount)
    {
        if (IsDead)
            return;

        Hp -= amount;

        OnHpChange.Invoke(Hp, MaxHp);

        if (Hp <= 0)
        {
            Hp = 0;
            IsDead = true;
            OnHpChange.Invoke(0, MaxHp);
        }


    }
}
