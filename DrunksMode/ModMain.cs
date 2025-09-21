using System;
using MelonLoader;
using DrunksMode;
using UnityEngine;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime;
using Il2CppSystem.Threading.Tasks;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;

[assembly: MelonInfo(typeof(ModMain), "Tavern Mod", "1.4", "SS122")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace DrunksMode;

public class ModMain : MelonMod
{
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<TavernMode>();
        ClassInjector.RegisterTypeInIl2Cpp<Bartender>();
        ClassInjector.RegisterTypeInIl2Cpp<Tavernkeeper>();
    }
    public override void OnLateInitializeMelon()
    {
        Application.runInBackground = true;
        var loadedAllSprites = Resources.FindObjectsOfTypeAll(Il2CppSystem.Type.GetTypeFromHandle(RuntimeReflectionHelper.GetRuntimeTypeHandle<Sprite>()));
        TavernSave.allSprites = new Sprite[loadedAllSprites.Length];
        for (int i = 0; i < loadedAllSprites.Length; i++)
        {
            TavernSave.allSprites[i] = loadedAllSprites[i]!.Cast<Sprite>();
        }
        GameObject leftUI = GameObject.Find("Game/Gameplay/Content/Canvas/UI/Objectives_Left");
        TavernSave.objTavernScore = GameObject.Instantiate(leftUI.transform.FindChild("Objective (13)").gameObject);
        TavernSave.objTavern = GameObject.Instantiate(leftUI.transform.FindChild("Objective (14) A").gameObject);
        TavernSave.objTavernScore.name = "Objectives (13) Tav";
        TavernSave.objTavern.name = "Objectives (14) Tav";
        TavernSave.objTavernScore.transform.SetParent(leftUI.transform);
        TavernSave.objTavern.transform.SetParent(leftUI.transform);
        TavernSave.objTavernScore.GetComponent<EnableOnMode>().mode = TavernSave.tavern;
        TavernSave.objTavern.GetComponent<EnableOnMode>().mode = TavernSave.tavern;
        TavernSave.objTavern.GetComponent<EnableOnMode>().mode2 = null;
        TavernSave.objTavernScore.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
        TavernSave.objTavern.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
        TavernSave.objTavernScore.SetActive(true);
        TavernSave.objTavern.SetActive(true);

        CharacterData bartender = TavernSave.createCharData("Bartender", "5", ECharacterType.Outcast,
        EAlignment.Good, new Bartender());
        bartender.bluffable = false;
        bartender.description = "I do not appear naturally.\n\nOne Villager gains the role of <b>Bartender.</b> The Bartender cannot be Corrupted or affected by Evils.";
        bartender.flavorText = "\"His drink-serving skills are unmatched. Though not like there was anyone to compete in the first place.\"";
        bartender.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        bartender.tags.Add(ECharacterTag.Corrupt);
        bartender.cardBgColor = new Color(0.196f, 0.101f, 0.034f);
        bartender.cardBorderColor = new Color(0.839f, 0.52f, 0.29f);
        bartender.color = new Color(1f, 0.5f, 0.4472f);
        bartender.hints = "Villagers in my table will become Corrupted, Evils in my table must tell the truth and will register as Corrupted.\n\nEffect cannot be reversed by Alchemist, but will be removed if a Drunk is present in my table.";
        bartender.characterId = "Bartender_TAVERN";
        TavernSave.bartender = bartender;

        CharacterData tavernkeeper = TavernSave.createCharData("Tavernkeeper", "5", ECharacterType.Outcast,
        EAlignment.Good, new Tavernkeeper());
        tavernkeeper.bluffable = false;
        tavernkeeper.art_cute = TavernSave.getSprite("demon06a1");
        tavernkeeper.description = "Learn the amount of Corrupted visitors in my Tavern";
        tavernkeeper.flavorText = "\"His intimidating stare is actually an act of welcoming his visitors.\"";
        tavernkeeper.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        tavernkeeper.tags.Add(ECharacterTag.Corrupt);
        tavernkeeper.cardBgColor = new Color(0.196f, 0.101f, 0.034f);
        tavernkeeper.cardBorderColor = new Color(0.839f, 0.52f, 0.29f);
        tavernkeeper.color = new Color(1f, 0.5f, 0.4472f);
        tavernkeeper.hints = "I am always Good. \nI can not Lie.";
        tavernkeeper.characterId = "Tavernkeeper_TAVERN";
        TavernSave.tavernkeeper = tavernkeeper;
        Characters.Instance.startGameActOrder = insertAfterAct("Puppet", bartender);
        Characters.Instance.startGameActOrder = insertAfterAct("Puppet", tavernkeeper);

        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        allCharactersAscension.startingOutsiders = appendToArray(allCharactersAscension.startingOutsiders,
        new List<CharacterData> { bartender, tavernkeeper });

        float[] circ18difX = new float[] { -60, 0, 20, 0, -80, -250, -105, -60, -20, 20, 60, 105, 250, 80, 0, -20, 0, 60 };
        float[] circ18difY = new float[] { 20, -40, -50, -40, -10, 50, -120, 50, 150, 150, 50, -120, 50, -10, -40, -50, -40, 20 };
        // circle radius = 390px
        GameObject circChar = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character");
        GameObject circCharLeft = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (1)");
        GameObject circCharRight = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (4)");
        GameObject circCharDown = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (3)");
        GameObject circ18 = new GameObject();
        circ18.name = "Circle_18";
        circ18.transform.SetParent(GameObject.Find("Game/Gameplay/Content/Canvas/Characters").transform);
        RectTransform circleRect = circ18.AddComponent<RectTransform>();
        CharactersPool circ18Pool = circ18.AddComponent<CharactersPool>();
        circ18Pool.characters = new Character[18];
        for (int i = 0; i < 6; i++)
        {
            int rotation = 20 * i + 300;
            GameObject card = GameObject.Instantiate(circCharLeft);
            card.transform.SetParent(circ18.transform);
            string cardname = "Character";
            if (i > 0)
            {
                cardname += " (" + i + ")";
            }
            card.name = cardname;
            Transform icon = card.transform.Find("Icon");
            card.transform.Rotate(0, 0, rotation);
            icon.Rotate(0, 0, 360 - rotation);
            int rotation2 = rotation - 300;
            float xdif = -390 * Mathf.Sin(Mathf.PI * rotation2 / 180f);
            float ydif = 390 * Mathf.Cos(Mathf.PI * rotation2 / 180f) - 390;
            float xdif2 = -390 * Mathf.Sin(Mathf.PI * (rotation2 + 10f) / 180f);
            float ydif2 = 390 * Mathf.Cos(Mathf.PI * (rotation2 + 10f) / 180f) - 390;
            card.transform.localPosition = new Vector3(circ18difX[i] + xdif2 - xdif, circ18difY[i] + ydif2 - ydif);
            circ18Pool.characters[i] = card.GetComponent<Character>();
        }
        for (int i = 6; i < 12; i++)
        {
            int rotation = 20 * i + 180;
            GameObject card = GameObject.Instantiate(circCharDown);
            card.transform.SetParent(circ18.transform);
            card.name = "Character (" + i + ")";
            Transform icon = card.transform.Find("Icon");
            card.transform.Rotate(0, 0, rotation);
            icon.Rotate(0, 0, 360 - rotation);
            int rotation2 = rotation - 180;
            float xdif = -390 * Mathf.Sin(Mathf.PI * rotation2 / 180f);
            float ydif = 390 * Mathf.Cos(Mathf.PI * rotation2 / 180f) - 390;
            float xdif2 = -390 * Mathf.Sin(Mathf.PI * (rotation2 + 10f) / 180f);
            float ydif2 = 390 * Mathf.Cos(Mathf.PI * (rotation2 + 10f) / 180f) - 390;
            card.transform.localPosition = new Vector3(circ18difX[i] + xdif2 - xdif, circ18difY[i] + ydif2 - ydif);
            circ18Pool.characters[i] = card.GetComponent<Character>();
        }
        for (int i = 12; i < 18; i++)
        {
            int rotation = 20 * i + 120;
            GameObject card = GameObject.Instantiate(circCharRight);
            card.transform.SetParent(circ18.transform);
            card.name = "Character (" + i + ")";
            Transform icon = card.transform.Find("Icon");
            card.transform.Rotate(0, 0, rotation);
            icon.Rotate(0, 0, 360 - rotation);
            int rotation2 = rotation - 120;
            float xdif = -390 * Mathf.Sin(Mathf.PI * rotation2 / 180f);
            float ydif = 390 * Mathf.Cos(Mathf.PI * rotation2 / 180f) - 390;
            float xdif2 = -390 * Mathf.Sin(Mathf.PI * (rotation2 + 10f) / 180f);
            float ydif2 = 390 * Mathf.Cos(Mathf.PI * (rotation2 + 10f) / 180f) - 390;
            card.transform.localPosition = new Vector3(circ18difX[i] + xdif2 - xdif, circ18difY[i] + ydif2 - ydif);
            circ18Pool.characters[i] = card.GetComponent<Character>();
        }
        circ18.transform.position = new UnityEngine.Vector3(0f, 1f, 85.9444f);
        circ18.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
        circ18.SetActive(false);
        TavernSave.addToCharsPool(circ18Pool);
    }
    public override void OnUpdate()
    {
        if (TavernSave.allData.Count == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                /*
                TavernSave.allData = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    TavernSave.allData[i] = loadedCharList[i]!.Cast<CharacterData>();
                }
                */
                AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
                foreach (CharacterData townData in allCharactersAscension.startingTownsfolks)
                {
                    TavernSave.allData.Add(townData);
                }
                foreach (CharacterData outsData in allCharactersAscension.startingOutsiders)
                {
                    TavernSave.allData.Add(outsData);
                }
                foreach (CharacterData miniData in allCharactersAscension.startingMinions)
                {
                    TavernSave.allData.Add(miniData);
                }
                foreach (CharacterData demoData in allCharactersAscension.startingDemons)
                {
                    TavernSave.allData.Add(demoData);
                }
            }
            if (TavernSave.allData.Count > 0)
            {
                TavernSave.createPool();
                TavernSave.tavernData.bbt();
            }
        }
        // Temp start Game option
        if (Input.GetKeyDown(KeyCode.V))
        {
            TavernSave.createPool();
            GameData.bgk(TavernSave.tavern);
        }
    }
    public CharacterData[] insertAfterAct(string previous, CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++)
        {
            if (inserted)
            {
                newActList[i + 1] = actList[i];
            }
            else
            {
                newActList[i] = actList[i];
                if (actList[i].name == previous)
                {
                    newActList[i + 1] = data;
                    inserted = true;
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
    public CharacterData[] appendToArray(CharacterData[] array, List<CharacterData> additions)
    {
        int len = array.Length;
        CharacterData[] newArray = new CharacterData[len + additions.Count];
        for (int i = 0; i < len; i++)
        {
            newArray[i] = array[i];
        }
        for (int j = 0; j < additions.Count; j++)
        {
            newArray[len + j] = additions[j];
        }
        return newArray;
    }
}
public static class TavernSave
{
    public static AscensionsData tavernData = GameObject.Instantiate(ProjectContext.Instance.gameData.advancedAscension);
    public static AscensionsData dataBase = GameObject.Instantiate(ProjectContext.Instance.gameData.advancedAscension);
    public static TavernMode tavern = new TavernMode();
    public static GameObject objTavernScore;
    public static GameObject objTavern;
    public static List<string> poolUnused = new List<string> { "Mutant", "Wretch", "Marionette", "Puppet", "Bounty Hunter", "Bartender" };
    public static List<int> pool = new List<int>();
    public static List<int> poolEvil = new List<int>();
    public static List<CharacterData> allData = new List<CharacterData>();
    public static Sprite[] allSprites = Array.Empty<Sprite>();
    public static CharacterData bartender;
    public static CharacterData tavernkeeper;
    public static ECharacterStatus bt = (ECharacterStatus)201;
    public static void createPool()
    {
        pool.Clear();
        poolEvil.Clear();
        for (int i = 0; i < allData.Count; i++)
        {
            if (!poolUnused.Contains(allData[i].name))
            {
                CharacterData data = allData[i];
                if (data.startingAlignment == EAlignment.Evil)
                {
                    poolEvil.Add(i);
                }
                pool.Add(i);
            }
        }
        AscensionsData ascensionsData = dataBase;
        tavernData.possibleScriptsData = new CustomScriptData[9];
        int j = 0;
        foreach (CustomScriptData customScriptData in ascensionsData.possibleScriptsData)
        {
            CustomScriptData newData = UnityEngine.Object.Instantiate(customScriptData);
            ScriptInfo script = new ScriptInfo();
            script.startingTownsfolks = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.startingOutsiders = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.startingMinions = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.startingDemons = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.mustInclude = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            // script.mustInclude.Add(bartender);
            script.mustInclude.Add(tavernkeeper);
            foreach (int roleNum in pool)
            {
                CharacterData roleData = allData[roleNum];
                switch (roleData.type)
                {
                    case ECharacterType.Villager:
                        script.startingTownsfolks.Add(roleData);
                        break;
                    case ECharacterType.Outcast:
                        script.startingOutsiders.Add(roleData);
                        break;
                    case ECharacterType.Minion:
                        script.startingMinions.Add(roleData);
                        break;
                    case ECharacterType.Demon:
                        script.startingDemons.Add(roleData);
                        break;
                }
            }
            script.characterCounts = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
            int[] roleCounts = randRoleCount(18);
            CharactersCount newCharCount = new CharactersCount(18, roleCounts[0], roleCounts[1], roleCounts[2], roleCounts[3]);
            script.characterCounts.Add(newCharCount);
            newData.scriptInfo = script;
            tavernData.possibleScriptsData[j] = newData;
            j++;
        }
    }
    public static int[] randRoleCount(int count)
    {
        int[] counts = { count - 1, 0, 1, 0 };
        List<int> tempPool = new List<int>(pool);
        List<int> tempPoolEvil = new List<int>(poolEvil);
        int evil = tempPool[UnityEngine.Random.RandomRangeInt(0, tempPoolEvil.Count)];
        if (allData[evil].type == ECharacterType.Minion)
        {
            counts[3]++;
        }
        else
        {
            counts[1]++;
        }
        tempPool.Remove(evil);
        counts[0]--;
        for (int i = 0; i < count - 1; i++)
        {
            int randomRole = tempPool[UnityEngine.Random.RandomRangeInt(0, tempPool.Count)];
            switch (allData[randomRole].type)
            {
                case ECharacterType.Outcast:
                    if (counts[2] < 4)
                    {
                        counts[2]++;
                        counts[0]--;
                    }
                    break;
                case ECharacterType.Minion:
                    counts[3]++;
                    counts[0]--;
                    break;
                case ECharacterType.Demon:
                    counts[1]++;
                    counts[0]--;
                    break;
                default:
                    break;
            }
            tempPool.Remove(randomRole);
        }
        return counts;
    }
    public static CharacterData? getData(string name)
    {
        foreach (CharacterData data in allData)
        {
            if (data.name == name)
            {
                return data;
            }
        }
        MelonLogger.Msg("Missing characterData \"" + name + "\"!");
        return null;
    }
    public static Sprite? getSprite(string name)
    {
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }
        MelonLogger.Msg("Missing sprite \"" + name + "\"!");
        return null;
    }
    public static CharacterData createCharData(string name, string bgName, ECharacterType type, EAlignment alignment, Role role, bool picking = false, EAbilityUsage abilityUsage = EAbilityUsage.Once)
    {
        // unadded: tags, description, notes, bluffable
        CharacterData newData = new CharacterData();
        newData.name = name;
        newData.abilityUsage = abilityUsage;
        newData.backgroundArt = getSprite(bgName);
        newData.bundledCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        newData.canAppearIf = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        switch (type)
        {
            case ECharacterType.Villager:
                newData.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
                newData.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
                newData.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
                newData.color = new Color(1f, 0.935f, 0.7302f);
                break;
            case ECharacterType.Outcast:
                newData.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
                newData.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
                newData.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
                newData.color = new Color(0.9659f, 1f, 0.4472f);
                break;
            case ECharacterType.Minion:
                newData.artBgColor = new Color(1f, 0f, 0f);
                newData.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                newData.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
                newData.color = new Color(0.8491f, 0.4555f, 0f);
                break;
            case ECharacterType.Demon:
                newData.artBgColor = new Color(1f, 0f, 0f);
                newData.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                newData.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
                newData.color = new Color(1f, 0.3811f, 0.3811f);
                break;
        }
        newData.descriptionCHN = "";
        newData.descriptionPL = "";
        newData.hints = "";
        newData.ifLies = "";
        newData.notes = "";
        newData.picking = picking;
        newData.role = role;
        newData.skins = new Il2CppSystem.Collections.Generic.List<SkinData>();
        newData.startingAlignment = alignment;
        newData.type = type;
        return newData;
    }
    public static void addToCharsPool(CharactersPool pool)
    {
        CharactersPool[] pools = Characters.Instance.characterPool;
        CharactersPool[] newPools = new CharactersPool[pools.Length + 1];
        for (int i = 0; i < pools.Length; i++)
        {
            newPools[i] = pools[i];
        }
        newPools[pools.Length] = pool;
        Characters.Instance.characterPool = newPools;
    }

    [HarmonyPatch(typeof(Character), nameof(Character.en))]
    public static class BartenderText
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.fo(bt))
            {
                __instance.chName.text += "<color=orange><size=18>\nBartender</color></size>";
            }
        }
    }
}
