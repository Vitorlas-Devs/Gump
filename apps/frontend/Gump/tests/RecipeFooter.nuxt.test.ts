import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import type { ComponentPublicInstance } from 'vue'
import RecipeFooter from '~/components/RecipeFooter.vue'

interface IRecipeFooterProps {
  viewCount: number
  likeCount: number
  saveCount: number
  isLiked: boolean
  isSaved: boolean
}

describe('RecipeFooter', () => {
  let wrapper: VueWrapper<ComponentPublicInstance<IRecipeFooterProps>>

  beforeEach(() => {
    wrapper = mount(RecipeFooter, {
      attachTo: document.body,
      props: {
        viewCount: 123,
        likeCount: 1234,
        saveCount: 12345,
        isLiked: false,
        isSaved: false,
      },
    })
  })

  it('should have the correct props', () => {
    expect(wrapper.props()).toEqual({
      viewCount: 123,
      likeCount: 1234,
      saveCount: 12345,
      isLiked: false,
      isSaved: false,
    })
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

    await wrapper.setProps({ isLiked: true, isSaved: true })

    expect(likeIcon.classes()).toContain('i-ph-heart-fill')
    expect(saveIcon.classes()).toContain('i-ph-bookmark-simple-fill')
  })

  it('should emit an event when the icons are clicked', async () => {
    const likeButton = wrapper.find('.cursor-pointer:nth-child(0)')
    const saveButton = wrapper.find('.cursor-pointer:nth-child(1)')

    await likeButton.trigger('click')
    await saveButton.trigger('click')

    expect(wrapper.emitted()).toHaveProperty('like')
    expect(wrapper.emitted()).toHaveProperty('save')
  })
})
