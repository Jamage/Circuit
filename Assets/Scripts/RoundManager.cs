using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : GenericSingletonClass<RoundManager>
{
    public ParticleSystem roundOverParticles;
    public TextMeshProUGUI roundOverText;

    private void OnEnable()
    {
        
    }

    public void RoundOver()
    {
        roundOverParticles.gameObject.SetActive(true);
        roundOverText.gameObject.SetActive(true);
    }


}
