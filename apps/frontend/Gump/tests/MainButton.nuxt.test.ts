import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'
import MainButton from '~/components/MainButton.vue'

describe('MainButton', () => {
  it('should render the title prop', () => {
    const wrapper = mount(MainButton, {
      props: {
        title: 'My Title',
        iconType: 'create',
        color: 'orangeGradient',
      },
    })
    expect(wrapper.html()).toContain('My Title')
  })

  it('should render the iconType prop', () => {
    const wrapper = mount(MainButton, {
      props: {
        title: 'My Title',
        iconType: 'delete',
        color: 'crimsonGradient',
      },
    })
    expect(wrapper.find('.i-fa6-solid-trash-can').exists()).toBe(true)
  })

  it('should render the color prop', () => {
    const wrapper = mount(MainButton, {
      props: {
        title: 'My Title',
        iconType: 'create',
        color: 'orangeGradient',
      },
    })
    expect(wrapper.find('.orangeGradient').exists()).toBe(true)
  })
})
