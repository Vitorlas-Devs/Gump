import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'
import { RouterLink } from 'vue-router'
import TheNavbar from '~/components/TheNavbar.vue'
import { tabData } from '~/stores/ui'

describe('TheNavbar', () => {
  it('should render the correct number of RouterLink elements', () => {
    const wrapper = mount(TheNavbar)
    expect(wrapper.findAllComponents(RouterLink)).toHaveLength(tabData.length)
  })
})
