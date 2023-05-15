import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import type { ComponentPublicInstance } from 'vue'
import RecipeFooter from '~/components/recipe/RecipeFooter.vue'
import { emptyRecipe } from '~/stores/recipe'

type IRecipeFooterProps = {
  recipe: SearchRecipe | Recipe
}

describe('RecipeFooter', () => {
  let wrapper: VueWrapper<ComponentPublicInstance<IRecipeFooterProps>>

  beforeEach(() => {
    emptyRecipe.viewCount = 123
    emptyRecipe.likeCount = 1234
    emptyRecipe.saveCount = 12345
    wrapper = mount(RecipeFooter, {
      attachTo: document.body,
      props: {
        recipe: emptyRecipe,
      },
    })
  })

  it('should have the correct props', () => {
    expect(wrapper.props()).toEqual({
      recipe: emptyRecipe,
    })
    expect(wrapper.props('recipe').viewCount).toBe(123)
    expect(wrapper.props('recipe').likeCount).toBe(1234)
    expect(wrapper.props('recipe').saveCount).toBe(12345)
  })

  it('should render the counts in the correct format', () => {
    expect(wrapper.html()).toContain('123')
    expect(wrapper.html()).toContain('1.23k')
    expect(wrapper.html()).toContain('12.3k')
  })

  it('should change the icons when the props change', async () => {
    const likeIcon = wrapper.find('.crimsonIcon')
    const saveIcon = wrapper.find('.blueIcon')

    expect(likeIcon.classes()).toContain('i-ph-heart-bold')
    expect(saveIcon.classes()).toContain('i-ph-bookmark-simple-bold')

    // await wrapper.setProps({ isLiked: true, isSaved: true })
    await wrapper.setProps({
      recipe: {
        isLiked: true,
        isSaved: true,
      },
    })
    console.log(wrapper.props('recipe'))

    expect(likeIcon.classes()).toContain('i-ph-heart-fill')
    expect(saveIcon.classes()).toContain('i-ph-bookmark-simple-fill')
  })

  it('should emit an event when the icons are clicked', async () => {
    const likeButton = wrapper.find('#likeButton')
    const saveButton = wrapper.find('#saveButton')

    await likeButton.trigger('click')
    await saveButton.trigger('click')

    expect(wrapper.emitted()).toHaveProperty('like')
    expect(wrapper.emitted()).toHaveProperty('save')
  })
})
