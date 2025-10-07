using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using System;
using UnityEngine;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace DrunksModeExtraMinions;

[RegisterTypeInIl2Cpp]
public class Summoner : Minion
{
    public CharacterData evilDataRef = getBasicMinion();
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            evilDataRef = getBasicMinion();
            int closestClockwise = 0;
            List<Character> list1 = CharactersHelper.tl(Gameplay.CurrentCharacters, charRef);
            for (int i = 1; i < list1.Count; i++)
            {
                Character ch = list1[list1.Count - i];
                if (ch.dataRef.startingAlignment == EAlignment.Evil && ch.dataRef.characterId != "Summoner_TAVERN")
                {
                    this.evilDataRef = ch.dataRef;
                    closestClockwise = i;
                    break;
                }
            }
            for (int j = 1; j < list1.Count; j++)
            {
                Character ch = list1[j];
                if (ch.dataRef.startingAlignment == EAlignment.Evil && ch.dataRef.characterId != "Summoner_TAVERN")
                {
                    if (j < closestClockwise)
                    {
                        this.evilDataRef = ch.dataRef;
                    }
                    break;
                }
            }
        }
        evilDataRef.role.bcs(trigger, charRef);
    }
    public override void bct(Character charRef)
    {
        evilDataRef.role.bct(charRef);
    }
    public override CharacterData bcz(Character charRef)
    {
        CharacterData data = evilDataRef.role.bcz(charRef);
        // Cannot grab bcz from Minion
        return DrunkStatic.minion.bcz(charRef);
    }
    public static CharacterData getBasicMinion()
    {
        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        int i = 0;
        for (int j = 0; j < allCharactersAscension.startingMinions.Length; j++)
        {
            CharacterData miniData = allCharactersAscension.startingMinions[j];
            if (miniData.name == "Minion")
            {
                i = j;
            }
        }
        return allCharactersAscension.startingMinions[i];
    }
    public Summoner() : base(ClassInjector.DerivedConstructorPointer<Summoner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Summoner(IntPtr ptr) : base(ptr)
    {

    }
}
