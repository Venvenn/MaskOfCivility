using System;
using System.Collections.Generic;
using System.Linq;
using Arch.Core;
using Escalon.Utility;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class Event
    {
        public Type Type;
        public Dictionary<(Entity, ActionExecutionPhase), List<Sequence>> _sequences;
        
        public Event(Type type)
        {
            Type = type;
            _sequences = new Dictionary<(Entity, ActionExecutionPhase), List<Sequence>>();
        }

        public int Count()
        {
            return _sequences.Count;
        }

        public void Add(Entity entity, ActionExecutionPhase phase, Sequence sequence)
        {
            if (!_sequences.ContainsKey((entity, phase)))
            {
                _sequences.Add((entity, phase), new List<Sequence>());
            }
            _sequences[(entity, phase)].Add(sequence);
        }
        
        public void Remove(Entity entity, ActionExecutionPhase phase, Sequence sequence)
        {
            if (_sequences.ContainsKey((entity, phase)))
            {
                _sequences[(entity, phase)].Remove(sequence);
            }
        }

        public void RemoveAll(Entity entity)
        {
            foreach (var eventSet in _sequences)
            {
                for (var i = eventSet.Value.Count - 1; i >= 0; --i)
                {
                    if (eventSet.Value[i].SequenceSource == entity)
                    {
                        eventSet.Value.RemoveAt(i);
                    }
                }
            }
        }
        
        public void Remove(Entity entity, Sequence sequence)
        {
            if (_sequences.ContainsKey((entity, ActionExecutionPhase.Before)))
            {
                _sequences[(entity, ActionExecutionPhase.Before)].Remove(sequence);
            }
            if (_sequences.ContainsKey((entity, ActionExecutionPhase.Execute)))
            {
                _sequences[(entity, ActionExecutionPhase.Execute)].Remove(sequence);
            }
            if (_sequences.ContainsKey((entity, ActionExecutionPhase.After)))
            {
                _sequences[(entity, ActionExecutionPhase.After)].Remove(sequence);
            }
        }

        public void RemoveAll(Sequence sequence)
        {
            var keys = _sequences.Keys.ToList();
            foreach (var key in keys)
            {
                if (_sequences[key].Contains(sequence))
                {
                    _sequences[key].Remove(sequence);
                }
            }
        }
        
        public List<Sequence> GetSequences(Entity entity, ActionExecutionPhase phase)
        {
            List<Sequence> sequences = new List<Sequence>();
            _sequences.TryGetValue((entity, phase), out var entitySequences);
            if (entitySequences != null)
            {
                sequences.AddRange(entitySequences);
            }
            
            if (entity != Entity.Null)
            {
                _sequences.TryGetValue((Entity.Null, phase), out var globalSequences); 
                if (globalSequences != null)
                {
                    sequences.AddRange(globalSequences);
                }
            }
            
            return sequences;
        }
        
        public List<Sequence> GetAllEntitySequences(Entity entity)
        {
            List<Sequence> sequences = new List<Sequence>();

            sequences.AddRange(GetSequences(entity, ActionExecutionPhase.Before));
            sequences.AddRange(GetSequences(entity, ActionExecutionPhase.Execute));
            sequences.AddRange(GetSequences(entity, ActionExecutionPhase.After));
            
            return sequences;
        }
    }
}

