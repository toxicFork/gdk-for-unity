using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.QueryBasedInterest
{
    public class Interest
    {
        private static readonly List<uint> EmptyList = new List<uint>();

        private ComponentInterest.Query query;

        public static Interest Query(ComponentInterest.QueryConstraint constraint)
        {
            var interest = new Interest
            {
                query =
                {
                    Constraint = constraint,
                    FullSnapshotResult = true,
                    ResultComponentId = EmptyList
                }
            };
            return interest;
        }

        public Interest MaxFrequencyHz(float frequency)
        {
            query.Frequency = frequency;
            return this;
        }

        public ComponentInterest.Query Filter(params uint[] resultComponentIds)
        {
            if (resultComponentIds.Length > 0)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>(resultComponentIds);
            }

            return query;
        }

        public static implicit operator ComponentInterest.Query(Interest interestBuilder)
        {
            return interestBuilder.query;
        }
    }
}
