// nodeWalk: walk the element tree, stop when func(node) returns false
function nodeWalk(node: any, func: any) {
  let result = func(node)

  if (node) {
    for (node = node.firstChild; result !== false && node; node = node.nextSibling) {
      result = nodeWalk(node, func)
    }
  }

  return result
}

// this is just a helper function that actually works... i just found it and i'm not going to add types
export function useCursorPosition(element: any) {
  const sel = window.getSelection()
  // cumulative length of text nodes, [0] is anchor, [1] is focus
  // anchor is the start of the selection, focus is the end
  let cumLength = [0, 0]

  if (sel && sel.anchorNode == element) {
    cumLength = [sel.anchorOffset, sel.focusOffset]
  } else {
    const nodesToFind = [sel?.anchorNode, sel?.focusNode]
    if (!element.contains(sel?.anchorNode) || !element.contains(sel?.focusNode)) {
      return undefined
    } else {
      const found = [0, 0]
      let i

      nodeWalk(element, function (node: Node) {
        for (i = 0; i < 2; i++) {
          if (node == nodesToFind[i]) {
            found[i] = 1
            if (found[1 - i]) {
              return false
            }
          }
        }

        if (node.textContent && !node.firstChild) {
          for (i = 0; i < 2; i++) {
            if (!found[i]) {
              cumLength[i] += node.textContent.length
            }
          }
        }
      })
      if (sel?.anchorOffset && sel?.focusOffset) {
        cumLength[0] += sel.anchorOffset
        cumLength[1] += sel.focusOffset
      }
    }
  }
  if (cumLength[0] <= cumLength[1]) {
    return cumLength
  }

  return [cumLength[1], cumLength[0]]
}

export const findChildWithCursor = (node: Node, cursorPosition: number) => {
  let nodeEndPosition = 0
  let relativeCursorPosition = 0
  let childNode: Node | null = null

  node.childNodes.forEach((child: Node) => {
    if (child.textContent) {
      // start adding the length of the text nodes until the cursor position is reached
      if (child.textContent && nodeEndPosition < cursorPosition) {
        nodeEndPosition += child.textContent.length
      }

      // if the cursor position is reached, set the child node
      if (nodeEndPosition >= cursorPosition && !childNode) {
        childNode = child
        relativeCursorPosition = cursorPosition - (nodeEndPosition - child.textContent.length)
      }
    }
  })

  const index = Array.prototype.indexOf.call(node.childNodes, childNode)

  let textNode = node.childNodes[index]

  // nodeType: 3 is text node, 1 is element
  if (textNode && textNode.nodeType === 1) {
    textNode = textNode.childNodes[0]
  }

  return { textNode, relativeCursorPosition }
}
