using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;
using Il2Cpp;

namespace DrunksMode;

[RegisterTypeInIl2Cpp]
public class Bartender : Role
{
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Start)
        {
            return;
        }
        charRef.statuses.fm(ECharacterStatus.CorruptionResistant, charRef);
        charRef.statuses.fm(ECharacterStatus.UnkillableByDemon, charRef);
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
    public override CharacterData? bcz(Character charRef)
    {
        serveTable(charRef);
        /*
        Characters instance = Characters.Instance;
        Gameplay gameplay = Gameplay.Instance;
        Il2CppSystem.Collections.Generic.List<CharacterData> uniquePool = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        foreach (CharacterData data1 in instance.UniquePool)
        {
            if (data1.name != "Alchemist")
            {
                uniquePool.Add(data1);
            }
        }
        Il2CppSystem.Collections.Generic.List<CharacterData> currentTown = gameplay.currentTownsfolks;
        foreach (CharacterData data in currentTown)
        {
            if (data.name != "Alchemist")
            {
                uniquePool.Add(data);
            }
        }
        Il2CppSystem.Collections.Generic.List<CharacterData> townPool = instance.gw(uniquePool, ECharacterType.Villager);
        CharacterData randomData = townPool[UnityEngine.Random.RandomRangeInt(0, townPool.Count)];
        */
        if (GameData.GameMode is TavernMode)
        {
            TavernMode tavernMode = (TavernMode)GameData.GameMode;
            CharacterData? bartenderData = tavernMode.bartenderData;
            if (bartenderData != null)
            {
                tavernMode.replaceBartender(charRef, bartenderData);
            }
        }
        return null;
    }
    public void serveTable(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        int charactersSize = characters.Count;
        int tableNum = getTablePos(charRef.id);
        int[] table = getTable(tableNum);
        foreach (int charId in table)
        {
            Character ch = characters[charactersSize - charId];
            if (ch.dataRef.name == "Drunk")
            {
                return;
            }
        }
        foreach (int charId in table)
        {
            Character ch = characters[charactersSize - charId];
            if (ch.dataRef.name != "Puppet" && ch.dataRef.type != ECharacterType.Outcast && !ch.statuses.fo(ECharacterStatus.CorruptionResistant))
            {
                ch.statuses.fm(ECharacterStatus.Corrupted, charRef, null);
                if (ch.dataRef.type == ECharacterType.Minion || ch.dataRef.type == ECharacterType.Demon)
                {
                    ch.statuses.fm(ECharacterStatus.HealthyBluff, charRef, null);
                }
            }
        }
    }
    public Bartender() : base(ClassInjector.DerivedConstructorPointer<Bartender>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Bartender(IntPtr ptr) : base(ptr)
    {

    }
}