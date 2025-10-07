using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;
using Il2Cpp;

namespace DrunksModeExtraRoles;

[RegisterTypeInIl2Cpp]
public class BeerThower : Demon
{
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            startCorrupt(charRef);
        }   
    }
    public int[] getTable(int tableNum)
    {
        switch (tableNum)
        {
            case 1:
                return new int[] { 13, 14, 15, 16, 17, 18 };
            case 2:
                return new int[] { 7, 8, 9, 10, 11, 12 };
            case 3:
                return new int[] { 1, 2, 3, 4, 5, 6 };
            default:
                return Array.Empty<int>();
        }
    }
    public int getTablePos(int id)
    {
        if (id >= 7 && id <= 12)
        {
            return 2;
        }
        if (id >= 13 && id <= 18)
        {
            return 1;
        }
        return 3;
    }
    public void startCorrupt(Character charRef)
    {
        System.Collections.Generic.List<int> tables = new System.Collections.Generic.List<int>() { 1, 2, 3 };
        List<Character> characters = Gameplay.CurrentCharacters;
        int charactersSize = characters.Count;
        List<Character> list1 = new List<Character>();
        Characters instance = Characters.Instance;
        if (characters.Count == 18)
        {
            foreach (Character character in characters)
            {
                if (character.dataRef.name == "Bartender" || character.statuses.fo((ECharacterStatus)201))
                {
                    int barTable = getTablePos(character.id);
                    tables.Remove(barTable);
                }
            }
            foreach (int tableNum in tables)
            {
                int[] table = getTable(tableNum);
                foreach (int charId in table)
                {
                    Character ch = characters[charactersSize - charId];
                    bool corrupted = ch.statuses.fo(ECharacterStatus.Corrupted);
                    bool beerThrower = ch.id == charRef.id;
                    bool notCorrupt = ch.statuses.fo(ECharacterStatus.CorruptionResistant) || ch.dataRef.name == "Puppet";
                    if (!corrupted && !beerThrower && !notCorrupt)
                    {
                        list1.Add(ch);
                    }
                }
            }
        }
        else
        {
            foreach (Character ch in characters)
            {
                bool corrupted = ch.statuses.fo(ECharacterStatus.Corrupted);
                bool beerThrower = ch.id == charRef.id;
                bool notCorrupt = ch.statuses.fo(ECharacterStatus.CorruptionResistant) || ch.dataRef.name == "Puppet";
                if (!corrupted && !beerThrower && !notCorrupt)
                {
                    list1.Add(ch);
                }
            }
        }
        List<Character> list2 = instance.gs(list1, ECharacterType.Villager);
        List<Character> list3 = instance.gs(list1, ECharacterType.Minion);
        List<Character> list4 = instance.gs(list1, ECharacterType.Demon);
        foreach (Character demon in list4)
        {
            list3.Add(demon);
        }
        if (list2.Count > 0)
        {
            Character randomVillager = list2[UnityEngine.Random.RandomRangeInt(0, list2.Count)];
            randomVillager.statuses.fm(ECharacterStatus.Corrupted, charRef);
            randomVillager.statuses.fm(ECharacterStatus.MessedUpByEvil, charRef);
            list2.Remove(randomVillager);
        }
        if (list3.Count > 0)
        {
            Character randomMinion = list3[UnityEngine.Random.RandomRangeInt(0, list3.Count)];
            randomMinion.statuses.fm(ECharacterStatus.Corrupted, charRef);
            randomMinion.statuses.fm(ECharacterStatus.HealthyBluff, charRef);
            randomMinion.statuses.fm(ECharacterStatus.MessedUpByEvil, charRef);
        }
        else
        {
            if (list2.Count > 0)
            {
                Character randomVillager = list2[UnityEngine.Random.RandomRangeInt(0, list2.Count)];
                randomVillager.statuses.fm(ECharacterStatus.Corrupted, charRef);
                randomVillager.statuses.fm(ECharacterStatus.MessedUpByEvil, charRef);
                list2.Remove(randomVillager);
            }
        }
    }
    public BeerThower() : base(ClassInjector.DerivedConstructorPointer<BeerThower>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public BeerThower(IntPtr ptr) : base(ptr)
    {

    }
}