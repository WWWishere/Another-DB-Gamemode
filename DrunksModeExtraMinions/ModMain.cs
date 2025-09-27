using System;
using System.Collections.Generic;
using DrunksModeExtraMinions;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ModMain), "Tavern Mod Minions Pack", "1.0", "SS122")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace DrunksModeExtraMinions;

public class ModMain : MelonMod
{
    public Sprite[] allSprites = Array.Empty<Sprite>();
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Strategist>();
    }
    public override void OnLateInitializeMelon()
    {
        var loadedAllSprites = Resources.FindObjectsOfTypeAll(Il2CppSystem.Type.GetTypeFromHandle(RuntimeReflectionHelper.GetRuntimeTypeHandle<Sprite>()));
        allSprites = new Sprite[loadedAllSprites.Length];
        for (int i = 0; i < loadedAllSprites.Length; i++)
        {
            allSprites[i] = loadedAllSprites[i]!.Cast<Sprite>();
        }

        CharacterData strategist = createCharData("Strategist", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Strategist());
        strategist.bluffable = false;
        strategist.description = "I Lie and Disguise.\n\nIf you are about to Execute me, I will swap places with a random Minion.";
        strategist.flavorText = "\"He doesn't need companions. The only thing he cares about is the brilliance stored in his mind.\"";
        strategist.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        strategist.characterId = "Strategist_TAVERN";

        CharacterData florist = createCharData("Florist", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Florist());
        florist.bluffable = false;
        florist.description = "I prevent one random good Villager's active ability from being used.\n\nI Lie and Disguise.";
        florist.flavorText = "\"Her red roses are pretty...\npretty deadly\"";
        florist.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        florist.characterId = "Florist_TAVERN";

        CharacterData gangster = createCharData("Gangster", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Gangster());
        gangster.bluffable = false;
        gangster.description = "<b>At Night:</b>\nIf there are adjacent hidden Good Characters, kill one and deal 2 damage to you.\n\nI Lie and Disguise.";
        gangster.flavorText = "\"He doesn't ghost people, he creates them.\"";
        gangster.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        gangster.hints = "I will not deal damage to you, if there is no one to kill.";
        gangster.characterId = "Gangster_TAVERN";

        CharacterData brewer = createCharData("Brewer", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Brewer());
        brewer.bluffable = false;
        brewer.description = "I am Corrupted, and will always tell the Truth.\n\nI Disguise as a random Villager or Outcast.";
        brewer.flavorText = "\"Some may say his obssession with alcohol is like... An alcoholic.\"";
        brewer.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        brewer.characterId = "Brewer_TAVERN";

        CharacterData summoner = createCharData("Summoner", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Summoner());
        summoner.bluffable = false;
        summoner.description = "I copy the abilities of the nearest Evil.\n\nI Lie and Disguise.";
        summoner.flavorText = "\"Even her summoning lines were copied from a different summoner.\"";
        summoner.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        summoner.hints = "I Lie and Disguise like a regular Minion.\nIf closest Evils are equidistant, I will pick the Evil that is Clockwise from me.";
        summoner.characterId = "Summoner_TAVERN";

        CharacterData trickster = createCharData("Trickster", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Trickster());
        trickster.bluffable = false;
        trickster.description = "Nearby Villagers register as a <b>Trickster.</b>\n\nI Lie and Disguise.";
        trickster.flavorText = "\"He'll force you to play his little game. In fact, he'll force everyone to play along with him.\"";
        trickster.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        trickster.characterId = "Trickster_TAVERN";

        GameObject content = GameObject.Find("Game/Gameplay/Content");
        NightPhase nightPhase = content.GetComponent<NightPhase>();
        nightPhase.nightCharactersOrder.Add(gangster);
        nightPhase.nightCharactersOrder.Add(summoner);

        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", florist);
        Characters.Instance.startGameActOrder = insertAfterAct("Lilis", summoner);
        Characters.Instance.startGameActOrder = insertAfterAct("Shaman", trickster);

        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        allCharactersAscension.startingMinions = appendToArray(allCharactersAscension.startingMinions,
        new List<CharacterData> { strategist, florist, gangster, brewer, summoner, trickster });
    }
    public CharacterData createCharData(string name, string bgName, ECharacterType type, EAlignment alignment, Role role, bool picking = false, EAbilityUsage abilityUsage = EAbilityUsage.Once)
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
    public Sprite? getSprite(string name)
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
public static class DrunkStatic
{
    public static ECharacterStatus unusable = (ECharacterStatus)202;
    public static Minion minion = new Minion();
}