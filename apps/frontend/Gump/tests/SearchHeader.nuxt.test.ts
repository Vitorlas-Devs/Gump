import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import { createI18n } from 'vue-i18n'
import SearchHeader from '~/components/search/SearchHeader.vue'

describe('SearchHeader', () => {
  let wrapper: VueWrapper<InstanceType<typeof SearchHeader>>

  beforeEach(() => {
    const i18n = createI18n ({
      messages: {
        en: {
          tip: 'Tip',
        },
        ch: {
          tip: '提示',
        },
      },
    })
    wrapper = mount(SearchHeader, {
      attachTo: document.body,
      global: {
        plugins: [i18n],
      },
    })
  })

  afterEach(() => {
    vi.resetAllMocks()
  })

  it('should render the title when ui.searchToggled is false', async () => {
    const ui = useUIStore()
    ui.searchToggled = false
    await wrapper.vm.$nextTick()
    expect(wrapper.html()).toContain('SearchNav')
  })

  it('should render the input when ui.searchToggled is true', async () => {
    const ui = useUIStore()
    ui.searchToggled = true
    await wrapper.vm.$nextTick()
    expect(wrapper.find('input').exists()).toBe(true)
  })

  // this breaks vitest and it runs forever
  // it('should focus on the input when ui.searchToggled is true', async () => {
  //   const ui = useUIStore()
  //   ui.searchToggled = true
  //   await wrapper.vm.$nextTick()
  //   const input = wrapper.find('input')
  //   expect(input.element).toBe(document.activeElement)
  // })

  it('should update ui.searchValue with the input value', async () => {
    const ui = useUIStore()
    ui.searchToggled = true
    await wrapper.vm.$nextTick()
    const input = wrapper.find('input')
    input.element.value = 'test'
    await input.trigger('input')
    expect(ui.searchValue).toBe('test')
  })
})
