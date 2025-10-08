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
public class Gangster : Minion
{
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("");
    }
    public override List<SpecialRule> GetRules()
    {
        List<SpecialRule> rules = new List<SpecialRule>();
        // Not the best looking thing
        Gameplay gameplay = Gameplay.Instance;
        List<CharacterData> characterDatas = gameplay.GetScriptCharacters();
        foreach (CharacterData data in characterDatas)
        {
            if (data.name == "Lilis")
            {
                return rules;
            }
        }
        rules.Add(new NightModeRule(4));
        return rules;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (charRef.state == ECharacterState.Dead)
        {
            return;
        }
        if (trigger == ETriggerPhase.Night)
        {
            List<Character> characters = Gameplay.CurrentCharacters;
            List<Character> list1 = CharactersHelper.GetSortedListWithCharacterFirst(characters, charRef);
            List<Character> targetable = new List<Character>();
            checkAddNeighbor(list1[1], targetable);
            checkAddNeighbor(list1[list1.Count - 1], targetable);
            if (targetable.Count == 0)
            {
                return;
            }
            Character random = targetable[UnityEngine.Random.RandomRange(0, targetable.Count)];
            Health health = PlayerController.PlayerInfo.health;
            health.Damage(2);
            random.KillByDemon(charRef);
        }
    }
    public void checkAddNeighbor(Character neighbor, List<Character> list)
    {
        if (neighbor.statuses.Contains(ECharacterStatus.UnkillableByDemon))
        {
            return;
        }
        if (neighbor.state == ECharacterState.Hidden && neighbor.alignment == EAlignment.Good)
        {
            list.Add(neighbor);
        }
    }
    public Gangster() : base(ClassInjector.DerivedConstructorPointer<Gangster>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Gangster(IntPtr ptr) : base(ptr)
    {

    }
}
