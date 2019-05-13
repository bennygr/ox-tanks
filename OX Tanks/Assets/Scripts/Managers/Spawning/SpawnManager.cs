using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private List<GameObject> tankPrefabs = new List<GameObject>(4);


    // FIXME: Couple skill prefabs with tank skills
    [SerializeField]
    private List<Image> skillImagePrefabs = new List<Image>(4);

    [SerializeField]
    private GameObject playerRigPrefab;

    // TODO: replace spawnPoints with a List
    [SerializeField]
    private GameObject spawnPointA;
    [SerializeField]
    private GameObject spawnPointB;

    [SerializeField]
    private Camera camera;
    [HideInInspector]
    private CameraControl cameraControl;


    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake() {
        if (camera == null) {
            Debug.LogError("No Camera attached to the SpanwManger.");
        }
        cameraControl = camera.transform.parent.gameObject.GetComponent<CameraControl>();
        if (cameraControl == null) {
            Debug.LogError("No CameraControl attached to the camera.");
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            Debug.Log("Spawned Rocket tank");
            initialiseTankPrefab(0, "Rocket Tank", 1);
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            Debug.Log("Spawed Mine tank");
            initialiseTankPrefab(1, "Mine Tank", 1);
        } else if (Input.GetKeyDown(KeyCode.F3)) {
            Debug.Log("Spawned Shotgun tank");
            initialiseTankPrefab(2, "Shotgun Tank", 1);
        } else if (Input.GetKeyDown(KeyCode.F4)) {
            Debug.Log("Spawned Grenade tank");
            initialiseTankPrefab(3, "Grenade Tank", 1);
        }
    }

    /// <summary>
    /// Initialises the tank prefab and makes all necessary connections.
    /// </summary>
    /// <param name="num">Tank prefab's number.</param>
    /// <param name="playerName">Player name.</param>
    /// <returns> The spawned GameObject</returns>
    public GameObject initialiseTankPrefab(int num, string playerName, int playerNumber) {
        if (num > tankPrefabs.Count) {
            return null;
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

        Image skillImage = skillImagePrefabs[num];
        assignTankClass(playerRig, tankPrefab, skillImage, num);
        playerRig.transform.position = getSpawnPosition(playerNumber);
        playerRig.GetComponent<TankVitals>().PlayerNumber = playerNumber;
        playerRig.GetComponent<TankVitals>().PlayerName = playerName;
        playerRig.GetComponent<TankVitals>().SkillCooldown.sprite = skillImage.sprite;

        cameraControl.AddCameraTarget(playerRig.transform);

        return playerRig;
    }

    private Vector3 getSpawnPosition(int playerNumber) {
        switch (playerNumber) {
            case 1:
                return spawnPointA.transform.position;
            case 2:
                return spawnPointB.transform.position;
            default:
                throw new Exception("Unable to spawn player with number " + playerNumber + ". Not enough spawn points");
        }
    }

    /// <summary>
    /// Assigns the tank class.
    /// </summary>
    /// <param name="playerRig">The player rig.</param>
    /// <param name="num">The tank class number</param>
    private void assignTankClass(GameObject playerRig, GameObject prefab, Image skillImage, int num) {
        switch (num) {
            case 0: {
                    RocketSkill rocketSkill = playerRig.AddComponent<RocketSkill>();
                    playerRig.GetComponent<RocketSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                    rocketSkill.SkillCooldown = skillImage;
                    break;
                }
            case 1: {
                    MineSkill mineSkill = playerRig.AddComponent<MineSkill>();
                    playerRig.GetComponent<MineSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                    mineSkill.SkillCooldown = skillImage;
                    break;
                }
            case 2: {
                    ShotgunSkill shotgunSkill = playerRig.AddComponent<ShotgunSkill>();
                    for (int i = 1; i <= 5; i++) {
                        playerRig.GetComponent<ShotgunSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform" + i).transform);
                    }
                    shotgunSkill.SkillCooldown = skillImage;
                    break;
                }
            case 3: {
                    GrenadeSkill grenadeSkill = playerRig.AddComponent<GrenadeSkill>();
                    for (int i = 1; i <= 8; i++) {
                        playerRig.GetComponent<GrenadeSkill>().addSkillTransform(prefab.transform.Find("SpecialFireTransform" + i).transform);
                    }
                    grenadeSkill.SkillCooldown = skillImage;
                    break;
                }
            default:
                RocketSkill skill = playerRig.AddComponent<RocketSkill>();
                playerRig.GetComponent<RocketSkill>().setSkillTransform(prefab.transform.Find("SpecialFireTransform").transform);
                skill.SkillCooldown = skillImage;
                break;
        }
    }
}
