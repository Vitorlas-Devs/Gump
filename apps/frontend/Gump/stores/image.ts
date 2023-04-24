interface IImageState {
  id: number
  url: string
}

export const useImageStore = defineStore('image', () => {
  const state = reactive<IImageState>({
    id: 0,
    url: '',
  })

  const getImage = async (id: number) => {
    if (!state.url) {
      // gumpFetch is defined in composables\gumpFetch.ts
      // const { data } = await useFetch<IImageState>('https://api.gump.live/api' + `/image/${id}`, { method: 'GET' })
      // if (data.value) {
      //   state.id = data.value.id
      //   state.url = data.value.url
      //   return data.value.url
      // }
      return `https://api.gump.live/image/${id}`
    }
    else {
      return state.url
    }
  }

  return {
    ...toRefs(state),
    getImage,
  }
})
