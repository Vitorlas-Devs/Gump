import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import type { ComponentPublicInstance } from 'vue'
import { createI18n } from 'vue-i18n'
import SearchDropdown from '~/components/search/SearchDropdown.vue'

type ISeachDropdownProps = {
  topPosition?: number
  showResults?: boolean
}

describe('SearchDropdown', () => {
  let wrapper: VueWrapper<ComponentPublicInstance<ISeachDropdownProps>>

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
    wrapper = mount(SearchDropdown, {
      global: {
        plugins: [i18n],
      },
    })
  })

  it('should render the title', () => {
    expect(wrapper.html()).toContain('SearchRecent')
  })

  it('should render the tip', () => {
    expect(wrapper.html()).toContain('SearchTip')
  })

  it('should not render the search history if empty', async () => {
    const ui = useUIStore()
    ui.searchHistory = []
    await wrapper.vm.$nextTick()
    expect(wrapper.html()).not.toContain('orangeIcon')
  })

  it('should render the search history', async () => {
    const ui = useUIStore()
    ui.searchHistory = ['test1', 'test2']
    await wrapper.vm.$nextTick()
    expect(wrapper.html()).toContain('test1')
    expect(wrapper.html()).toContain('test2')
  })
})
