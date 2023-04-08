// node_walk: walk the element tree, stop when func(node) returns false
function node_walk(node: any, func: any) {
  let result = func(node)
  if (node) {
    for (node = node.firstChild; result !== false && node; node = node.nextSibling) {
      result = node_walk(node, func)
    }
  }
  return result
}

// this is just a helper function that actually works... i'm not going to add types
export function useCursorPosition(elem: any) {
  const sel = window.getSelection()
  let cum_length = [0, 0]

  if (sel && sel.anchorNode == elem) {
    cum_length = [sel.anchorOffset, sel.focusOffset]
  } else {
    const nodes_to_find = [sel?.anchorNode, sel?.focusNode]
    if (!elem.contains(sel?.anchorNode) || !elem.contains(sel?.focusNode)) {
      return undefined
    } else {
      const found = [0, 0]
      let i
      node_walk(elem, function (node: Node) {
        for (i = 0; i < 2; i++) {
          if (node == nodes_to_find[i]) {
            found[i] = 1
            if (found[1 - i]) return false
          }
        }

        if (node.textContent && !node.firstChild) {
          for (i = 0; i < 2; i++) {
            if (!found[i]) {
              cum_length[i] += node.textContent.length
            }
          }
        }
      })
      if (sel?.anchorOffset && sel?.focusOffset) {
        cum_length[0] += sel.anchorOffset
        cum_length[1] += sel.focusOffset
      }
    }
  }
  if (cum_length[0] <= cum_length[1]) {
    return cum_length
  }
  return [cum_length[1], cum_length[0]]
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
  // 3 is text node, 1 is element
  if (textNode && textNode.nodeType === 1) {
    textNode = textNode.childNodes[0]
  }

  return { textNode, relativeCursorPosition }
}
