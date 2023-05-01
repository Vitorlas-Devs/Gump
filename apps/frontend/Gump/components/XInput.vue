<script setup lang="ts">
const props = defineProps<{
  text: string
  // 1. what text to replace, 2. what class to add
  specialValues: Record<string, string>
  readonly?: boolean
}>()

const emit = defineEmits(['input'])
const inputElement = ref<HTMLDivElement | null>(null)

function ColorSpecialValues() {
  const specialValues = props.specialValues
  const text = props.text ?? ''
  const keys = Object.keys(specialValues).sort((a, b) => b.length - a.length)
  const regex = new RegExp(keys.join('|'), 'g')
  return text.replace(regex, (match) => {
    if (specialValues[match])
      return `<span class="${specialValues[match]}">${match}</span>`
    else
      return match
  })
}

onMounted(() => {
  const element = inputElement.value as HTMLDivElement
  element.innerHTML = ColorSpecialValues()
})

watch(() => props.text, () => {
  const element = inputElement.value as HTMLDivElement

  const absoluteCursorPosition = useCursorPosition(element)?.[0] ?? 0

  element.innerHTML = ColorSpecialValues()

  const sel = window.getSelection()
  const focusNode = sel?.focusNode // div (parent of text)

  const { textNode, relativeCursorPosition } = findChildWithCursor(
    focusNode as Node,
    absoluteCursorPosition,
  )

  if (props.text && textNode)
    setCursorPosition(textNode, relativeCursorPosition)
})

function setCursorPosition(element: ChildNode, position: number) {
  const sel = window.getSelection()
  const range = document.createRange()
  range.setStart(element, position)
  range.setEnd(element, position)
  sel?.removeAllRanges()
  sel?.addRange(range)
}

function handleInput(e: Event) {
  const element = e.target as HTMLDivElement
  const input = element.innerHTML

  const result = input.replace(/<[^>]*>/g, '')
  emit('input', result)
}

function focus() {
  const element = inputElement.value as HTMLDivElement
  element.focus()

  let textNode = element.lastChild as ChildNode

  if (textNode && textNode.nodeType === 1)
    textNode = textNode.lastChild as ChildNode

  if (props.text && textNode)
    setCursorPosition(textNode, textNode.textContent?.length ?? 0)
  else if (element.lastChild)
    setCursorPosition(element.lastChild, element.lastChild.textContent?.length ?? 0)
}

defineExpose({ focus })

function render() {
  return h(
    'div',
    {
      contenteditable: !props.readonly,
      ref: inputElement,
      // class: 'h-max w-100 shadow-inner bg-crimson-50 rounded-3xl p-3 min-h-12 overflow-hidden',
      style: 'white-space: -moz-pre-space',
      onInput: handleInput,
    },
  )
}
</script>

<template>
  <render />
</template>
