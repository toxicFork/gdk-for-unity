using System.Collections.Generic;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    public class InterestTemplate
    {
        private Dictionary<uint, ComponentInterest> interest;

        public InterestTemplate()
        {
            interest = new Dictionary<uint, ComponentInterest>();
        }

        public InterestTemplate AddComponentInterest(uint componentId, ComponentInterest componentInterest)
        {
            if (interest.ContainsKey(componentId))
            {
                Debug.LogWarning($"Skipping component with id {componentId}.");
            }
            else
            {
                interest.Add(componentId, componentInterest);
            }
            
            return this;
        }

        public InterestTemplate AddQuery(uint componentId, ComponentInterest.Query interestQuery)
        {
            if (!interest.TryGetValue(componentId, out var query))
            {
                return AddComponentInterest(componentId, new ComponentInterest(new List<ComponentInterest.Query>
                {
                    interestQuery
                }));
            }

            query.Queries.Add(interestQuery);
            return this;
        }

        public static implicit operator ComponentData(InterestTemplate interestTemplate)
        {
            return Improbable.Interest.Component.CreateSchemaComponentData(interestTemplate.interest);
        }
    }
}
