using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;
using Il2Cpp;

namespace DrunksMode;

[RegisterTypeInIl2Cpp]
public class Tavernkeeper : Role
{
    public override ActedInfo GetInfo(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        List<Character> newList = new List<Character>();
        foreach (Character character in characters)
        {
            bool corrupted = false;
            if (character.statuses != null)
            {
                corrupted = character.statuses.Contains(ECharacterStatus.Corrupted);
            }
            if (corrupted)
            {
                newList.Add(character);
            }
        }
        string line = string.Format("There are {0} Corrupted visitors", newList.Count);
        ActedInfo actedInfo = new ActedInfo(line);
        return actedInfo;
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return GetInfo(charRef);
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            charRef.statuses.AddStatus(ECharacterStatus.CorruptionResistant, charRef);
            charRef.statuses.AddStatus(ECharacterStatus.UnkillableByDemon, charRef);
            if (GameData.GameMode is TavernMode)
            {
                List<Character> allCharacters = Gameplay.CurrentCharacters;
                Characters instance = Characters.Instance;
                Gameplay gameplay = Gameplay.Instance;
                List<Character> list1 = instance.FilterAliveCharacters(allCharacters);
                List<Character> list2 = instance.FilterRealCharacterType(list1, ECharacterType.Villager);
                List<Character> list3 = new List<Character>();
                foreach (Character town in list2)
                {
                    if (town.dataRef.name != "Alchemist")
                    {
                        list3.Add(town);
                    }
                }
                Character randomCharacter = list3[UnityEngine.Random.RandomRangeInt(0, list3.Count)];
                TavernMode tavernMode = (TavernMode)GameData.GameMode;
                tavernMode.bartenderData = randomCharacter.GetCharacterBluffIfAble();
                randomCharacter.Init(TavernSave.bartender);
                gameplay.AddScriptCharacter(ECharacterType.Outcast, TavernSave.bartender);
            }
        }
            else if (trigger == ETriggerPhase.Day)
            {
                this.onActed.Invoke(this.GetInfo(charRef));
            }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        this.Act(trigger, charRef);
    }

    public Tavernkeeper() : base(ClassInjector.DerivedConstructorPointer<Tavernkeeper>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Tavernkeeper(IntPtr ptr) : base(ptr)
    {

    }
}