using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;
using Il2Cpp;

namespace DrunksModeExtraRoles;

[RegisterTypeInIl2Cpp]
public class Waiter : Role
{
    public override ActedInfo bcq(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        int charactersSize = characters.Count;
        int tableNum = getTablePos(charRef.id);
        int[] table = getTable(tableNum);
        int corrupted = 0;
        foreach (int charId in table)
        {
            Character ch = characters[charactersSize - charId];
            if (ch.statuses.fo(ECharacterStatus.Corrupted))
            {
                corrupted++;
            }
        }
        string line = string.Format("There are {0} Corruptions at my table.", corrupted);
        ActedInfo actedInfo = new ActedInfo(line);
        return actedInfo;
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.bcq(charRef));
        }
    }
    public override ActedInfo bcr(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        int charactersSize = characters.Count;
        int tableNum = getTablePos(charRef.id);
        int[] table = getTable(tableNum);
        int corrupted = 0;
        foreach (int charId in table)
        {
            Character ch = characters[charactersSize - charId];
            if (ch.statuses.fo(ECharacterStatus.Corrupted))
            {
                corrupted++;
            }
        }
        int rand = Calculator.tf(corrupted, 0, 5);
        string line = string.Format("There are {0} Corruptions at my table.", rand);
        ActedInfo actedInfo = new ActedInfo(line);
        return actedInfo;
    }
    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.bcr(charRef));
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
    public Waiter() : base(ClassInjector.DerivedConstructorPointer<Waiter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Waiter(IntPtr ptr) : base(ptr)
    {

    }
}