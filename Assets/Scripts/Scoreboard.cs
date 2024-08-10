using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    string leaderboardID = "upsidedown";
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    public TMP_InputField playerNameInputField;
    public TextMeshProUGUI finalScore;

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        Debug.Log("Submitting score");
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Score submitted successfully");
                done = true;
                finalScore.text = scoreToUpload.ToString();
            }
            else
            {
                Debug.LogError("Could not submit score");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Score\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Got player name");
                UpdateNameText(response.name);
            }
            else
            {
                Debug.Log("Could not set player name:" + response.errorData.message);
            }
        });
    }


    public void GetPlayerName()
    {
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                UpdateNameText(response.name);
                
            }
            else
            {
                Debug.Log("Could not set player name:" + response.errorData.message);
            }
        });
        WaitForSeconds wait = new WaitForSeconds(1.5f);
        StartCoroutine(FetchTopHighscoresRoutine());
    }

    public void UpdateNameText(string name)
    {
        playerNames.text = name;
    }
}
