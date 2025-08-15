using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public Slider StaminaSlider, HpSlider, HungerBarSlider;
    public float Stamina, HP;
    public bool IsClimbing, IsClimbingAndMoving, InFog;
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
        if(!IsClimbing && !IsClimbingAndMoving)
        {
            GainStamina();
        }
        HpSlider.value = HP;
        HungerBarSlider.value = Hunger;
        if (InFog)
        {
            FogDamage();
        }    
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fog"))
        {
            InFog = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Fog"))
        {
            InFog = false;
        }
    }

    public void depleteStamina()
    {
        if (IsClimbing)
        {
            Stamina -= Time.deltaTime * 2 * DepletionRate;
        }
        else if (IsClimbingAndMoving)
        {
            Stamina -= Time.deltaTime * 3 * DepletionRate;

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
        HP -= Time.deltaTime * 4 * DepletionRate;
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
