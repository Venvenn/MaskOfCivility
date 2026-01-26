using System.Collections.Generic;
using Arch.Core;

namespace Escalon.Traits
{
    /// <summary>
    /// System for interacting with and managing traits
    /// </summary>
    public static class TraitSystem
    {
        public static Trait CreateTrait(string name, TraitModifier[] traitValue = null, TraitModifier[] environmentModifiers = null, TraitSequence[] sequences = null)
        {
            return new Trait
            {
                Name = name,
                TraitValue = traitValue,
                EnvironmentModifiers = environmentModifiers,
                Sequences = sequences
            };
        }

        public static Trait CreateTrait(string name, TraitModifier traitValue)
        {
            return new Trait
            {
                Name = name,
                TraitValue = new[] { traitValue },
                EnvironmentModifiers = null,
                Sequences = null
            };
        }

        public static void AddTrait(Entity entity, Trait trait, CoreManagers coreManagers)
        {
            ref TraitData traitData = ref coreManagers.EntityManager.GetComponentReadWrite<TraitData>(entity);

            traitData.Traits ??= new List<Trait>();
            traitData.Traits.Add(trait);

            if (trait.TraitValue != null)
            {
                foreach (TraitModifier traitModifier in trait.TraitValue)
                {
                    ActionValueSystem.Add(entity, traitModifier.Value, traitModifier.Modifier, ActionValueType.Value, coreManagers.EntityManager);
                }
            }

            if (trait.EnvironmentModifiers != null)
            {
                foreach (TraitModifier traitModifier in trait.EnvironmentModifiers)
                {
                    ActionValueSystem.Add(entity, traitModifier.Value, traitModifier.Modifier, ActionValueType.Modifier, coreManagers.EntityManager);
                }
            }

            if (trait.Sequences != null)
            {
                foreach (TraitSequence sequence in trait.Sequences)
                {
                    if (sequence.SourceOnly)
                    {
                        coreManagers.ActionManager.AddSequence(sequence.Event, sequence.Sequence, entity);
                    }
                    else
                    {
                        coreManagers.ActionManager.AddSequence(sequence.Event, sequence.Sequence);
                    }
                }
            }
        }

        public static void RemoveTrait(Entity entity, Trait trait, CoreManagers coreManagers)
        {
            TraitData traitData = coreManagers.EntityManager.GetComponent<TraitData>(entity);
            traitData.Traits.Remove(trait);

            foreach (TraitModifier traitModifier in trait.TraitValue)
            {
                ActionValueSystem.Remove(entity, traitModifier.Value, traitModifier.Modifier, ActionValueType.Value, coreManagers);
            }

            foreach (TraitModifier traitModifier in trait.EnvironmentModifiers)
            {
                ActionValueSystem.Remove(entity, traitModifier.Value, traitModifier.Modifier, ActionValueType.Modifier, coreManagers);
            }

            foreach (TraitSequence sequence in trait.Sequences)
            {
                coreManagers.ActionManager.RemoveSequence(sequence.Event, sequence.Sequence);
            }
        }
    }
}