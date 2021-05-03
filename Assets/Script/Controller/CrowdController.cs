using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasketballGameDefinition;

public class CrowdController : MonoBehaviour
{
    private GameObject[] human;
    [SerializeField] private AudioSource auSource;

    private void Awake()
    {
        human = new GameObject[this.transform.childCount];
        for(int i = 0; i < human.Length; i++)
        {
            human[i] = this.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        GameManager.Basket += CrowdHappy;
    }

    private void CrowdAnimation(CrowdAnimationClips _crowdAnimationClips)
    {
        for(int i = 0; i < human.Length; i++)
        {
            if(human[i].GetComponent<Animator>()!=null)
                human[i].GetComponent<Animator>().Play(_crowdAnimationClips.ToString());
        }
    }

    private void CrowdHappy()
    {
        Debug.Log("Happy");
        AudioController.Instance.PlaySound(SoundEffect.CrowdHappy, auSource);
        CrowdAnimation(CrowdAnimationClips.Happy);
        
    }

    private void CrowdUnHappy()
    {
        CrowdAnimation(CrowdAnimationClips.Unhappy);
        AudioController.Instance.PlaySound(SoundEffect.CrowdUnhappy, auSource);
    }
}
