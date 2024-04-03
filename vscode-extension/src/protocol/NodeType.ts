/*------------------------------------------------------------------------------------------------
 *  Copyright (c) 2022 ICSharpCode
 *  Licensed under the MIT License. See LICENSE.TXT in the project root for license information.
 *-----------------------------------------------------------------------------------------------*/

export enum NodeType {
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
  TypeUsedBy,
}
