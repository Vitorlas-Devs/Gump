<script setup lang="ts">
const props = defineProps<{
  text: string
  // 1. what text to replace, 2. what class to add
  specialValues: Record<string, string>
  // the special value keys to track
  track?: string | string[]
  readonly?: boolean
}>()

const emit = defineEmits<{
  (event: 'input', value: string): void
  (event: 'trackedKeys', keys: string[]): void
}>()

const inputElement = ref<HTMLDivElement | null>(null)
const trackedKeys = ref<string[]>([]) // this holds the keys that are found
const track = computed(() =>
  Array.isArray(props.track) ? props.track : [props.track],
)

function ColorSpecialValues() {
  const specialValues = props.specialValues
  const text = props.text ?? ''
  const keys = Object.keys(specialValues).sort((a, b) => b.length - a.length)
  const regex = new RegExp(keys.join('|'), 'g')
  const matches = text.match(regex)
  if (matches) {
    const tracked = matches.filter(match => track.value.includes(match))
    if (tracked.length > 0) {
      trackedKeys.value = tracked
      emit('trackedKeys', trackedKeys.value)
    }
  }
  if (trackedKeys.value.length > 0) {
    const tracked = trackedKeys.value.filter(key => text.includes(key))
    if (tracked.length === 0) {
      trackedKeys.value = []
      emit('trackedKeys', trackedKeys.value)
    }
  }
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
      class: 'p-3 overflow-hidden w-full',
      style: 'white-space: -moz-pre-space; white-space: pre-wrap;',
      onInput: handleInput,
    },
  )
}
</script>

<template>
  <render />
</template>
