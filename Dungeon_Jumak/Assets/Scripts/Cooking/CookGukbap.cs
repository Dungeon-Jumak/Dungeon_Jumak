using UnityEngine;
using UnityEngine.UI;

public class CookGukbap : MonoBehaviour
{
    public Fire fire;
    private float cookingTime = 5f;
    public int gukbapCount = 0;
    public Slider TimeSlider;

    private float decreaseRate = 1f;

    private void Start()
    {
        SetTime(cookingTime);
    }

    private void Update()
    {
        // 새로운 불 크기에 따른 감소 비율 적용
        float decreaseAmount = Time.deltaTime * decreaseRate;

        if (fire.fireSize > 0 && cookingTime > 0 && gukbapCount < 5)
        {
            fire.cook = true;
            Decrease(decreaseAmount);
        }
        else if (fire.fireSize <= 0 || gukbapCount >= 5)
        {
            StopCooking();
        }
    }

    public void SetTime(float amount)
    {
        cookingTime = amount;
        CheckTime();
    }

    private void CheckTime()
    {
        if (TimeSlider != null)
        {
            TimeSlider.value = cookingTime / 5f; 
        }
    }

    private void Decrease(float decrease)
    {
        if (cookingTime <= 0)
            return;

        decreaseRate = 1f;

        if (fire.fireSize >= 0.0001 && fire.fireSize <= 50)
        {
            decreaseRate = 1f / 7f; // 7초에 1씩 감소
        }

        cookingTime -= decrease;
        CheckTime();

        if (cookingTime <= 0)
        {
            gukbapCount++;
            SetTime(5f);
        }
    }

    private void StopCooking()
    {
        fire.cook = false;
    }

    public void SetDecreaseRate(float rate)
    {
        decreaseRate = rate;
    }
}
