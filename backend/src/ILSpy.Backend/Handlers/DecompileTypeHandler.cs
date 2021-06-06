﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See the LICENSE file in the project root for more information.

using ILSpy.Backend.Decompiler;
using ILSpy.Backend.Protocol;
using OmniSharp.Extensions.JsonRpc;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace ILSpy.Backend.Handlers
{
    [Serial, Method("ilspy/decompileType", Direction.ClientToServer)]
    public class DecompileTypeHandler : IJsonRpcRequestHandler<DecompileTypeRequest, DecompileResponse>
    {
        private readonly IDecompilerBackend decompilerBackend;

        public DecompileTypeHandler(IDecompilerBackend decompilerBackend)
        {
            this.decompilerBackend = decompilerBackend;
        }

        public Task<DecompileResponse> Handle(DecompileTypeRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new DecompileResponse(decompilerBackend.GetCode(request.AssemblyPath, MetadataTokens.EntityHandle(request.Handle))));
        }
    }
}