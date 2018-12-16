using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    public class InterestComponent
    {
        private Dictionary<uint, ComponentInterest> interest;

        public InterestComponent()
        {
            interest = new Dictionary<uint, ComponentInterest>();
        }

        public InterestComponent AddComponentInterest<T>(ComponentInterest componentInterest)
            where T : ISpatialComponentData
        {
            var componentId = Dynamic.GetComponentId<T>();
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

        public InterestComponent AddQuery<T>(ComponentInterest.Query interestQuery)
            where T : ISpatialComponentData
        {
            var componentId = Dynamic.GetComponentId<T>();
            if (!interest.TryGetValue(componentId, out var query))
            {
                return AddComponentInterest<T>(new ComponentInterest(new List<ComponentInterest.Query>
                {
                    interestQuery
                }));
            }

            query.Queries.Add(interestQuery);
            return this;
        }

        public static implicit operator ComponentData(InterestComponent interestComponent)
        {
            return Improbable.Interest.Component.CreateSchemaComponentData(interestComponent.interest);
        }
    }
}
