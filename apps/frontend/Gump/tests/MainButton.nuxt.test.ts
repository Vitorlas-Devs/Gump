import { describe, expect, it } from 'vitest'
import { setup } from '@nuxt/test-utils'
import { mount } from '@vue/test-utils'
import MainButton from '~/components/MainButton.vue'

describe('MainButton', () => {
  setup()

  it('should render the title prop', () => {
    const wrapper = mount(MainButton, {
      props: {
        title: 'My Title',
        iconType: 'create',
        color: 'orange',
      },
    })
    expect(wrapper.html()).toContain('My Title')
  })

  it('should render the iconType prop', () => {
    const wrapper = mount(MainButton, {
      props: {
        title: 'My Title',
        iconType: 'delete',
        color: 'crimson',
      },
    })
    expect(wrapper.find('.i-fa6-solid-trash-can').exists()).toBe(true)
  })

  it('should render the color prop', () => {
    const wrapper = mount(MainButton, {
      props: {
        title: 'My Title',
        iconType: 'create',
        color: 'orange',
      },
    })
    expect(wrapper.find('.orangeGradient').exists()).toBe(true)
  })
})
