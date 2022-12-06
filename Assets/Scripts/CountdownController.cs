using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public int countdownTime;
    public TMPro.TextMeshProUGUI countdownDisplay;

    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);

            countdownTime--;
        }

        
        countdownDisplay.text = "Round " + GameManager.round.ToString();
        yield return new WaitForSecondsRealtime(1.5f);
        countdownDisplay.text = "Start!";
        yield return new WaitForSecondsRealtime(1f);
		
		GameManager.Instance.BeginGame();

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
    }
}
