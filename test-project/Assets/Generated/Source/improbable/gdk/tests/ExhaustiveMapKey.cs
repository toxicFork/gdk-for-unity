// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveMapKey
    {
        public const uint ComponentId = 197719;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 197719;

            // Bit masks for tracking which component properties were changed locally and need to be synced.
            // Each byte tracks 8 component properties.
            private byte dirtyBits0;
            private byte dirtyBits1;
            private byte dirtyBits2;

            public bool IsDataDirty()
            {
                var isDataDirty = false;
                isDataDirty |= (dirtyBits0 != 0x0);
                isDataDirty |= (dirtyBits1 != 0x0);
                isDataDirty |= (dirtyBits2 != 0x0);
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
                if (propertyIndex < 0 || propertyIndex >= 18)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 17]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex / 8;
                switch (dirtyBitsByteIndex)
                {
                    case 0:
                        return (dirtyBits0 & (0x1 << propertyIndex % 8)) != 0x0;
                    case 1:
                        return (dirtyBits1 & (0x1 << propertyIndex % 8)) != 0x0;
                    case 2:
                        return (dirtyBits2 & (0x1 << propertyIndex % 8)) != 0x0;
                }

                return false;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 18)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 17]. " +
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
                    case 1:
                        dirtyBits1 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                    case 2:
                        dirtyBits2 |= (byte) (0x1 << propertyIndex % 8);
                        break;
                }
            }

            public void MarkDataClean()
            {
                dirtyBits0 = 0x0;
                dirtyBits1 = 0x0;
                dirtyBits2 = 0x0;
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(new SchemaComponentData(197719));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields(), world);

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint field1Handle;

            public global::System.Collections.Generic.Dictionary<BlittableBool,string> Field1
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field1Provider.Get(field1Handle);
                set
                {
                    MarkDataDirty(0);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field1Provider.Set(field1Handle, value);
                }
            }

            internal uint field2Handle;

            public global::System.Collections.Generic.Dictionary<float,string> Field2
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field2Provider.Get(field2Handle);
                set
                {
                    MarkDataDirty(1);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field2Provider.Set(field2Handle, value);
                }
            }

            internal uint field3Handle;

            public global::System.Collections.Generic.Dictionary<byte[],string> Field3
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field3Provider.Get(field3Handle);
                set
                {
                    MarkDataDirty(2);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field3Provider.Set(field3Handle, value);
                }
            }

            internal uint field4Handle;

            public global::System.Collections.Generic.Dictionary<int,string> Field4
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field4Provider.Get(field4Handle);
                set
                {
                    MarkDataDirty(3);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field4Provider.Set(field4Handle, value);
                }
            }

            internal uint field5Handle;

            public global::System.Collections.Generic.Dictionary<long,string> Field5
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field5Provider.Get(field5Handle);
                set
                {
                    MarkDataDirty(4);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field5Provider.Set(field5Handle, value);
                }
            }

            internal uint field6Handle;

            public global::System.Collections.Generic.Dictionary<double,string> Field6
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field6Provider.Get(field6Handle);
                set
                {
                    MarkDataDirty(5);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field6Provider.Set(field6Handle, value);
                }
            }

            internal uint field7Handle;

            public global::System.Collections.Generic.Dictionary<string,string> Field7
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field7Provider.Get(field7Handle);
                set
                {
                    MarkDataDirty(6);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field7Provider.Set(field7Handle, value);
                }
            }

            internal uint field8Handle;

            public global::System.Collections.Generic.Dictionary<uint,string> Field8
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field8Provider.Get(field8Handle);
                set
                {
                    MarkDataDirty(7);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field8Provider.Set(field8Handle, value);
                }
            }

            internal uint field9Handle;

            public global::System.Collections.Generic.Dictionary<ulong,string> Field9
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field9Provider.Get(field9Handle);
                set
                {
                    MarkDataDirty(8);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field9Provider.Set(field9Handle, value);
                }
            }

            internal uint field10Handle;

            public global::System.Collections.Generic.Dictionary<int,string> Field10
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field10Provider.Get(field10Handle);
                set
                {
                    MarkDataDirty(9);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field10Provider.Set(field10Handle, value);
                }
            }

            internal uint field11Handle;

            public global::System.Collections.Generic.Dictionary<long,string> Field11
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field11Provider.Get(field11Handle);
                set
                {
                    MarkDataDirty(10);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field11Provider.Set(field11Handle, value);
                }
            }

            internal uint field12Handle;

            public global::System.Collections.Generic.Dictionary<uint,string> Field12
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field12Provider.Get(field12Handle);
                set
                {
                    MarkDataDirty(11);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field12Provider.Set(field12Handle, value);
                }
            }

            internal uint field13Handle;

            public global::System.Collections.Generic.Dictionary<ulong,string> Field13
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field13Provider.Get(field13Handle);
                set
                {
                    MarkDataDirty(12);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field13Provider.Set(field13Handle, value);
                }
            }

            internal uint field14Handle;

            public global::System.Collections.Generic.Dictionary<int,string> Field14
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field14Provider.Get(field14Handle);
                set
                {
                    MarkDataDirty(13);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field14Provider.Set(field14Handle, value);
                }
            }

            internal uint field15Handle;

            public global::System.Collections.Generic.Dictionary<long,string> Field15
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field15Provider.Get(field15Handle);
                set
                {
                    MarkDataDirty(14);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field15Provider.Set(field15Handle, value);
                }
            }

            internal uint field16Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string> Field16
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field16Provider.Get(field16Handle);
                set
                {
                    MarkDataDirty(15);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field16Provider.Set(field16Handle, value);
                }
            }

            internal uint field17Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string> Field17
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field17Provider.Get(field17Handle);
                set
                {
                    MarkDataDirty(16);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field17Provider.Set(field17Handle, value);
                }
            }

            internal uint field18Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string> Field18
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field18Provider.Get(field18Handle);
                set
                {
                    MarkDataDirty(17);
                    Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field18Provider.Set(field18Handle, value);
                }
            }
        }

        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 197719;

            public global::System.Collections.Generic.Dictionary<BlittableBool,string> Field1;
            public global::System.Collections.Generic.Dictionary<float,string> Field2;
            public global::System.Collections.Generic.Dictionary<byte[],string> Field3;
            public global::System.Collections.Generic.Dictionary<int,string> Field4;
            public global::System.Collections.Generic.Dictionary<long,string> Field5;
            public global::System.Collections.Generic.Dictionary<double,string> Field6;
            public global::System.Collections.Generic.Dictionary<string,string> Field7;
            public global::System.Collections.Generic.Dictionary<uint,string> Field8;
            public global::System.Collections.Generic.Dictionary<ulong,string> Field9;
            public global::System.Collections.Generic.Dictionary<int,string> Field10;
            public global::System.Collections.Generic.Dictionary<long,string> Field11;
            public global::System.Collections.Generic.Dictionary<uint,string> Field12;
            public global::System.Collections.Generic.Dictionary<ulong,string> Field13;
            public global::System.Collections.Generic.Dictionary<int,string> Field14;
            public global::System.Collections.Generic.Dictionary<long,string> Field15;
            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string> Field16;
            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string> Field17;
            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string> Field18;
        }

        public static class Serialization
        {
            public static void SerializeComponent(Improbable.Gdk.Tests.ExhaustiveMapKey.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    foreach (var keyValuePair in component.Field1)
                    {
                        var mapObj = obj.AddObject(1);
                        mapObj.AddBool(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field2)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddFloat(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field3)
                    {
                        var mapObj = obj.AddObject(3);
                        mapObj.AddBytes(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddInt64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field6)
                    {
                        var mapObj = obj.AddObject(6);
                        mapObj.AddDouble(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field7)
                    {
                        var mapObj = obj.AddObject(7);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field8)
                    {
                        var mapObj = obj.AddObject(8);
                        mapObj.AddUint32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field9)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddUint64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field10)
                    {
                        var mapObj = obj.AddObject(10);
                        mapObj.AddSint32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field11)
                    {
                        var mapObj = obj.AddObject(11);
                        mapObj.AddSint64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field12)
                    {
                        var mapObj = obj.AddObject(12);
                        mapObj.AddFixed32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field13)
                    {
                        var mapObj = obj.AddObject(13);
                        mapObj.AddFixed64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field14)
                    {
                        var mapObj = obj.AddObject(14);
                        mapObj.AddSfixed32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field15)
                    {
                        var mapObj = obj.AddObject(15);
                        mapObj.AddSfixed64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field16)
                    {
                        var mapObj = obj.AddObject(16);
                        mapObj.AddEntityId(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field17)
                    {
                        var mapObj = obj.AddObject(17);
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field18)
                    {
                        var mapObj = obj.AddObject(18);
                        mapObj.AddEnum(1, (uint) keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }
                    
                }
            }

            public static void SerializeUpdate(Improbable.Gdk.Tests.ExhaustiveMapKey.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        foreach (var keyValuePair in component.Field1)
                        {
                            var mapObj = obj.AddObject(1);
                            mapObj.AddBool(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field1.Count == 0)
                        {
                            updateObj.AddClearedField(1);
                        }
                        
                }
                {
                    if (component.IsDataDirty(1))
                    {
                        foreach (var keyValuePair in component.Field2)
                        {
                            var mapObj = obj.AddObject(2);
                            mapObj.AddFloat(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field2.Count == 0)
                        {
                            updateObj.AddClearedField(2);
                        }
                        
                }
                {
                    if (component.IsDataDirty(2))
                    {
                        foreach (var keyValuePair in component.Field3)
                        {
                            var mapObj = obj.AddObject(3);
                            mapObj.AddBytes(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field3.Count == 0)
                        {
                            updateObj.AddClearedField(3);
                        }
                        
                }
                {
                    if (component.IsDataDirty(3))
                    {
                        foreach (var keyValuePair in component.Field4)
                        {
                            var mapObj = obj.AddObject(4);
                            mapObj.AddInt32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field4.Count == 0)
                        {
                            updateObj.AddClearedField(4);
                        }
                        
                }
                {
                    if (component.IsDataDirty(4))
                    {
                        foreach (var keyValuePair in component.Field5)
                        {
                            var mapObj = obj.AddObject(5);
                            mapObj.AddInt64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field5.Count == 0)
                        {
                            updateObj.AddClearedField(5);
                        }
                        
                }
                {
                    if (component.IsDataDirty(5))
                    {
                        foreach (var keyValuePair in component.Field6)
                        {
                            var mapObj = obj.AddObject(6);
                            mapObj.AddDouble(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field6.Count == 0)
                        {
                            updateObj.AddClearedField(6);
                        }
                        
                }
                {
                    if (component.IsDataDirty(6))
                    {
                        foreach (var keyValuePair in component.Field7)
                        {
                            var mapObj = obj.AddObject(7);
                            mapObj.AddString(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field7.Count == 0)
                        {
                            updateObj.AddClearedField(7);
                        }
                        
                }
                {
                    if (component.IsDataDirty(7))
                    {
                        foreach (var keyValuePair in component.Field8)
                        {
                            var mapObj = obj.AddObject(8);
                            mapObj.AddUint32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field8.Count == 0)
                        {
                            updateObj.AddClearedField(8);
                        }
                        
                }
                {
                    if (component.IsDataDirty(8))
                    {
                        foreach (var keyValuePair in component.Field9)
                        {
                            var mapObj = obj.AddObject(9);
                            mapObj.AddUint64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field9.Count == 0)
                        {
                            updateObj.AddClearedField(9);
                        }
                        
                }
                {
                    if (component.IsDataDirty(9))
                    {
                        foreach (var keyValuePair in component.Field10)
                        {
                            var mapObj = obj.AddObject(10);
                            mapObj.AddSint32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field10.Count == 0)
                        {
                            updateObj.AddClearedField(10);
                        }
                        
                }
                {
                    if (component.IsDataDirty(10))
                    {
                        foreach (var keyValuePair in component.Field11)
                        {
                            var mapObj = obj.AddObject(11);
                            mapObj.AddSint64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field11.Count == 0)
                        {
                            updateObj.AddClearedField(11);
                        }
                        
                }
                {
                    if (component.IsDataDirty(11))
                    {
                        foreach (var keyValuePair in component.Field12)
                        {
                            var mapObj = obj.AddObject(12);
                            mapObj.AddFixed32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field12.Count == 0)
                        {
                            updateObj.AddClearedField(12);
                        }
                        
                }
                {
                    if (component.IsDataDirty(12))
                    {
                        foreach (var keyValuePair in component.Field13)
                        {
                            var mapObj = obj.AddObject(13);
                            mapObj.AddFixed64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field13.Count == 0)
                        {
                            updateObj.AddClearedField(13);
                        }
                        
                }
                {
                    if (component.IsDataDirty(13))
                    {
                        foreach (var keyValuePair in component.Field14)
                        {
                            var mapObj = obj.AddObject(14);
                            mapObj.AddSfixed32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field14.Count == 0)
                        {
                            updateObj.AddClearedField(14);
                        }
                        
                }
                {
                    if (component.IsDataDirty(14))
                    {
                        foreach (var keyValuePair in component.Field15)
                        {
                            var mapObj = obj.AddObject(15);
                            mapObj.AddSfixed64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field15.Count == 0)
                        {
                            updateObj.AddClearedField(15);
                        }
                        
                }
                {
                    if (component.IsDataDirty(15))
                    {
                        foreach (var keyValuePair in component.Field16)
                        {
                            var mapObj = obj.AddObject(16);
                            mapObj.AddEntityId(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field16.Count == 0)
                        {
                            updateObj.AddClearedField(16);
                        }
                        
                }
                {
                    if (component.IsDataDirty(16))
                    {
                        foreach (var keyValuePair in component.Field17)
                        {
                            var mapObj = obj.AddObject(17);
                            global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field17.Count == 0)
                        {
                            updateObj.AddClearedField(17);
                        }
                        
                }
                {
                    if (component.IsDataDirty(17))
                    {
                        foreach (var keyValuePair in component.Field18)
                        {
                            var mapObj = obj.AddObject(18);
                            mapObj.AddEnum(1, (uint) keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }
                        
                    }

                    if (component.Field18.Count == 0)
                        {
                            updateObj.AddClearedField(18);
                        }
                        
                }
            }

            public static void SerializeSnapshot(Improbable.Gdk.Tests.ExhaustiveMapKey.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var keyValuePair in snapshot.Field1)
                {
                    var mapObj = obj.AddObject(1);
                    mapObj.AddBool(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field2)
                {
                    var mapObj = obj.AddObject(2);
                    mapObj.AddFloat(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field3)
                {
                    var mapObj = obj.AddObject(3);
                    mapObj.AddBytes(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddInt32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddInt64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field6)
                {
                    var mapObj = obj.AddObject(6);
                    mapObj.AddDouble(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field7)
                {
                    var mapObj = obj.AddObject(7);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field8)
                {
                    var mapObj = obj.AddObject(8);
                    mapObj.AddUint32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field9)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddUint64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field10)
                {
                    var mapObj = obj.AddObject(10);
                    mapObj.AddSint32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field11)
                {
                    var mapObj = obj.AddObject(11);
                    mapObj.AddSint64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field12)
                {
                    var mapObj = obj.AddObject(12);
                    mapObj.AddFixed32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field13)
                {
                    var mapObj = obj.AddObject(13);
                    mapObj.AddFixed64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field14)
                {
                    var mapObj = obj.AddObject(14);
                    mapObj.AddSfixed32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field15)
                {
                    var mapObj = obj.AddObject(15);
                    mapObj.AddSfixed64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field16)
                {
                    var mapObj = obj.AddObject(16);
                    mapObj.AddEntityId(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field17)
                {
                    var mapObj = obj.AddObject(17);
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in snapshot.Field18)
                {
                    var mapObj = obj.AddObject(18);
                    mapObj.AddEnum(1, (uint) keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
            }

            public static Improbable.Gdk.Tests.ExhaustiveMapKey.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ExhaustiveMapKey.Component();

                component.field1Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field1Provider.Allocate(world);
                {
                    component.Field1 = new global::System.Collections.Generic.Dictionary<BlittableBool,string>();
                    var map = component.Field1;
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field2Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field2Provider.Allocate(world);
                {
                    component.Field2 = new global::System.Collections.Generic.Dictionary<float,string>();
                    var map = component.Field2;
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field3Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field3Provider.Allocate(world);
                {
                    component.Field3 = new global::System.Collections.Generic.Dictionary<byte[],string>();
                    var map = component.Field3;
                    var mapSize = obj.GetObjectCount(3);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field4Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field4Provider.Allocate(world);
                {
                    component.Field4 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.Field4;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field5Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field5Provider.Allocate(world);
                {
                    component.Field5 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = component.Field5;
                    var mapSize = obj.GetObjectCount(5);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field6Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field6Provider.Allocate(world);
                {
                    component.Field6 = new global::System.Collections.Generic.Dictionary<double,string>();
                    var map = component.Field6;
                    var mapSize = obj.GetObjectCount(6);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field7Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field7Provider.Allocate(world);
                {
                    component.Field7 = new global::System.Collections.Generic.Dictionary<string,string>();
                    var map = component.Field7;
                    var mapSize = obj.GetObjectCount(7);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field8Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field8Provider.Allocate(world);
                {
                    component.Field8 = new global::System.Collections.Generic.Dictionary<uint,string>();
                    var map = component.Field8;
                    var mapSize = obj.GetObjectCount(8);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field9Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field9Provider.Allocate(world);
                {
                    component.Field9 = new global::System.Collections.Generic.Dictionary<ulong,string>();
                    var map = component.Field9;
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field10Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field10Provider.Allocate(world);
                {
                    component.Field10 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.Field10;
                    var mapSize = obj.GetObjectCount(10);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field11Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field11Provider.Allocate(world);
                {
                    component.Field11 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = component.Field11;
                    var mapSize = obj.GetObjectCount(11);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field12Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field12Provider.Allocate(world);
                {
                    component.Field12 = new global::System.Collections.Generic.Dictionary<uint,string>();
                    var map = component.Field12;
                    var mapSize = obj.GetObjectCount(12);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field13Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field13Provider.Allocate(world);
                {
                    component.Field13 = new global::System.Collections.Generic.Dictionary<ulong,string>();
                    var map = component.Field13;
                    var mapSize = obj.GetObjectCount(13);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field14Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field14Provider.Allocate(world);
                {
                    component.Field14 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.Field14;
                    var mapSize = obj.GetObjectCount(14);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field15Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field15Provider.Allocate(world);
                {
                    component.Field15 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = component.Field15;
                    var mapSize = obj.GetObjectCount(15);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field16Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field16Provider.Allocate(world);
                {
                    component.Field16 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>();
                    var map = component.Field16;
                    var mapSize = obj.GetObjectCount(16);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field17Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field17Provider.Allocate(world);
                {
                    component.Field17 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>();
                    var map = component.Field17;
                    var mapSize = obj.GetObjectCount(17);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field18Handle = Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.Field18Provider.Allocate(world);
                {
                    component.Field18 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>();
                    var map = component.Field18;
                    var mapSize = obj.GetObjectCount(18);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.Gdk.Tests.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                return component;
            }

            public static Improbable.Gdk.Tests.ExhaustiveMapKey.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ExhaustiveMapKey.Update();
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field1 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<BlittableBool,string>>(new global::System.Collections.Generic.Dictionary<BlittableBool,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        update.Field1.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<float,string>>(new global::System.Collections.Generic.Dictionary<float,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        update.Field2.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field3 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<byte[],string>>(new global::System.Collections.Generic.Dictionary<byte[],string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        update.Field3.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(4);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int,string>>(new global::System.Collections.Generic.Dictionary<int,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        update.Field4.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(5);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<long,string>>(new global::System.Collections.Generic.Dictionary<long,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        update.Field5.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(6);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 6;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field6 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<double,string>>(new global::System.Collections.Generic.Dictionary<double,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        update.Field6.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(7);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field7 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,string>>(new global::System.Collections.Generic.Dictionary<string,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        update.Field7.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field8 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,string>>(new global::System.Collections.Generic.Dictionary<uint,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        update.Field8.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field9 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<ulong,string>>(new global::System.Collections.Generic.Dictionary<ulong,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        update.Field9.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(10);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 10;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field10 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int,string>>(new global::System.Collections.Generic.Dictionary<int,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        update.Field10.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(11);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 11;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field11 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<long,string>>(new global::System.Collections.Generic.Dictionary<long,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        update.Field11.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(12);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 12;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field12 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint,string>>(new global::System.Collections.Generic.Dictionary<uint,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        update.Field12.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(13);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 13;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field13 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<ulong,string>>(new global::System.Collections.Generic.Dictionary<ulong,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        update.Field13.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(14);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 14;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field14 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int,string>>(new global::System.Collections.Generic.Dictionary<int,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        update.Field14.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(15);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 15;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field15 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<long,string>>(new global::System.Collections.Generic.Dictionary<long,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        update.Field15.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(16);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 16;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        update.Field16.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(17);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 17;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        update.Field17.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(18);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 18;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field18 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.Gdk.Tests.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        update.Field18.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static Improbable.Gdk.Tests.ExhaustiveMapKey.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ExhaustiveMapKey.Snapshot();

                {
                    component.Field1 = new global::System.Collections.Generic.Dictionary<BlittableBool,string>();
                    var map = component.Field1;
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field2 = new global::System.Collections.Generic.Dictionary<float,string>();
                    var map = component.Field2;
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field3 = new global::System.Collections.Generic.Dictionary<byte[],string>();
                    var map = component.Field3;
                    var mapSize = obj.GetObjectCount(3);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field4 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.Field4;
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field5 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = component.Field5;
                    var mapSize = obj.GetObjectCount(5);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field6 = new global::System.Collections.Generic.Dictionary<double,string>();
                    var map = component.Field6;
                    var mapSize = obj.GetObjectCount(6);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field7 = new global::System.Collections.Generic.Dictionary<string,string>();
                    var map = component.Field7;
                    var mapSize = obj.GetObjectCount(7);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field8 = new global::System.Collections.Generic.Dictionary<uint,string>();
                    var map = component.Field8;
                    var mapSize = obj.GetObjectCount(8);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field9 = new global::System.Collections.Generic.Dictionary<ulong,string>();
                    var map = component.Field9;
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field10 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.Field10;
                    var mapSize = obj.GetObjectCount(10);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field11 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = component.Field11;
                    var mapSize = obj.GetObjectCount(11);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field12 = new global::System.Collections.Generic.Dictionary<uint,string>();
                    var map = component.Field12;
                    var mapSize = obj.GetObjectCount(12);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field13 = new global::System.Collections.Generic.Dictionary<ulong,string>();
                    var map = component.Field13;
                    var mapSize = obj.GetObjectCount(13);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field14 = new global::System.Collections.Generic.Dictionary<int,string>();
                    var map = component.Field14;
                    var mapSize = obj.GetObjectCount(14);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field15 = new global::System.Collections.Generic.Dictionary<long,string>();
                    var map = component.Field15;
                    var mapSize = obj.GetObjectCount(15);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field16 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>();
                    var map = component.Field16;
                    var mapSize = obj.GetObjectCount(16);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field17 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>();
                    var map = component.Field17;
                    var mapSize = obj.GetObjectCount(17);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                {
                    component.Field18 = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>();
                    var map = component.Field18;
                    var mapSize = obj.GetObjectCount(18);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.Gdk.Tests.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ExhaustiveMapKey.Component component)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field1.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        component.Field1.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field2.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        component.Field2.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field3.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        component.Field3.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(4);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field4.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        component.Field4.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(5);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field5.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        component.Field5.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(6);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 6;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field6.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        component.Field6.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(7);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field7.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        component.Field7.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field8.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        component.Field8.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field9.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        component.Field9.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(10);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 10;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field10.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        component.Field10.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(11);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 11;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field11.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        component.Field11.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(12);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 12;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field12.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        component.Field12.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(13);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 13;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field13.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        component.Field13.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(14);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 14;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field14.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        component.Field14.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(15);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 15;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field15.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        component.Field15.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(16);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 16;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field16.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        component.Field16.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(17);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 17;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field17.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        component.Field17.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(18);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 18;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field18.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.Gdk.Tests.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        component.Field18.Add(key, value);
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::System.Collections.Generic.Dictionary<BlittableBool,string>> Field1;
            public Option<global::System.Collections.Generic.Dictionary<float,string>> Field2;
            public Option<global::System.Collections.Generic.Dictionary<byte[],string>> Field3;
            public Option<global::System.Collections.Generic.Dictionary<int,string>> Field4;
            public Option<global::System.Collections.Generic.Dictionary<long,string>> Field5;
            public Option<global::System.Collections.Generic.Dictionary<double,string>> Field6;
            public Option<global::System.Collections.Generic.Dictionary<string,string>> Field7;
            public Option<global::System.Collections.Generic.Dictionary<uint,string>> Field8;
            public Option<global::System.Collections.Generic.Dictionary<ulong,string>> Field9;
            public Option<global::System.Collections.Generic.Dictionary<int,string>> Field10;
            public Option<global::System.Collections.Generic.Dictionary<long,string>> Field11;
            public Option<global::System.Collections.Generic.Dictionary<uint,string>> Field12;
            public Option<global::System.Collections.Generic.Dictionary<ulong,string>> Field13;
            public Option<global::System.Collections.Generic.Dictionary<int,string>> Field14;
            public Option<global::System.Collections.Generic.Dictionary<long,string>> Field15;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId,string>> Field16;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeType,string>> Field17;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Tests.SomeEnum,string>> Field18;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapKey.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ExhaustiveMapKeyDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveMapKey.ComponentId;

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
                handler.Accept<Component, Update>(ExhaustiveMapKey.ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(ExhaustiveMapKey.ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }
        }
    }
}
