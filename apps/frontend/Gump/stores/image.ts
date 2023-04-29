export const useImageStore = defineStore('image', {
  state: () => ({
    id: 0,
    url: '',
  }),
  getters: {},
  actions: {
    getImage(id: number) {
      return `https://api.gump.live/image/${id}`
    },
  },
  persist: true,
})
