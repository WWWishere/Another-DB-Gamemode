using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;
using Il2Cpp;

namespace DrunksModeExtraRoles;

[RegisterTypeInIl2Cpp]
public class Chef : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        int[] groups = new int[] { 0, 0, 0 };
        int[] groupsShuffled = new int[3];
        List<Character> characters = Gameplay.CurrentCharacters;
        int charactersSize = characters.Count;
        for (int i = 1; i <= 3; i++)
        {
            int[] table = getTable(i);
            foreach (int charId in table)
            {
                Character ch = characters[charactersSize - charId];
                if (ch.alignment == EAlignment.Evil)
                {
                    groups[i - 1]++;
                }
            }
        }
        System.Collections.Generic.List<int> positions = new System.Collections.Generic.List<int>() { 0, 1, 2 };
        for (int j = 0; j < 3; j++)
        {
            int randomPos = positions[UnityEngine.Random.RandomRangeInt(0, positions.Count)];
            groupsShuffled[randomPos] = groups[j];
            positions.Remove(randomPos);
        }
        string line = string.Format("Evil conspires in groups of {0}, {1} and {2}.", groupsShuffled[0], groupsShuffled[1], groupsShuffled[2]);
        ActedInfo actedInfo = new ActedInfo(line);
        return actedInfo;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.GetInfo(charRef));
        }
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        int[] groups = new int[] { 0, 0, 0 };
        int[] groupsShuffled = new int[3];
        List<Character> characters = Gameplay.CurrentCharacters;
        int charactersSize = characters.Count;
        for (int i = 1; i <= 3; i++)
        {
            int[] table = getTable(i);
            foreach (int charId in table)
            {
                Character ch = characters[charactersSize - charId];
                if (ch.alignment == EAlignment.Evil)
                {
                    groups[i - 1]++;
                }
            }
        }
        // MelonLogger.Msg(string.Format("Chef lie data: real values are  {0}  {1}  {2}", groups[0], groups[1], groups[2]));
        int sum = groups[0] + groups[1] + groups[2];
        if (sum < 2)
        {
            groups[0] = 1;
            groups[1] = 1;
            groups[2] = 0;
        }
        else
        {
            int rand1 = 0;
            int rand2 = 0;
            int kmax = sum;
            int ksmallMax = kmax / 2;
            if (kmax >= 3 && hasSize(groups, kmax - 1))
            {
                kmax--;
            }
            System.Collections.Generic.List<int> pickableFirst = new System.Collections.Generic.List<int>();
            System.Collections.Generic.List<int> pickableLast = new System.Collections.Generic.List<int>();
            System.Collections.Generic.List<int> groupsRemovable = new System.Collections.Generic.List<int>(groups);
            for (int k = 0; k < kmax; k++)
            {
                if (sum >= 3)
                {
                    pickableFirst.Add(k);
                    if (k <= ksmallMax && k > 0)
                    {
                        pickableFirst.Add(k);
                    }
                }
                if (!hasSize(groups, k))
                {
                    pickableFirst.Add(k);
                }
            }
            if (pickableFirst.Count < 1)
            {
                pickableFirst.Add(2);
            }
            rand1 = pickableFirst[UnityEngine.Random.RandomRangeInt(0, pickableFirst.Count)];
            int remain = sum - rand1;
            if (hasSize(groups, rand1))
            {
                groupsRemovable.Remove(rand1);
                for (int l = 0; l < remain; l++)
                {
                    if (!groupsRemovable.Contains(l))
                    {
                        pickableLast.Add(l);
                    }
                }
                rand2 = pickableLast[UnityEngine.Random.RandomRangeInt(0, pickableLast.Count)];
            }
            else
            {
                if (remain > 0)
                {
                    rand2 = UnityEngine.Random.RandomRangeInt(0, remain);
                }
            }
            groups[0] = rand1;
            groups[1] = rand2;
            groups[2] = sum - rand1 - rand2;
        }
        // MelonLogger.Msg(string.Format("Chef lie data: fake values are  {0}  {1}  {2}", groups[0], groups[1], groups[2]));
        System.Collections.Generic.List<int> positions = new System.Collections.Generic.List<int>() { 0, 1, 2 };
        for (int j = 0; j < 3; j++)
        {
            int randomPos = positions[UnityEngine.Random.RandomRangeInt(0, positions.Count)];
            groupsShuffled[randomPos] = groups[j];
            positions.Remove(randomPos);
        }
        string line = string.Format("Evil conspires in groups of {0}, {1} and {2}.", groupsShuffled[0], groupsShuffled[1], groupsShuffled[2]);
        ActedInfo actedInfo = new ActedInfo(line);
        return actedInfo;
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.GetBluffInfo(charRef));
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
    public bool hasSize(int[] nums, int num)
    {
        foreach (int n in nums)
        {
            if (n == num)
            {
                return true;
            }
        }
        return false;
    }
    public Chef() : base(ClassInjector.DerivedConstructorPointer<Chef>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Chef(IntPtr ptr) : base(ptr)
    {

    }
}