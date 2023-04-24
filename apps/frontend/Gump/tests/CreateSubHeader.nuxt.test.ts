import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import { createI18n } from 'vue-i18n'
import CreateSubHeader from '~/components/CreateSubHeader.vue'

describe('CreateSubHeader (ingredients)', () => {
  let wrapper: VueWrapper<InstanceType<typeof CreateSubHeader>>

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
    wrapper = mount(CreateSubHeader, {
      attachTo: document.body,
      props: {
        variant: 'ingredients',
      },
      global: {
        plugins: [i18n],
      },
    })
  })

  it('should render the title', () => {
    expect(wrapper.html()).toContain('Ingredients')
  })

  it('should render the mode switcher', async () => {
    await wrapper.find('.orangeIcon').trigger('click')
    expect(wrapper.html()).toContain('Raw')
    expect(wrapper.html()).toContain('Design')
  })
})

describe('CreateSubHeader (steps)', () => {
  let wrapper: VueWrapper<InstanceType<typeof CreateSubHeader>>

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
    wrapper = mount(CreateSubHeader, {
      attachTo: document.body,
      props: {
        variant: 'steps',
      },
      global: {
        plugins: [i18n],
      },
    })
  })

  it('should render the title', () => {
    expect(wrapper.html()).toContain('Steps')
  })

  it('should render the tip', async () => {
    await wrapper.find('.orangeIcon').trigger('click')
    expect(wrapper.html()).toContain('CreateStepsTip')
  })
})
