// Copyright (c) 2022 ICSharpCode
// Licensed under the MIT license. See the LICENSE file in the project

namespace ILSpy.Backend.Model;

public enum NodeType
{
    Unknown,
    Assembly,
    Namespace,
    Class,
    Interface,
    Struct,
    Enum,
    Delegate,
    Event,
    Field,
    Method,
    Const,
    Property,
    AssemblyReference,
    ReferencesRoot,
    AttributeAppliedTo,
    EventImplementedBy,
    EventOverriddenBy,
    AssignedByFieldAccess,
    ReadByFieldAccess,
    MemberImplementsInterface,
    MethodImplementedBy,
    MethodOverriddenBy,
    MethodUsedBy,
    MethodUses,
    MethodVirtualUsedBy,
    PropertyImplementedBy,
    PropertyOverriddenBy,
    TypeExposedBy,
    TypeExtensionMethods,
    TypeInstantiatedBy,
    TypeUsedBy
}
