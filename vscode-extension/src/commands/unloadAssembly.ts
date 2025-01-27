/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

import * as vscode from "vscode";
import {
  DecompiledTreeProvider,
  MemberNode,
} from "../decompiler/DecompiledTreeProvider";

export function registerUnloadAssembly(
  decompiledTreeProvider: DecompiledTreeProvider
) {
  return vscode.commands.registerCommand(
    "ilspy.unloadAssembly",
    async (node: MemberNode) => {
      if (!node) {
        vscode.window.showInformationMessage(
          'Please use context menu: right-click on the assembly node then select "Unload Assembly"'
        );
        return;
      }
      console.log("Unloading assembly " + node.name);
      const removed = await decompiledTreeProvider.removeAssembly(node.name);
      if (removed) {
        decompiledTreeProvider.refresh();
      }
    }
  );
}
