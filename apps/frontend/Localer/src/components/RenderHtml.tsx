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
