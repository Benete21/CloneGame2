using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BranchScript : MonoBehaviour
{
    [SerializeField]
    private int BranchDuration;
    private CharacterControls PlayerScript;
    GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<CharacterControls>();
    }
    public void StartBreaking()
    {
        StartCoroutine(BreakBranch());
    }

    IEnumerator BreakBranch()
    {
        yield return new WaitForSeconds(BranchDuration - 0.1f);
        PlayerScript.LetGoOFbranch();
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
