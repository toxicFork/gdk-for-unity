using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.QueryBasedInterest
{
    public class Interest
    {
        internal ComponentInterest.Query query;

        public static implicit operator ComponentInterest.Query(Interest interest)
        {
            return interest.query;
        }
        
        protected internal Interest()
        {
            query = new ComponentInterest.Query
            {
                FullSnapshotResult = true
            }; 
        }
        
        public static Constraint Query()
        {
            return new Constraint();
        }
        
        public Interest Frequency(float frequency)
        {
            query.Frequency = frequency;
            return this;
        }
        
        public Interest Result<T>() where T : ISpatialComponentData
        {
            if (query.FullSnapshotResult.HasValue)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>();
            }
            
            query.ResultComponentId.Add(Dynamic.GetComponentId<T>());
            
            return this;
        }
        
        public Interest Result<T, T2>() 
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
        
        public Interest Result<T, T2, T3>() 
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
        
        public Interest Result<T, T2, T3, T4>() 
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
        
        public Interest Result<T, T2, T3, T4, T5>() 
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
