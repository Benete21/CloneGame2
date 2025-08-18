using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public Slider StaminaSlider, HpSlider, HungerBarSlider;
    public float Stamina, HP;
    public bool IsClimbing, IsClimbingAndMoving, InFog;
    public float MaxStamina, MaxHp, Hp, Hunger;
    [SerializeField]
    private float HungerDepletion;
    [SerializeField]
    private float DepletionRate;
    public RawImage targetImage; // Assign a UI Image
    [SerializeField]
    private int StaminaGainrate;
    private float HalfStamina;
    private void Start()
    {
        StartCoroutine(HungerBar());
    }

    

    void Update()
    {
        float progress = Mathf.Clamp01(Stamina / 50f);
        targetImage.color = Color.Lerp(Color.red, new Color(0.5f, 0.8f, 1f), progress);

        depleteStamina();
        StaminaSlider.value = Stamina;
        if (!IsClimbing && !IsClimbingAndMoving)
        {
            GainStamina();
        }
        HpSlider.value = Hp;
        HungerBarSlider.value = Hunger;
        if (InFog)
        {
            FogDamage();
        }

        HalfStamina = MaxStamina / 2;

        StaminaSlider.maxValue = MaxStamina;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fog"))
        {
            InFog = true;
        }
        else if (other.CompareTag("Fire"))
        {
            Stamina = MaxStamina;
        }
        else if (other.CompareTag("Fruit"))
        {
            if(Hp <  MaxHp)
            {
                Hp += MaxHp * 0.3f;
                if (Hp > MaxHp)
                {
                    Hp = MaxHp;
                }
            }

            if(Stamina < MaxStamina)
            {
                Stamina += MaxStamina * 0.5f;
                if (Stamina > MaxStamina)
                {
                    Stamina = MaxStamina;
                }
            }

            Hunger = 0;
            
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Death_Zone"))
        {
            HitDeath_Zone();
            print("Hit");
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
        if (Stamina > 0)
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

    }

    public void GainStamina()
    {
        if (!IsClimbing && Stamina < MaxStamina)
        {
            Stamina += Time.deltaTime * StaminaGainrate;
        }
    }

    public void FogDamage()
    {
        Hp -= Time.deltaTime * 4 * DepletionRate;
    }

    public void HitDeath_Zone()
    {
        Hp -= MaxHp;
    }

    public void LeapStaminaDepletion()
    {
        Stamina -= 5;
    }

    IEnumerator HungerBar()
    {
        if (Hunger < 2.5f)
        {
            yield return new WaitForSeconds(30);
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
