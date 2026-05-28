/*------------------------------------------------------------------------------------------------
 *  Copyright (c) 2022 ICSharpCode
 *  Licensed under the MIT License. See LICENSE.TXT in the project root for license information.
 *-----------------------------------------------------------------------------------------------*/

import * as vscode from "vscode";
import { DecompilerTextDocumentContentProvider } from "../decompiler/DecompilerTextDocumentContentProvider";
import { ILSPY_TEXT_MD_LANG, languageInfos } from "../decompiler/languageInfos";
import { nodeDataToUri } from "../decompiler/nodeUri";
import { getDefaultOutputLanguage } from "../decompiler/settings";
import { hasNodeCommand } from "../decompiler/utils";
import { executeILSpyCommand, registerILSpyCommand } from "./commandUtils";
import {
  AvailableNodeCommands,
  DecompilationFormat,
  Node,
} from "../extension-types";

let lastSelectedNode: Node | undefined = undefined;

export function registerDecompileNodeCommand(
  contentProvider: DecompilerTextDocumentContentProvider,
) {
  return registerILSpyCommand(
    "ilspy.decompileNode",
    async (node: Node, revealInTree = false) => {
      const uri = nodeDataToUri(node);
      if (hasNodeCommand(node, AvailableNodeCommands.Decompile)) {
        const language = getDefaultOutputLanguage();

        contentProvider.setDocumentOutputLanguage(uri, language);

        if (node.decompiledAs === DecompilationFormat.MarkdownDocument) {
          await vscode.commands.executeCommand("markdown.showPreview", uri);
        } else {
          let doc = await vscode.workspace.openTextDocument(uri);
          vscode.languages.setTextDocumentLanguage(
            doc,
            languageInfos[language].vsLanguageMode,
          );
          await vscode.window.showTextDocument(doc, { preview: true });
        }
      }

      if (revealInTree) {
        await executeILSpyCommand("ilspy.revealNode", node);
      }
    },
  );
}
