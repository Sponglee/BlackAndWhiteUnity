using UnityEngine;
using UnityEngine.UI;

public class UIHpElement : MonoBehaviour
{
    public HPSystem referenceHp;

    public Slider hpSlider;

    public void InitializeUIHP(HPSystem target)
    {
        referenceHp = target;

        target.OnHpChange.AddListener(HpChangeHandler);
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(referenceHp.transform.position);
    }


    private void HpChangeHandler(float hpValue, float maxHp)
    {
        hpSlider.value = hpValue / maxHp;

        if (hpSlider.value == 0)
        {
            referenceHp.OnHpChange.RemoveListener(HpChangeHandler);
            Destroy(gameObject);
        }
    }
}
