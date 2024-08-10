using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
public class PlayerInfoManager : MonoBehaviour
{
    [SerializeField] private Scoreboard scoreboard;

    private void Start()
    {
        StartCoroutine(SetupRoutine());
    }
    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player logged in successfully");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                scoreboard.GetPlayerName();
                scoreboard.playerNames.text = "Player: " + response.player_id.ToString();
                done = true;
            }
            else
            {
                Debug.LogError("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return scoreboard.FetchTopHighscoresRoutine();
    }
}
