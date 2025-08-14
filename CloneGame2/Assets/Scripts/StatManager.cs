using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public Slider StaminaSlider, HpSlider, HungerBarSlider;
    public float Stamina, HP;
    public bool IsClimbing, IsClimbingAndMoving;
    public float MaxStamina, Hp, Hunger;
    [SerializeField]
    private float HungerDepletion;
    [SerializeField]
    private float DepletionRate;

    private void Start()
    {
        StartCoroutine(HungerBar());

    }
    private void Update()
    {
        depleteStamina();
        StaminaSlider.value = Stamina;
        GainStamina();
        HpSlider.value = HP;
        HungerBarSlider.value = Hunger;
    }

    public void depleteStamina()
    {
        if (IsClimbing)
        {
            Stamina -= Time.deltaTime * 1 * DepletionRate;
        }
        else if (IsClimbingAndMoving)
        {
            Stamina -= Time.deltaTime * 2 * DepletionRate;

        }
            
    }

    public void GainStamina()
    {
        if (!IsClimbing && Stamina < MaxStamina)
        {
            Stamina += Time.deltaTime * 3;
        }
    }

    public void FogDamage()
    {

    }

    public void LeapStaminaDepletion()
    {
        Stamina -= 5;
    }

    IEnumerator HungerBar()
    {
        if (Hunger < 2.5f)
        {
            yield return new WaitForSeconds(2);
            Hunger += HungerDepletion;

            StartCoroutine(HungerBar());
        }
        else if (Hunger >= 2.5f)
        {
            yield return new WaitForSeconds(30);
            Hunger += HungerDepletion;
            DepletionRate += 0.5f;
            StartCoroutine(HungerBar());
        }
    }
}
