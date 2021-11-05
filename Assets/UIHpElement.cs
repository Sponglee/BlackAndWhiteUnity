using UnityEngine;
using UnityEngine.UI;

public class UIHpElement : MonoBehaviour
{
    public HPSystem referenceHp;

    public Slider hpSlider;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public void InitializeUIHP(HPSystem target)
    {
        referenceHp = target;

        target.OnHpChange.AddListener(HpChangeHandler);
    }

    private void FixedUpdate()
    {
        transform.position = cam.WorldToScreenPoint(referenceHp.transform.position);
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
