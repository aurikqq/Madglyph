using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStage : MonoBehaviour
{
    [SerializeField] GameObject enterTip, enterButton, characterHead, characterBody, characterLegs;
    void firstStageEnabled()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
        enterTip.SetActive(true);
        #endif
        
        #if UNITY_MOBILE
        enterButton.SetActive(true);
        #endif
    }

    #if UNITY_STANDALONE || UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CharacterTreats characterTreats = new CharacterTreats();
            switch (characterHead.GetComponent<Image>().sprite.name)
            {
                case "bodies_set_0":
                    characterTreats.body = "female_0";
                    break;

                case "bodies_set_1":
                    characterTreats.body = "male_0";
                    break;

                case "bodies_set_2":
                    characterTreats.body = "male_1";
                    break;

                case "bodies_set_3":
                    characterTreats.body = "female_1";
                    break;
            }

            switch (characterLegs.GetComponent<Image>().sprite.name)
            {
                case "legs_set_0":
                    characterTreats.legs = "male_0";
                    break;

                case "legs_set_1":
                    characterTreats.legs = "female_0";
                    break;

                case "legs_set_2":
                    characterTreats.legs = "female_1";
                    break;

                case "legs_set_3":
                    characterTreats.legs = "male_1";
                    break;
            }

            switch (characterHead.GetComponent<Image>().sprite.name)
            {
                case "heads_set_0":
                    characterTreats.head = "female_0";
                    break;

                case "heads_set_1":
                    characterTreats.head = "female_1";
                    break;

                case "heads_set_2":
                    characterTreats.head = "male_0";
                    break;

                case "heads_set_3":
                    characterTreats.head = "male_1";
                    break;

                case "heads_set_4":
                    characterTreats.head = "idk_5th_variant";
                    break;
            }

            //CharacterTreats characterTreats = JsonUtility.FromJson<CharacterTreats>(File.ReadAllText(Application.streamingAssetsPath + "/characterTreats.json"));
            if (!File.Exists(Application.streamingAssetsPath + "/character_treats.json"))
            {
                File.Create(Application.streamingAssetsPath + "/character_treats.json").Dispose();
            }
            
            File.WriteAllText(Application.streamingAssetsPath + "/character_treats.json", JsonUtility.ToJson(characterTreats));
        }
    }
    #endif

    #if UNITY_MOBILE
    public void saveAppearance()
    {
        CharacterTreats characterTreats = new CharacterTreats();
        switch (characterHead.GetComponent<Image>().sprite.name)
        {
            case "bodies_set_0":
                characterTreats.body = "female_0";
                break;

            case "bodies_set_1":
                characterTreats.body = "male_0";
                break;

            case "bodies_set_2":
                characterTreats.body = "male_1";
                break;

            case "bodies_set_3":
                characterTreats.body = "female_1";
                break;
        }

        switch (characterLegs.GetComponent<Image>().sprite.name)
        {
            case "legs_set_0":
                characterTreats.legs = "male_0";
                break;

            case "legs_set_1":
                characterTreats.head = "female_0";
                break;

            case "legs_set_2":
                characterTreats.head = "female_1";
                break;

            case "legs_set_3":
                characterTreats.head = "male_1";
                break;
        }

        File.WriteAllText(Path.Combine(Application.persistentDataPath, "character_treats.json"), JsonUtility.ToJson(characterTreats));
    }
    #endif

    [System.Serializable]
    public class CharacterTreats
    {
        public string head;
        public string body;
        public string legs;
    }
}
