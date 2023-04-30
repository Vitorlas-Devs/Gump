import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import RecipeBoxMini from '~/components/recipe/RecipeBoxMini.vue'

describe('RecipeBoxMini', () => {
  let wrapper: VueWrapper<InstanceType<typeof RecipeBoxMini>>

  beforeEach(() => {
    wrapper = mount(RecipeBoxMini, {
      attachTo: document.body,
      props: {
        title: 'Hello',
        authorId: 1,
        imgId: 4,
      },
    })
  })

  it('should render the title prop', () => {
    expect(wrapper.html()).toContain('Hello')
  })
})
