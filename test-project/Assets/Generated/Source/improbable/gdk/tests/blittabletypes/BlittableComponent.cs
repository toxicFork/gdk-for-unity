// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public const uint ComponentId = 1001;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 1001;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
                return isDataDirty;
            }

            /*
            The propertyIndex argument counts up from 0 in the order defined in your schema component.
            It is not the schema field number itself. For example:
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a corresponds to propertyIndex 0 and val_b corresponds to propertyIndex 1 in this method.
            This method throws an InvalidOperationException in case your component doesn't contain properties.
            */
            public bool IsDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 5)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 4]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        dirtyBits0 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(new SchemaComponentData(1001));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields(), world);

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            private BlittableBool boolField;

            public BlittableBool BoolField
            {
                get => boolField;
                set
                {
                    MarkDataDirty(0);
                    this.boolField = value;
                }
            }

            private int intField;

            public int IntField
            {
                get => intField;
                set
                {
                    MarkDataDirty(1);
                    this.intField = value;
                }
            }

            private long longField;

            public long LongField
            {
                get => longField;
                set
                {
                    MarkDataDirty(2);
                    this.longField = value;
                }
            }

            private float floatField;

            public float FloatField
            {
                get => floatField;
                set
                {
                    MarkDataDirty(3);
                    this.floatField = value;
                }
            }

            private double doubleField;

            public double DoubleField
            {
                get => doubleField;
                set
                {
                    MarkDataDirty(4);
                    this.doubleField = value;
                }
            }
        }

        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 1001;

            public BlittableBool BoolField;
            public int IntField;
            public long LongField;
            public float FloatField;
            public double DoubleField;
        }

        public static class Serialization
        {
            public static void SerializeComponent(Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    obj.AddBool(1, component.BoolField);
                }
                {
                    obj.AddInt32(2, component.IntField);
                }
                {
                    obj.AddInt64(3, component.LongField);
                }
                {
                    obj.AddFloat(4, component.FloatField);
                }
                {
                    obj.AddDouble(5, component.DoubleField);
                }
            }

            public static void SerializeUpdate(Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        obj.AddBool(1, component.BoolField);
                    }

                }
                {
                    if (component.IsDataDirty(1))
                    {
                        obj.AddInt32(2, component.IntField);
                    }

                }
                {
                    if (component.IsDataDirty(2))
                    {
                        obj.AddInt64(3, component.LongField);
                    }

                }
                {
                    if (component.IsDataDirty(3))
                    {
                        obj.AddFloat(4, component.FloatField);
                    }

                }
                {
                    if (component.IsDataDirty(4))
                    {
                        obj.AddDouble(5, component.DoubleField);
                    }

                }
            }

            public static void SerializeSnapshot(Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddBool(1, snapshot.BoolField);
                }
                {
                    obj.AddInt32(2, snapshot.IntField);
                }
                {
                    obj.AddInt64(3, snapshot.LongField);
                }
                {
                    obj.AddFloat(4, snapshot.FloatField);
                }
                {
                    obj.AddDouble(5, snapshot.DoubleField);
                }
            }

            public static Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component();

                {
                    component.BoolField = obj.GetBool(1);
                }
                {
                    component.IntField = obj.GetInt32(2);
                }
                {
                    component.LongField = obj.GetInt64(3);
                }
                {
                    component.FloatField = obj.GetFloat(4);
                }
                {
                    component.DoubleField = obj.GetDouble(5);
                }
                return component;
            }

            public static Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update();
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        update.BoolField = new global::Improbable.Gdk.Core.Option<BlittableBool>(value);
                    }
                    
                }
                {
                    if (obj.GetInt32Count(2) == 1)
                    {
                        var value = obj.GetInt32(2);
                        update.IntField = new global::Improbable.Gdk.Core.Option<int>(value);
                    }
                    
                }
                {
                    if (obj.GetInt64Count(3) == 1)
                    {
                        var value = obj.GetInt64(3);
                        update.LongField = new global::Improbable.Gdk.Core.Option<long>(value);
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        update.FloatField = new global::Improbable.Gdk.Core.Option<float>(value);
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(5) == 1)
                    {
                        var value = obj.GetDouble(5);
                        update.DoubleField = new global::Improbable.Gdk.Core.Option<double>(value);
                    }
                    
                }
                return update;
            }

            public static Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Snapshot();

                {
                    component.BoolField = obj.GetBool(1);
                }

                {
                    component.IntField = obj.GetInt32(2);
                }

                {
                    component.LongField = obj.GetInt64(3);
                }

                {
                    component.FloatField = obj.GetFloat(4);
                }

                {
                    component.DoubleField = obj.GetDouble(5);
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    if (obj.GetBoolCount(1) == 1)
                    {
                        var value = obj.GetBool(1);
                        component.BoolField = value;
                    }
                    
                }
                {
                    if (obj.GetInt32Count(2) == 1)
                    {
                        var value = obj.GetInt32(2);
                        component.IntField = value;
                    }
                    
                }
                {
                    if (obj.GetInt64Count(3) == 1)
                    {
                        var value = obj.GetInt64(3);
                        component.LongField = value;
                    }
                    
                }
                {
                    if (obj.GetFloatCount(4) == 1)
                    {
                        var value = obj.GetFloat(4);
                        component.FloatField = value;
                    }
                    
                }
                {
                    if (obj.GetDoubleCount(5) == 1)
                    {
                        var value = obj.GetDouble(5);
                        component.DoubleField = value;
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<BlittableBool> BoolField;
            public Option<int> IntField;
            public Option<long> LongField;
            public Option<float> FloatField;
            public Option<double> DoubleField;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class BlittableComponentDynamic : IDynamicInvokable
        {
            public uint ComponentId => BlittableComponent.ComponentId;

            private static Component DeserializeData(ComponentData data, World world)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.Deserialize(schemaDataOpt.Value.GetFields(), world);
            }

            private static Update DeserializeUpdate(ComponentUpdate update, World world)
            {
                var schemaDataOpt = update.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentUpdate)}");
                }

                return Serialization.DeserializeUpdate(schemaDataOpt.Value);
            }

            private static Snapshot DeserializeSnapshot(ComponentData snapshot, World world)
            {
                var schemaDataOpt = snapshot.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.DeserializeSnapshot(schemaDataOpt.Value.GetFields(), world);
            }

            private static void SerializeSnapshot(Snapshot snapshot, ComponentData data)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not serialise an empty {nameof(ComponentData)}");
                }

                Serialization.SerializeSnapshot(snapshot, data.SchemaData.Value.GetFields());
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(BlittableComponent.ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(BlittableComponent.ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }
        }
    }
}
