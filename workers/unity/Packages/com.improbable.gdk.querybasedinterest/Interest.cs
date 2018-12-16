using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    public class Interest
    {
        private ComponentInterest.Query query;

        public static implicit operator ComponentInterest.Query(Interest interest)
        {
            return interest.query;
        }

        private Interest()
        {
            query = new ComponentInterest.Query
            {
                FullSnapshotResult = true
            }; 
        }
        
        public static Interest Query()
        {
            return new Interest();
        }
        
        public Interest SetFrequency(float frequency)
        {
            query.Frequency = frequency;
            return this;
        }
        
        public Interest ClearFrequency()
        {
            query.Frequency = null;
            return this;
        }
        
        public Interest SphereConstraint(double radius, Vector3 center)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                SphereConstraint = new ComponentInterest.SphereConstraint
                {
                    Center = new Coordinates(center.x, center.y, center.z),
                    Radius = radius
                }
            };

            return this;
        }
        
        public Interest CylinderConstraint(double radius, Vector3 center)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                CylinderConstraint = new ComponentInterest.CylinderConstraint()
                {
                    Center = new Coordinates(center.x, center.y, center.z),
                    Radius = radius
                }
            };

            return this;
        }
        
        public Interest BoxConstraint(double xLength, double yLength, double zLength, Vector3 center)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                BoxConstraint = new ComponentInterest.BoxConstraint
                {
                    Center = new Coordinates(center.x, center.y, center.z),
                    EdgeLength = new EdgeLength(xLength, yLength, zLength)
                }
            };

            return this;
        }
        
        public Interest RelativeSphereConstraint(double radius)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                RelativeSphereConstraint = new ComponentInterest.RelativeSphereConstraint
                {
                    Radius = radius
                }
            };

            return this;
        }
        
        public Interest RelativeCylinderConstraint(double radius)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                RelativeCylinderConstraint = new ComponentInterest.RelativeCylinderConstraint
                {
                    Radius = radius
                }
            };

            return this;
        }
        
        public Interest RelativeBoxConstraint(double xLength, double yLength, double zLength)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                RelativeBoxConstraint = new ComponentInterest.RelativeBoxConstraint
                {
                    EdgeLength = new EdgeLength(xLength, yLength, zLength)
                }
            };

            return this;
        }
        
        public Interest EntityIdConstraint(EntityId entityId)
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                EntityIdConstraint = entityId.Id
            };

            return this;
        }
        
        public Interest ComponentConstraint<T>() where T : ISpatialComponentData
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                ComponentConstraint = Dynamic.GetComponentId<T>()
            };

            return this;
        }
        
        public Interest And()
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>()
            };

            return this;
        }
        
        public Interest Or()
        {
            query.Constraint = new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>()
            };

            return this;
        }
        
        public Interest Return<T>() where T : ISpatialComponentData
        {
            if (query.FullSnapshotResult.HasValue)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>();
            }
            
            query.ResultComponentId.Add(Dynamic.GetComponentId<T>());

            return this;
        }
        
        public Interest Return<T, T2>() 
            where T : ISpatialComponentData 
            where T2 : ISpatialComponentData 
        {
            if (query.FullSnapshotResult.HasValue)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>();
            }
            
            query.ResultComponentId.Add(Dynamic.GetComponentId<T>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T2>());

            return this;
        }
        
        public Interest Return<T, T2, T3>() 
            where T : ISpatialComponentData 
            where T2 : ISpatialComponentData 
            where T3 : ISpatialComponentData
        {
            if (query.FullSnapshotResult.HasValue)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>();
            }
            
            query.ResultComponentId.Add(Dynamic.GetComponentId<T>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T2>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T3>());

            return this;
        }
        
        public Interest Return<T, T2, T3, T4>() 
            where T : ISpatialComponentData 
            where T2 : ISpatialComponentData 
            where T3 : ISpatialComponentData
            where T4 : ISpatialComponentData
        {
            if (query.FullSnapshotResult.HasValue)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>();
            }
            
            query.ResultComponentId.Add(Dynamic.GetComponentId<T>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T2>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T3>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T4>());

            return this;
        }
        
        public Interest Return<T, T2, T3, T4, T5>() 
            where T : ISpatialComponentData 
            where T2 : ISpatialComponentData 
            where T3 : ISpatialComponentData
            where T4 : ISpatialComponentData 
            where T5 : ISpatialComponentData
        {
            if (query.FullSnapshotResult.HasValue)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>();
            }
            
            query.ResultComponentId.Add(Dynamic.GetComponentId<T>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T2>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T3>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T4>());
            query.ResultComponentId.Add(Dynamic.GetComponentId<T5>());

            return this;
        }
    }
}
