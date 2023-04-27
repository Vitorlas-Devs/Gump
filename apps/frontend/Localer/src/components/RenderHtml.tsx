// @ts-nocheck
import { defineComponent } from 'vue'

const RenderHtml = defineComponent({
  name: 'RenderHtml',
  props: {
    html: {
      type: String,
      required: true
    }
  },
  emits: ['input'],
  setup(props, { emit }) {
    const vnode = (
      <div
        type="text"
        flex="grow"
        p="3"
        shadow="inner"
        bg="crimson-50"
        rounded="3xl"
        h="min-12"
        w="100"
        resize="none"
        overflow="hidden"
        style="white-space: -moz-pre-space"
        innerHTML={props.html || ''}
        onInput={(e: Event) => {
          emit('input', (e.target as HTMLDivElement).innerHTML)
        }}
      />
    )

    return () => vnode
  }
})

export default RenderHtml
