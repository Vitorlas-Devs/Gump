import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import type { ComponentPublicInstance } from 'vue'
import SearchSelect from '~/components/search/SearchSelect.vue'

type ISearchSelectProps = {
  model: string | string[]
  options: string[]
  mode: 'single' | 'multiple' | 'tags'
  queryFunction?: (query: string) => Promise<any>
}

describe('SearchSelect (tags)', () => {
  let wrapper: VueWrapper<ComponentPublicInstance<ISearchSelectProps>>

  beforeEach(() => {
    wrapper = mount(SearchSelect, {
      attachTo: document.body,
      props: {
        model: [],
        options: ['pizza', 'pasta', 'salad'],
        mode: 'tags',
      },
    })
  })

  it('should render the options prop', () => {
    expect(wrapper.html()).toContain('pizza')
    expect(wrapper.html()).toContain('pasta')
    expect(wrapper.html()).toContain('salad')
  })

  it('should add a tag when an option is selected', async () => {
    await wrapper.find('#multiselect-option-pizza').trigger('click')
    expect(wrapper.emitted()['update:model'][0]).toEqual([['pizza']])
  })

  it('should remove a tag when an option is deselected', async () => {
    await wrapper.setProps({ model: ['pizza'] })
    const pizzaTag = wrapper.find('.multiselect-tag')
    const xmarkIcon = pizzaTag.find('.multiselect-tag-remove')
    await xmarkIcon.trigger('click')
    expect(wrapper.emitted()['update:model'][0]).toEqual([[]])
  })

  it('should create a new option when a tag is entered', async () => {
    await wrapper.find('input').setValue('burger')
    await wrapper.find('input').trigger('keydown.enter')
    expect(wrapper.html()).toContain('burger')
  })

  it('should clear all tags when the clear button is clicked', async () => {
    await wrapper.setProps({ model: ['pizza', 'pasta'] })
    await wrapper.find('.multiselect-clear').trigger('click')
    expect(wrapper.emitted()['update:model'][0]).toEqual([[]])
  })
})

describe('SearchSelect (multiple)', () => {
  let wrapper: VueWrapper<ComponentPublicInstance<ISearchSelectProps>>

  beforeEach(() => {
    wrapper = mount(SearchSelect, {
      attachTo: document.body,
      props: {
        model: [],
        options: ['pizza', 'pasta', 'salad'],
        mode: 'multiple',
      },
    })
  })

  it('should render the options prop', () => {
    expect(wrapper.html()).toContain('pizza')
    expect(wrapper.html()).toContain('pasta')
    expect(wrapper.html()).toContain('salad')
  })

  it('should add a tag when an option is selected', async () => {
    await wrapper.find('#multiselect-option-pizza').trigger('click')
    expect(wrapper.emitted()['update:model'][0]).toEqual([['pizza']])
  })

  it('should display the option when searching', async () => {
    await wrapper.find('input').setValue('pizza')
    const pizzaOption = wrapper.find('#multiselect-option-pizza') // the pizza option
    const caret = wrapper.find('.multiselect-caret.is-open') // open dropdown
    expect(pizzaOption.exists()).toBe(true)
    expect(caret.exists()).toBe(true)
  })

  it('should clear the selected option when the clear button is clicked', async () => {
    await wrapper.setProps({ model: ['pizza'] })
    await wrapper.find('.multiselect-clear').trigger('click')
    expect(wrapper.emitted()['update:model'][0]).toEqual([[]])
  })
})
