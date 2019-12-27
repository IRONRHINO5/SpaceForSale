using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{

    private NavMeshAgent player;
    private bool started;
    GameFlow GameFlowInstance;
    GameData GameDataInstance;
    ChanceFunctions ChanceController;

    bool orbitOn;

    private void Update()
    {
    }

    // Use this for initialization
    void Start ()
    {
        player = PlayerNetwork.Instance.GamePiece.GetComponent<NavMeshAgent>();
        started = false;
        orbitOn = false;
        GameFlowInstance = GameFlow.GameFlowInstance;
        GameDataInstance = GameObject.Find("GameData").GetComponent<GameData>();
        ChanceController = GameObject.Find("GameData").GetComponent<ChanceFunctions>();
    }
    
    public void Move(Vector3 target)
    {
        StartCoroutine(Mover(target));
    }

    public void Orbit(GameObject planet, Quaternion originalRotation)
    {
        StartCoroutine(Orbiter(planet, originalRotation));
    }

    IEnumerator Mover(Vector3 target)
    {
        Text txtDiceRoll = GameObject.Find("DiceText").GetComponent<Text>();
        int diceRoll = Int32.Parse(txtDiceRoll.text);

        //gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);

        orbitOn = false;
        NavMeshPath path = new NavMeshPath();
        player.CalculatePath(target, path);
        player.SetPath(path);
        while ((Vector3.Distance(player.destination, player.transform.position) >= player.stoppingDistance) &&
               (player.hasPath || player.velocity.sqrMagnitude != 0f))
        {
            if (player.isOnOffMeshLink && !started)
            {
                StartCoroutine(Parabola(0.0f, 1.0f));
                started = true;
                txtDiceRoll.text = "Moving " + diceRoll.ToString() + " \"spaces\"";
                diceRoll--;
            }
            yield return null;
        }
        GameFlowInstance.GateSwitcher();

        // Show property card
        if (GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().BoardPosition]].GetComponent<Property>())
        {
            GameObject.Find("HUD").GetComponentInChildren<PlanetInfo>().show();

            orbitOn = true;
            Orbit(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().BoardPosition]], gameObject.transform.GetChild(0).rotation);
        }
        else if (GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().BoardPosition]].GetComponent<Chance>())
        {
            ChanceController.ChanceSelection();
        }
        else
        {
            GameDataInstance.ActivePlayer++;
        }
        // Make dice text disappear when player finshes moving
        GameObject.Find("DiceText").GetComponent<Text>().text = "";
    }

    IEnumerator Orbiter(GameObject planet, Quaternion originalRotation)
    {
        while (orbitOn)
        {
            gameObject.transform.GetChild(0).RotateAround(planet.transform.position, gameObject.transform.right, 40 * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.GetChild(0).position = planet.transform.GetChild(0).position;
        gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(0,0,0);
        orbitOn = false;
    }

    IEnumerator Parabola(float height, float duration)
    {
        OffMeshLinkData data = player.currentOffMeshLinkData;
        Vector3 startPos = player.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * player.baseOffset;
        Quaternion targetRot = Quaternion.LookRotation(endPos - startPos);
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            player.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            //player.transform.LookAt(endPos);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRot, normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        player.CompleteOffMeshLink();
        started = false;
        //do the dice decrement here!!

    }

    public void Teleport(Vector3 target)
    {
        orbitOn = false;
        player.Warp(target);
        orbitOn = true;
        Orbit(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().BoardPosition]], gameObject.transform.GetChild(0).rotation);
    }
}
