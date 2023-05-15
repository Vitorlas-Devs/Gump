import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import RecipeBoxMini from '~/components/recipe/RecipeBoxMini.vue'
import { emptyRecipe } from '~/stores/recipe'

describe('RecipeBoxMini', () => {
  let wrapper: VueWrapper<InstanceType<typeof RecipeBoxMini>>

  beforeEach(() => {
    emptyRecipe.title = 'Test recipe'
    wrapper = mount(RecipeBoxMini, {
      attachTo: document.body,
      props: {
        recipe: emptyRecipe,
      },
    })
  })

  it('should render the title prop', () => {
    expect(wrapper.html()).toContain('Test recipe')
  })
})
