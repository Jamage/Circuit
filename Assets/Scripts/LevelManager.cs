using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : GenericSingletonClass<LevelManager>
{
    public ParticleSystem roundOverParticles;
    public TextMeshProUGUI roundOverText;
    public List<LevelData> levelList;

    public void RoundOver()
    {
        //roundOverParticles.gameObject.SetActive(true);
        //roundOverText.gameObject.SetActive(true);
    }

    public void SelectLevel(LevelData levelData)
    {
        SceneManager.LoadScene("SampleScene");
        
    }
}
