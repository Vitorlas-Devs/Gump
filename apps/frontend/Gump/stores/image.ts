import type { Actions, Getters, StoreFactory } from './shared/generic'

type ImageStore = StoreFactory<'image', Image, {}, ImageGetters, ImageActions>

type ImageGetters = {
  getImageUrl(): (id: number) => string
} & Getters

type ImageActions = {
  uploadImage(file: FileList): Promise<void>
} & Actions

const getters = createGetters<Image, ImageStore>({
  getImageUrl() {
    return (id: number) => {
      return `https://api.gump.live/image/${id}`
    }
  },
})

// 🗑️ - we remove the getAll() from the base actions (redefine it here with undefined to remove it)
// 👀 - now it won't even show up in the devtools and in "this"

const actions = createActions<Image, ImageStore>({
  getAll() {
    return undefined
  },
  async uploadImage(file: FileList) {
    // send image in base64
    const reader = new FileReader()
    reader.readAsDataURL(file[0])
    reader.onload = async () => {
      for (let i = 0; i < 2; i++) {
        const { data, error, statusCode } = await gumpFetch('image', {
          body: JSON.stringify({
            image: reader.result,
          }),
        }).text().post()

        if (data.value) {
          const id = parseInt(data.value, 10)
          const recipe = useRecipeStore()
          if (recipe.currentRecipe)
            recipe.currentRecipe.image = id
          return
        }

        const user = useUserStore()
        if (i === 0 && user.current.token !== 'offline' && statusCode.value === 401) {
          await user.login({
            username: user.current.username,
            password: user.current.password,
          })
          continue
        }

        if (error.value)
          return error.value
      }
    }
  },
})

export const useImageStore = useStore<Image, ImageStore>(
  'image',
  {
    getters,
    actions,
  },
  true,
)
