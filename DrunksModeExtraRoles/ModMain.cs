using System;
using MelonLoader;
using DrunksModeExtraRoles;
using Il2CppInterop.Runtime.Injection;
using Il2Cpp;
using UnityEngine;
using Il2CppInterop.Runtime;
using System.Collections.Generic;

[assembly: MelonInfo(typeof(ModMain), "Tavern Mod Extra Roles", "1.1", "SS122")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace DrunksModeExtraRoles;

public class ModMain : MelonMod
{
    public Sprite[] allSprites = Array.Empty<Sprite>();
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Waiter>();
        ClassInjector.RegisterTypeInIl2Cpp<Chef>();
        ClassInjector.RegisterTypeInIl2Cpp<BeerThower>();
    }
    public override void OnLateInitializeMelon()
    {
        var loadedAllSprites = Resources.FindObjectsOfTypeAll(Il2CppSystem.Type.GetTypeFromHandle(RuntimeReflectionHelper.GetRuntimeTypeHandle<Sprite>()));
        allSprites = new Sprite[loadedAllSprites.Length];
        for (int i = 0; i < loadedAllSprites.Length; i++)
        {
            allSprites[i] = loadedAllSprites[i]!.Cast<Sprite>();
        }

        CharacterData waiter = createCharData("Waiter", "2 1", ECharacterType.Villager,
        EAlignment.Good, new Waiter());
        waiter.bluffable = true;
        waiter.description = "Learn how many Corrupted characters are at my table.";
        waiter.flavorText = "\"He'll take your order, then order your tab. Hope you're not too drunk to pay.\"";
        waiter.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        waiter.tags.Add(ECharacterTag.Corrupt);
        waiter.hints = "If I am Corrupted, I will count as a Corrupted in my table.";
        waiter.characterId = "Waiter_TAVERN";

        CharacterData chef = createCharData("Chef", "2 1", ECharacterType.Villager,
        EAlignment.Good, new Chef());
        chef.bluffable = true;
        chef.description = "Learn how many Evil characters sit at each table, but not which number belongs to which table.";
        chef.flavorText = "\"She'll cook up any storm for you, but there are some things that Good just doesn't order.\"";
        chef.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        chef.characterId = "Chef_TAVERN";

        CharacterData beerThrower = createCharData("Beer Thrower", "evil", ECharacterType.Demon,
        EAlignment.Evil, new BeerThower());
        beerThrower.bluffable = false;
        beerThrower.description = "<b>Game Start:</b>\nA random Villager and Evil are Corrupted.\n\nI Lie and Disguise.";
        beerThrower.flavorText = "\"Has been caught at least 20 times for hitting random people with Beers. Refuses to change his habits.\"";
        beerThrower.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        beerThrower.tags.Add(ECharacterTag.Corrupt);
        beerThrower.hints = "I cannot Corrupt Characters in the same table as the Bartender.\nIf there are no Evils to pick, I will Corrupt a second Villager instead.";
        beerThrower.characterId = "BeerThrower_TAVERN";
        Characters.Instance.startGameActOrder = insertAfterAct("Lilis", beerThrower);

        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        allCharactersAscension.startingTownsfolks = appendToArray(allCharactersAscension.startingTownsfolks,
        new List<CharacterData> { waiter, chef });
        allCharactersAscension.startingDemons = appendToArray(allCharactersAscension.startingDemons,
        new List<CharacterData> { beerThrower });
        GameObject objCompendium = GameObject.Find("Game/Menu/Compendium");
        Compendium compendium = objCompendium.GetComponent<Compendium>();
        var pages = compendium.pages;
        CharactersCompendiumPage pageVillage2 = pages[1];
        CharactersCompendiumPage pageDemon = pages[4];
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

