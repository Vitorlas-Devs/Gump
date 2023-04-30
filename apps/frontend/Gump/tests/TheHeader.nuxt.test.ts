import { beforeEach, describe, expect, it } from 'vitest'
import { mount } from '@vue/test-utils'
import TheHeader from '~/components/TheHeader.vue'

describe('TheHeader', () => {
  const mockStore = useUIStore()

  beforeEach(() => {
    mockStore.activeNav = 'Home'
    mockStore.activeSort = 'hot'
  })

  it('should render the title prop', () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
      },
    })
    expect(wrapper.html()).toContain('Hello World')
  })

  it('should render the subtitle prop if provided', () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
        subtitle: 'This is a subtitle',
      },
    })
    expect(wrapper.html()).toContain('This is a subtitle')
  })

  it('should render the title color prop if provided', () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
        titleColor: 'blue',
      },
    })
    expect(wrapper.find('h1').classes()).toContain('text-blue-500')
  })

  it('should render the moderator label if showModerator prop is true', () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
        showModerator: true,
      },
      global: {
        mocks: {
          $t: (key: string) => key,
        },
      },
    })
    expect(wrapper.html()).toContain('ProfileModerator')
  })

  it('should render the icons if showIcons prop is true', () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
        showIcons: true,
      },
    })
    expect(wrapper.findAll('div').length).toBe(6)
  })

  it('should update the ui store when an icon is clicked', async () => {
    const wrapper = mount(TheHeader, {
      props: {
        title: 'Hello World',
        showIcons: true,
      },
    })
    const icon = wrapper.findAll('div')[0]
    await icon.trigger('click', { button: 0 })

    expect(mockStore.activeSort).toBe('hot')
  })
})
