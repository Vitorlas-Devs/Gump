import { describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'
import { NuxtLink } from 'vue-router'
import TheNavbar from '~/components/TheNavbar.vue'
import { tabData } from '~/stores/ui'

describe('TheNavbar', () => {
  it('should render the correct number of NuxtLink elements', () => {
    const wrapper = mount(TheNavbar)
    expect(wrapper.findAllComponents(NuxtLink)).toHaveLength(tabData.length)
  })
})
