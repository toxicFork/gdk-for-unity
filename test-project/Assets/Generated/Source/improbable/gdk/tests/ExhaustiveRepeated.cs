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
    public partial class ExhaustiveRepeated
    {
        public const uint ComponentId = 197717;

        public struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            public uint ComponentId => 197717;

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
                var componentDataSchema = new ComponentData(new SchemaComponentData(197717));
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields(), world);

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }

            internal uint field1Handle;

            public global::System.Collections.Generic.List<BlittableBool> Field1
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field1Provider.Get(field1Handle);
                set
                {
                    MarkDataDirty(0);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field1Provider.Set(field1Handle, value);
                }
            }

            internal uint field2Handle;

            public global::System.Collections.Generic.List<float> Field2
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field2Provider.Get(field2Handle);
                set
                {
                    MarkDataDirty(1);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field2Provider.Set(field2Handle, value);
                }
            }

            internal uint field3Handle;

            public global::System.Collections.Generic.List<byte[]> Field3
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field3Provider.Get(field3Handle);
                set
                {
                    MarkDataDirty(2);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field3Provider.Set(field3Handle, value);
                }
            }

            internal uint field4Handle;

            public global::System.Collections.Generic.List<int> Field4
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field4Provider.Get(field4Handle);
                set
                {
                    MarkDataDirty(3);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field4Provider.Set(field4Handle, value);
                }
            }

            internal uint field5Handle;

            public global::System.Collections.Generic.List<long> Field5
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field5Provider.Get(field5Handle);
                set
                {
                    MarkDataDirty(4);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field5Provider.Set(field5Handle, value);
                }
            }

            internal uint field6Handle;

            public global::System.Collections.Generic.List<double> Field6
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field6Provider.Get(field6Handle);
                set
                {
                    MarkDataDirty(5);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field6Provider.Set(field6Handle, value);
                }
            }

            internal uint field7Handle;

            public global::System.Collections.Generic.List<string> Field7
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field7Provider.Get(field7Handle);
                set
                {
                    MarkDataDirty(6);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field7Provider.Set(field7Handle, value);
                }
            }

            internal uint field8Handle;

            public global::System.Collections.Generic.List<uint> Field8
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field8Provider.Get(field8Handle);
                set
                {
                    MarkDataDirty(7);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field8Provider.Set(field8Handle, value);
                }
            }

            internal uint field9Handle;

            public global::System.Collections.Generic.List<ulong> Field9
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field9Provider.Get(field9Handle);
                set
                {
                    MarkDataDirty(8);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field9Provider.Set(field9Handle, value);
                }
            }

            internal uint field10Handle;

            public global::System.Collections.Generic.List<int> Field10
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field10Provider.Get(field10Handle);
                set
                {
                    MarkDataDirty(9);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field10Provider.Set(field10Handle, value);
                }
            }

            internal uint field11Handle;

            public global::System.Collections.Generic.List<long> Field11
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field11Provider.Get(field11Handle);
                set
                {
                    MarkDataDirty(10);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field11Provider.Set(field11Handle, value);
                }
            }

            internal uint field12Handle;

            public global::System.Collections.Generic.List<uint> Field12
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field12Provider.Get(field12Handle);
                set
                {
                    MarkDataDirty(11);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field12Provider.Set(field12Handle, value);
                }
            }

            internal uint field13Handle;

            public global::System.Collections.Generic.List<ulong> Field13
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field13Provider.Get(field13Handle);
                set
                {
                    MarkDataDirty(12);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field13Provider.Set(field13Handle, value);
                }
            }

            internal uint field14Handle;

            public global::System.Collections.Generic.List<int> Field14
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field14Provider.Get(field14Handle);
                set
                {
                    MarkDataDirty(13);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field14Provider.Set(field14Handle, value);
                }
            }

            internal uint field15Handle;

            public global::System.Collections.Generic.List<long> Field15
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field15Provider.Get(field15Handle);
                set
                {
                    MarkDataDirty(14);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field15Provider.Set(field15Handle, value);
                }
            }

            internal uint field16Handle;

            public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId> Field16
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field16Provider.Get(field16Handle);
                set
                {
                    MarkDataDirty(15);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field16Provider.Set(field16Handle, value);
                }
            }

            internal uint field17Handle;

            public global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType> Field17
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field17Provider.Get(field17Handle);
                set
                {
                    MarkDataDirty(16);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field17Provider.Set(field17Handle, value);
                }
            }

            internal uint field18Handle;

            public global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum> Field18
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field18Provider.Get(field18Handle);
                set
                {
                    MarkDataDirty(17);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field18Provider.Set(field18Handle, value);
                }
            }
        }

        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 197717;

            public global::System.Collections.Generic.List<BlittableBool> Field1;
            public global::System.Collections.Generic.List<float> Field2;
            public global::System.Collections.Generic.List<byte[]> Field3;
            public global::System.Collections.Generic.List<int> Field4;
            public global::System.Collections.Generic.List<long> Field5;
            public global::System.Collections.Generic.List<double> Field6;
            public global::System.Collections.Generic.List<string> Field7;
            public global::System.Collections.Generic.List<uint> Field8;
            public global::System.Collections.Generic.List<ulong> Field9;
            public global::System.Collections.Generic.List<int> Field10;
            public global::System.Collections.Generic.List<long> Field11;
            public global::System.Collections.Generic.List<uint> Field12;
            public global::System.Collections.Generic.List<ulong> Field13;
            public global::System.Collections.Generic.List<int> Field14;
            public global::System.Collections.Generic.List<long> Field15;
            public global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId> Field16;
            public global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType> Field17;
            public global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum> Field18;
        }

        public static class Serialization
        {
            public static void SerializeComponent(Improbable.Gdk.Tests.ExhaustiveRepeated.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                {
                    foreach (var value in component.Field1)
                    {
                        obj.AddBool(1, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field2)
                    {
                        obj.AddFloat(2, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field3)
                    {
                        obj.AddBytes(3, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field4)
                    {
                        obj.AddInt32(4, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field5)
                    {
                        obj.AddInt64(5, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field6)
                    {
                        obj.AddDouble(6, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field7)
                    {
                        obj.AddString(7, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field8)
                    {
                        obj.AddUint32(8, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field9)
                    {
                        obj.AddUint64(9, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field10)
                    {
                        obj.AddSint32(10, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field11)
                    {
                        obj.AddSint64(11, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field12)
                    {
                        obj.AddFixed32(12, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field13)
                    {
                        obj.AddFixed64(13, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field14)
                    {
                        obj.AddSfixed32(14, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field15)
                    {
                        obj.AddSfixed64(15, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field16)
                    {
                        obj.AddEntityId(16, value);
                    }
                    
                }
                {
                    foreach (var value in component.Field17)
                    {
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(value, obj.AddObject(17));
                    }
                    
                }
                {
                    foreach (var value in component.Field18)
                    {
                        obj.AddEnum(18, (uint) value);
                    }
                    
                }
            }

            public static void SerializeUpdate(Improbable.Gdk.Tests.ExhaustiveRepeated.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    if (component.IsDataDirty(0))
                    {
                        foreach (var value in component.Field1)
                        {
                            obj.AddBool(1, value);
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
                        foreach (var value in component.Field2)
                        {
                            obj.AddFloat(2, value);
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
                        foreach (var value in component.Field3)
                        {
                            obj.AddBytes(3, value);
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
                        foreach (var value in component.Field4)
                        {
                            obj.AddInt32(4, value);
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
                        foreach (var value in component.Field5)
                        {
                            obj.AddInt64(5, value);
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
                        foreach (var value in component.Field6)
                        {
                            obj.AddDouble(6, value);
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
                        foreach (var value in component.Field7)
                        {
                            obj.AddString(7, value);
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
                        foreach (var value in component.Field8)
                        {
                            obj.AddUint32(8, value);
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
                        foreach (var value in component.Field9)
                        {
                            obj.AddUint64(9, value);
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
                        foreach (var value in component.Field10)
                        {
                            obj.AddSint32(10, value);
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
                        foreach (var value in component.Field11)
                        {
                            obj.AddSint64(11, value);
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
                        foreach (var value in component.Field12)
                        {
                            obj.AddFixed32(12, value);
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
                        foreach (var value in component.Field13)
                        {
                            obj.AddFixed64(13, value);
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
                        foreach (var value in component.Field14)
                        {
                            obj.AddSfixed32(14, value);
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
                        foreach (var value in component.Field15)
                        {
                            obj.AddSfixed64(15, value);
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
                        foreach (var value in component.Field16)
                        {
                            obj.AddEntityId(16, value);
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
                        foreach (var value in component.Field17)
                        {
                            global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(value, obj.AddObject(17));
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
                        foreach (var value in component.Field18)
                        {
                            obj.AddEnum(18, (uint) value);
                        }
                        
                    }

                    if (component.Field18.Count == 0)
                        {
                            updateObj.AddClearedField(18);
                        }
                        
                }
            }

            public static void SerializeSnapshot(Improbable.Gdk.Tests.ExhaustiveRepeated.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    foreach (var value in snapshot.Field1)
                {
                    obj.AddBool(1, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field2)
                {
                    obj.AddFloat(2, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field3)
                {
                    obj.AddBytes(3, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field4)
                {
                    obj.AddInt32(4, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field5)
                {
                    obj.AddInt64(5, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field6)
                {
                    obj.AddDouble(6, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field7)
                {
                    obj.AddString(7, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field8)
                {
                    obj.AddUint32(8, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field9)
                {
                    obj.AddUint64(9, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field10)
                {
                    obj.AddSint32(10, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field11)
                {
                    obj.AddSint64(11, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field12)
                {
                    obj.AddFixed32(12, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field13)
                {
                    obj.AddFixed64(13, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field14)
                {
                    obj.AddSfixed32(14, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field15)
                {
                    obj.AddSfixed64(15, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field16)
                {
                    obj.AddEntityId(16, value);
                }
                
                }
                {
                    foreach (var value in snapshot.Field17)
                {
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(value, obj.AddObject(17));
                }
                
                }
                {
                    foreach (var value in snapshot.Field18)
                {
                    obj.AddEnum(18, (uint) value);
                }
                
                }
            }

            public static Improbable.Gdk.Tests.ExhaustiveRepeated.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ExhaustiveRepeated.Component();

                component.field1Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field1Provider.Allocate(world);
                {
                    component.Field1 = new global::System.Collections.Generic.List<BlittableBool>();
                    var list = component.Field1;
                    var listLength = obj.GetBoolCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexBool(1, (uint) i));
                    }
                    
                }
                component.field2Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field2Provider.Allocate(world);
                {
                    component.Field2 = new global::System.Collections.Generic.List<float>();
                    var list = component.Field2;
                    var listLength = obj.GetFloatCount(2);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFloat(2, (uint) i));
                    }
                    
                }
                component.field3Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field3Provider.Allocate(world);
                {
                    component.Field3 = new global::System.Collections.Generic.List<byte[]>();
                    var list = component.Field3;
                    var listLength = obj.GetBytesCount(3);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexBytes(3, (uint) i));
                    }
                    
                }
                component.field4Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field4Provider.Allocate(world);
                {
                    component.Field4 = new global::System.Collections.Generic.List<int>();
                    var list = component.Field4;
                    var listLength = obj.GetInt32Count(4);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt32(4, (uint) i));
                    }
                    
                }
                component.field5Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field5Provider.Allocate(world);
                {
                    component.Field5 = new global::System.Collections.Generic.List<long>();
                    var list = component.Field5;
                    var listLength = obj.GetInt64Count(5);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt64(5, (uint) i));
                    }
                    
                }
                component.field6Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field6Provider.Allocate(world);
                {
                    component.Field6 = new global::System.Collections.Generic.List<double>();
                    var list = component.Field6;
                    var listLength = obj.GetDoubleCount(6);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexDouble(6, (uint) i));
                    }
                    
                }
                component.field7Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field7Provider.Allocate(world);
                {
                    component.Field7 = new global::System.Collections.Generic.List<string>();
                    var list = component.Field7;
                    var listLength = obj.GetStringCount(7);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexString(7, (uint) i));
                    }
                    
                }
                component.field8Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field8Provider.Allocate(world);
                {
                    component.Field8 = new global::System.Collections.Generic.List<uint>();
                    var list = component.Field8;
                    var listLength = obj.GetUint32Count(8);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexUint32(8, (uint) i));
                    }
                    
                }
                component.field9Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field9Provider.Allocate(world);
                {
                    component.Field9 = new global::System.Collections.Generic.List<ulong>();
                    var list = component.Field9;
                    var listLength = obj.GetUint64Count(9);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexUint64(9, (uint) i));
                    }
                    
                }
                component.field10Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field10Provider.Allocate(world);
                {
                    component.Field10 = new global::System.Collections.Generic.List<int>();
                    var list = component.Field10;
                    var listLength = obj.GetSint32Count(10);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSint32(10, (uint) i));
                    }
                    
                }
                component.field11Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field11Provider.Allocate(world);
                {
                    component.Field11 = new global::System.Collections.Generic.List<long>();
                    var list = component.Field11;
                    var listLength = obj.GetSint64Count(11);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSint64(11, (uint) i));
                    }
                    
                }
                component.field12Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field12Provider.Allocate(world);
                {
                    component.Field12 = new global::System.Collections.Generic.List<uint>();
                    var list = component.Field12;
                    var listLength = obj.GetFixed32Count(12);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFixed32(12, (uint) i));
                    }
                    
                }
                component.field13Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field13Provider.Allocate(world);
                {
                    component.Field13 = new global::System.Collections.Generic.List<ulong>();
                    var list = component.Field13;
                    var listLength = obj.GetFixed64Count(13);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFixed64(13, (uint) i));
                    }
                    
                }
                component.field14Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field14Provider.Allocate(world);
                {
                    component.Field14 = new global::System.Collections.Generic.List<int>();
                    var list = component.Field14;
                    var listLength = obj.GetSfixed32Count(14);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSfixed32(14, (uint) i));
                    }
                    
                }
                component.field15Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field15Provider.Allocate(world);
                {
                    component.Field15 = new global::System.Collections.Generic.List<long>();
                    var list = component.Field15;
                    var listLength = obj.GetSfixed64Count(15);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSfixed64(15, (uint) i));
                    }
                    
                }
                component.field16Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field16Provider.Allocate(world);
                {
                    component.Field16 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId>();
                    var list = component.Field16;
                    var listLength = obj.GetEntityIdCount(16);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexEntityIdStruct(16, (uint) i));
                    }
                    
                }
                component.field17Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field17Provider.Allocate(world);
                {
                    component.Field17 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType>();
                    var list = component.Field17;
                    var listLength = obj.GetObjectCount(17);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.IndexObject(17, (uint) i)));
                    }
                    
                }
                component.field18Handle = Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.Field18Provider.Allocate(world);
                {
                    component.Field18 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum>();
                    var list = component.Field18;
                    var listLength = obj.GetEnumCount(18);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add((global::Improbable.Gdk.Tests.SomeEnum) obj.IndexEnum(18, (uint) i));
                    }
                    
                }
                return component;
            }

            public static Improbable.Gdk.Tests.ExhaustiveRepeated.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ExhaustiveRepeated.Update();
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var listSize = obj.GetBoolCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field1 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<BlittableBool>>(new global::System.Collections.Generic.List<BlittableBool>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexBool(1, (uint) i);
                        update.Field1.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetFloatCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<float>>(new global::System.Collections.Generic.List<float>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexFloat(2, (uint) i);
                        update.Field2.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetBytesCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field3 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<byte[]>>(new global::System.Collections.Generic.List<byte[]>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexBytes(3, (uint) i);
                        update.Field3.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetInt32Count(4);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<int>>(new global::System.Collections.Generic.List<int>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(4, (uint) i);
                        update.Field4.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetInt64Count(5);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<long>>(new global::System.Collections.Generic.List<long>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt64(5, (uint) i);
                        update.Field5.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetDoubleCount(6);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 6;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field6 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<double>>(new global::System.Collections.Generic.List<double>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexDouble(6, (uint) i);
                        update.Field6.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetStringCount(7);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field7 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<string>>(new global::System.Collections.Generic.List<string>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexString(7, (uint) i);
                        update.Field7.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetUint32Count(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field8 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<uint>>(new global::System.Collections.Generic.List<uint>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexUint32(8, (uint) i);
                        update.Field8.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetUint64Count(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field9 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<ulong>>(new global::System.Collections.Generic.List<ulong>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexUint64(9, (uint) i);
                        update.Field9.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSint32Count(10);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 10;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field10 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<int>>(new global::System.Collections.Generic.List<int>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSint32(10, (uint) i);
                        update.Field10.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSint64Count(11);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 11;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field11 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<long>>(new global::System.Collections.Generic.List<long>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSint64(11, (uint) i);
                        update.Field11.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetFixed32Count(12);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 12;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field12 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<uint>>(new global::System.Collections.Generic.List<uint>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexFixed32(12, (uint) i);
                        update.Field12.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetFixed64Count(13);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 13;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field13 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<ulong>>(new global::System.Collections.Generic.List<ulong>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexFixed64(13, (uint) i);
                        update.Field13.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSfixed32Count(14);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 14;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field14 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<int>>(new global::System.Collections.Generic.List<int>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSfixed32(14, (uint) i);
                        update.Field14.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSfixed64Count(15);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 15;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field15 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<long>>(new global::System.Collections.Generic.List<long>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSfixed64(15, (uint) i);
                        update.Field15.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetEntityIdCount(16);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 16;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId>>(new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexEntityIdStruct(16, (uint) i);
                        update.Field16.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetObjectCount(17);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 17;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType>>(new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.IndexObject(17, (uint) i));
                        update.Field17.Value.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetEnumCount(18);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 18;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        update.Field18 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum>>(new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum>());
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = (global::Improbable.Gdk.Tests.SomeEnum) obj.IndexEnum(18, (uint) i);
                        update.Field18.Value.Add(value);
                    }
                    
                }
                return update;
            }

            public static Improbable.Gdk.Tests.ExhaustiveRepeated.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ExhaustiveRepeated.Snapshot();

                {
                    component.Field1 = new global::System.Collections.Generic.List<BlittableBool>();
                    var list = component.Field1;
                    var listLength = obj.GetBoolCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexBool(1, (uint) i));
                    }
                    
                }

                {
                    component.Field2 = new global::System.Collections.Generic.List<float>();
                    var list = component.Field2;
                    var listLength = obj.GetFloatCount(2);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFloat(2, (uint) i));
                    }
                    
                }

                {
                    component.Field3 = new global::System.Collections.Generic.List<byte[]>();
                    var list = component.Field3;
                    var listLength = obj.GetBytesCount(3);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexBytes(3, (uint) i));
                    }
                    
                }

                {
                    component.Field4 = new global::System.Collections.Generic.List<int>();
                    var list = component.Field4;
                    var listLength = obj.GetInt32Count(4);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt32(4, (uint) i));
                    }
                    
                }

                {
                    component.Field5 = new global::System.Collections.Generic.List<long>();
                    var list = component.Field5;
                    var listLength = obj.GetInt64Count(5);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexInt64(5, (uint) i));
                    }
                    
                }

                {
                    component.Field6 = new global::System.Collections.Generic.List<double>();
                    var list = component.Field6;
                    var listLength = obj.GetDoubleCount(6);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexDouble(6, (uint) i));
                    }
                    
                }

                {
                    component.Field7 = new global::System.Collections.Generic.List<string>();
                    var list = component.Field7;
                    var listLength = obj.GetStringCount(7);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexString(7, (uint) i));
                    }
                    
                }

                {
                    component.Field8 = new global::System.Collections.Generic.List<uint>();
                    var list = component.Field8;
                    var listLength = obj.GetUint32Count(8);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexUint32(8, (uint) i));
                    }
                    
                }

                {
                    component.Field9 = new global::System.Collections.Generic.List<ulong>();
                    var list = component.Field9;
                    var listLength = obj.GetUint64Count(9);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexUint64(9, (uint) i));
                    }
                    
                }

                {
                    component.Field10 = new global::System.Collections.Generic.List<int>();
                    var list = component.Field10;
                    var listLength = obj.GetSint32Count(10);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSint32(10, (uint) i));
                    }
                    
                }

                {
                    component.Field11 = new global::System.Collections.Generic.List<long>();
                    var list = component.Field11;
                    var listLength = obj.GetSint64Count(11);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSint64(11, (uint) i));
                    }
                    
                }

                {
                    component.Field12 = new global::System.Collections.Generic.List<uint>();
                    var list = component.Field12;
                    var listLength = obj.GetFixed32Count(12);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFixed32(12, (uint) i));
                    }
                    
                }

                {
                    component.Field13 = new global::System.Collections.Generic.List<ulong>();
                    var list = component.Field13;
                    var listLength = obj.GetFixed64Count(13);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexFixed64(13, (uint) i));
                    }
                    
                }

                {
                    component.Field14 = new global::System.Collections.Generic.List<int>();
                    var list = component.Field14;
                    var listLength = obj.GetSfixed32Count(14);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSfixed32(14, (uint) i));
                    }
                    
                }

                {
                    component.Field15 = new global::System.Collections.Generic.List<long>();
                    var list = component.Field15;
                    var listLength = obj.GetSfixed64Count(15);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexSfixed64(15, (uint) i));
                    }
                    
                }

                {
                    component.Field16 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId>();
                    var list = component.Field16;
                    var listLength = obj.GetEntityIdCount(16);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexEntityIdStruct(16, (uint) i));
                    }
                    
                }

                {
                    component.Field17 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType>();
                    var list = component.Field17;
                    var listLength = obj.GetObjectCount(17);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.IndexObject(17, (uint) i)));
                    }
                    
                }

                {
                    component.Field18 = new global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum>();
                    var list = component.Field18;
                    var listLength = obj.GetEnumCount(18);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add((global::Improbable.Gdk.Tests.SomeEnum) obj.IndexEnum(18, (uint) i));
                    }
                    
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ExhaustiveRepeated.Component component)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var listSize = obj.GetBoolCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field1.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexBool(1, (uint) i);
                        component.Field1.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetFloatCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field2.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexFloat(2, (uint) i);
                        component.Field2.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetBytesCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field3.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexBytes(3, (uint) i);
                        component.Field3.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetInt32Count(4);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field4.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt32(4, (uint) i);
                        component.Field4.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetInt64Count(5);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field5.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexInt64(5, (uint) i);
                        component.Field5.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetDoubleCount(6);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 6;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field6.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexDouble(6, (uint) i);
                        component.Field6.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetStringCount(7);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field7.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexString(7, (uint) i);
                        component.Field7.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetUint32Count(8);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field8.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexUint32(8, (uint) i);
                        component.Field8.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetUint64Count(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field9.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexUint64(9, (uint) i);
                        component.Field9.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSint32Count(10);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 10;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field10.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSint32(10, (uint) i);
                        component.Field10.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSint64Count(11);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 11;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field11.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSint64(11, (uint) i);
                        component.Field11.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetFixed32Count(12);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 12;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field12.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexFixed32(12, (uint) i);
                        component.Field12.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetFixed64Count(13);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 13;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field13.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexFixed64(13, (uint) i);
                        component.Field13.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSfixed32Count(14);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 14;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field14.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSfixed32(14, (uint) i);
                        component.Field14.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetSfixed64Count(15);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 15;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field15.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexSfixed64(15, (uint) i);
                        component.Field15.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetEntityIdCount(16);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 16;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field16.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = obj.IndexEntityIdStruct(16, (uint) i);
                        component.Field16.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetObjectCount(17);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 17;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field17.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(obj.IndexObject(17, (uint) i));
                        component.Field17.Add(value);
                    }
                    
                }
                {
                    var listSize = obj.GetEnumCount(18);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 18;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (listSize > 0 || isCleared)
                    {
                        component.Field18.Clear();
                    }
                    for (var i = 0; i < listSize; i++)
                    {
                        var value = (global::Improbable.Gdk.Tests.SomeEnum) obj.IndexEnum(18, (uint) i);
                        component.Field18.Add(value);
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::System.Collections.Generic.List<BlittableBool>> Field1;
            public Option<global::System.Collections.Generic.List<float>> Field2;
            public Option<global::System.Collections.Generic.List<byte[]>> Field3;
            public Option<global::System.Collections.Generic.List<int>> Field4;
            public Option<global::System.Collections.Generic.List<long>> Field5;
            public Option<global::System.Collections.Generic.List<double>> Field6;
            public Option<global::System.Collections.Generic.List<string>> Field7;
            public Option<global::System.Collections.Generic.List<uint>> Field8;
            public Option<global::System.Collections.Generic.List<ulong>> Field9;
            public Option<global::System.Collections.Generic.List<int>> Field10;
            public Option<global::System.Collections.Generic.List<long>> Field11;
            public Option<global::System.Collections.Generic.List<uint>> Field12;
            public Option<global::System.Collections.Generic.List<ulong>> Field13;
            public Option<global::System.Collections.Generic.List<int>> Field14;
            public Option<global::System.Collections.Generic.List<long>> Field15;
            public Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntityId>> Field16;
            public Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeType>> Field17;
            public Option<global::System.Collections.Generic.List<global::Improbable.Gdk.Tests.SomeEnum>> Field18;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.ExhaustiveRepeated.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ExhaustiveRepeatedDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveRepeated.ComponentId;

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
                handler.Accept<Component, Update>(ExhaustiveRepeated.ComponentId, DeserializeData, DeserializeUpdate);
            }

            public void InvokeSnapshotHandler(DynamicSnapshot.ISnapshotHandler handler)
            {
                handler.Accept<Snapshot>(ExhaustiveRepeated.ComponentId, DeserializeSnapshot, SerializeSnapshot);
            }
        }
    }
}
