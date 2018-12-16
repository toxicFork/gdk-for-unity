using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    public class Constraint
    {
        public Interest SphereConstraint(double radius, Vector3 center)
        {
            var interest = new Interest();
            interest.query.Constraint = Sphere(radius, center);
            return interest;
        }
        
        public Interest CylinderConstraint(double radius, Vector3 center)
        {
            var interest = new Interest();
            interest.query.Constraint = Cylinder(radius, center);
            return interest;
        }
        
        public Interest BoxConstraint(double xLength, double yLength, double zLength, Vector3 center)
        {
            var interest = new Interest();
            interest.query.Constraint = Box(xLength, yLength, zLength, center);
            return interest;
        }
        
        public Interest RelativeSphereConstraint(double radius)
        {
            var interest = new Interest();
            interest.query.Constraint = RelativeSphere(radius);
            return interest;
        }
        
        public Interest RelativeCylinderConstraint(double radius)
        {
            var interest = new Interest();
            interest.query.Constraint = RelativeCylinder(radius);
            return interest;
        }
        
        public Interest RelativeBoxConstraint(double xLength, double yLength, double zLength)
        {
            var interest = new Interest();
            interest.query.Constraint = RelativeBox(xLength, yLength, zLength);
            return interest;
        }
        
        public Interest EntityIdConstraint(EntityId entityId)
        {
            var interest = new Interest();
            interest.query.Constraint = EntityId(entityId);
            return interest;
        }
        
        public Interest ComponentConstraint<T>() where T : ISpatialComponentData
        {
            var interest = new Interest();
            interest.query.Constraint = Component<T>();
            return interest;
        }
        
        public Interest ComponentConstraint(uint componentId)
        {
            var interest = new Interest();
            interest.query.Constraint = Component(componentId);
            return interest;
        }
        
        public Interest All(params ComponentInterest.QueryConstraint[] constraints)
        {
            var interest = new Interest();
            interest.query.Constraint = All((IEnumerable<ComponentInterest.QueryConstraint>) constraints);
            return interest;
        }
        
        public Interest Any(params ComponentInterest.QueryConstraint[] constraints)
        {
            var interest = new Interest();
            interest.query.Constraint = Any((IEnumerable<ComponentInterest.QueryConstraint>) constraints);
            return interest;
        }
        
        public static ComponentInterest.QueryConstraint Sphere(double radius, Vector3 center)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                SphereConstraint = new ComponentInterest.SphereConstraint
                {
                    Center = new Coordinates(center.x, center.y, center.z),
                    Radius = radius
                }
            };
        }
        
        public static ComponentInterest.QueryConstraint Cylinder(double radius, Vector3 center)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                CylinderConstraint = new ComponentInterest.CylinderConstraint()
                {
                    Center = new Coordinates(center.x, center.y, center.z),
                    Radius = radius
                }
            };
        }
        
        public static ComponentInterest.QueryConstraint Box(double xLength, double yLength, double zLength, Vector3 center)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                BoxConstraint = new ComponentInterest.BoxConstraint
                {
                    Center = new Coordinates(center.x, center.y, center.z),
                    EdgeLength = new EdgeLength(xLength, yLength, zLength)
                }
            };
        }
        
        public static ComponentInterest.QueryConstraint RelativeSphere(double radius)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                RelativeSphereConstraint = new ComponentInterest.RelativeSphereConstraint
                {
                    Radius = radius
                }
            };
        }
        
        public static ComponentInterest.QueryConstraint RelativeCylinder(double radius)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                RelativeCylinderConstraint = new ComponentInterest.RelativeCylinderConstraint
                {
                    Radius = radius
                }
            };
        }
        
        public static ComponentInterest.QueryConstraint RelativeBox(double xLength, double yLength, double zLength)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                RelativeBoxConstraint = new ComponentInterest.RelativeBoxConstraint
                {
                    EdgeLength = new EdgeLength(xLength, yLength, zLength)
                }
            };
        }
        
        public static ComponentInterest.QueryConstraint EntityId(EntityId entityId)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                EntityIdConstraint = entityId.Id
            };
        }
        
        public static ComponentInterest.QueryConstraint Component<T>() where T : ISpatialComponentData
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                ComponentConstraint = Dynamic.GetComponentId<T>()
            };
        }
        
        public static ComponentInterest.QueryConstraint Component(uint componentId)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(),
                ComponentConstraint = componentId
            };
        }
        
        public static ComponentInterest.QueryConstraint All(IEnumerable<ComponentInterest.QueryConstraint> constraints)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(constraints),
                OrConstraint = new List<ComponentInterest.QueryConstraint>()
            };
        }
        
        public static ComponentInterest.QueryConstraint Any(IEnumerable<ComponentInterest.QueryConstraint> constraints)
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>(constraints)
            };
        }
    }
}
