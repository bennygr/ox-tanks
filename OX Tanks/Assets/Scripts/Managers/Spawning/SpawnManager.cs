using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private List<GameObject> tankPrefabs = new List<GameObject>(4);

    [SerializeField]
    private GameObject playerRigPrefab;

    private ChaseCamera playerCamera;
    private ChaseCamera2D playerCamera2D;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake() {
        GameObject mainCam = GameObject.Find("Main Camera").gameObject;
        playerCamera = mainCam.GetComponent<ChaseCamera>();

        GameObject playerCam = GameObject.Find("CameraRig2D").gameObject;
        playerCamera2D = playerCam.GetComponent<ChaseCamera2D>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            Debug.Log("Spawn MW tank");
            initialiseTankPrefab(0, "MW Tank");
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            Debug.Log("Spawn QA tank");
            initialiseTankPrefab(1, "QA Tank");
        } else if (Input.GetKeyDown(KeyCode.F3)) {
            Debug.Log("Spawn UI tank");
            initialiseTankPrefab(2, "UI Tank");
        } else if (Input.GetKeyDown(KeyCode.F4)) {
            Debug.Log("Spawn Finance tank");
            initialiseTankPrefab(3, "Finance Tank");
        }
    }

    /// <summary>
    /// Initialises the tank prefab and makes all necessary connections.
    /// </summary>
    /// <param name="num">Tank prefab's number.</param>
    /// <param name="playerName">Player name.</param>
    private void initialiseTankPrefab(int num, string playerName) {
        if (num > tankPrefabs.Count) {
            return;
        }
        GameObject exists = GameObject.Find(playerName);
        if (exists) {
            Destroy(exists);
        }
        GameObject tankPrefab = Instantiate(tankPrefabs[num]);
        tankPrefab.name = playerName + " Geometry";

        GameObject playerRig = Instantiate(playerRigPrefab);
        playerRig.name = playerName;

        GameObject modelNode = playerRig.transform.Find("Model").gameObject;
        tankPrefab.transform.parent = modelNode.transform;

        playerRig.GetComponent<PrimaryFire>().setFireTransform(tankPrefab.transform.Find("MainFireTransform").transform);

        Text[] texts = playerRig.transform.GetComponentsInChildren<Text>();
        Text displayName = null;
        foreach (Text text in texts) {
            if (text.name == "DisplayName") {
                displayName = text;
                break;
            }
        }
        if (displayName != null) {
            displayName.text = playerName;
        } else {
            Debug.LogWarningFormat("No display name was set for player {0}", playerName);
        }

        GameObject cameraRig = playerRig.transform.Find("CameraRig").gameObject;
        playerCamera.setMountPoint(cameraRig.transform.Find("CameraMount").gameObject.transform);
        playerCamera.setLookAtTarget(cameraRig.transform.Find("CameraLookAtTarget").gameObject.transform);

        playerCamera2D.setTargetToFollow(playerRig.transform);

        assignTankClass(playerRig, tankPrefab, num);
    }

    /// <summary>
    /// Assigns the tank class.
    /// </summary>
    /// <param name="playerRig">The player rig.</param>
    /// <param name="num">The tank class number</param>
    private void assignTankClass(GameObject playerRig, GameObject prefab, int num) {
        switch (num) {
            case 0:
                playerRig.AddComponent<RocketSkill>();
                playerRig.GetComponent<RocketSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                break;
            case 1:
                playerRig.AddComponent<MineSkill>();
                playerRig.GetComponent<MineSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                break;
            case 2:
                playerRig.AddComponent<ShotgunSkill>();
                playerRig.GetComponent<ShotgunSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform1").transform);
                playerRig.GetComponent<ShotgunSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform2").transform);
                playerRig.GetComponent<ShotgunSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform3").transform);
                playerRig.GetComponent<ShotgunSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform4").transform);
                playerRig.GetComponent<ShotgunSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform5").transform);
                break;
            case 3:
                playerRig.AddComponent<GrenadeSkill>();
                playerRig.GetComponent<GrenadeSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                break;
            default:
                playerRig.AddComponent<RocketSkill>();
                playerRig.GetComponent<RocketSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                break;
        }
    }
}