import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'
import TheHeader from '~/components/TheHeader.vue'

describe('TheHeader', () => {
  it('should render the title prop', () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
      },
    })
    expect(wrapper.html()).toContain('Hello World')
  })
})
