import { beforeEach, describe, expect, it } from 'vitest'
import { type VueWrapper, mount } from '@vue/test-utils'
import RecipeBoxMini from '~/components/recipe/RecipeBoxMini.vue'

describe('RecipeBoxMini', () => {
  let wrapper: VueWrapper<InstanceType<typeof RecipeBoxMini>>

  beforeEach(() => {
    wrapper = mount(RecipeBoxMini, {
      attachTo: document.body,
      props: {
        recipe: {
          id: 1,
          title: 'Teszt recept',
          author: 2,
          image: 32,
          language: 'hu_HU',
          serves: 4,
          categories: [
            'd',
          ],
          tags: [
            'teszt',
          ],
          ingredients: [
            {
              name: 'Teszt',
              value: 250,
              volume: 'g',
              linkedRecipe: 0,
            },
          ],
          steps: [
            'Teszteljük le.',
          ],
          viewCount: 1,
          saveCount: 0,
          isLiked: false,
          likeCount: 0,
          referenceCount: 1,
          isArchived: false,
          isOriginal: true,
          originalRecipe: 0,
          isPrivate: false,
          forks: [],
        },
      },
    })
  })

  it('should render the title prop', () => {
    expect(wrapper.html()).toContain('Teszt recept')
  })
})
