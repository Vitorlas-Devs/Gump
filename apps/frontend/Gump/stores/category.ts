// export const useCategoryStore = useStore<Category>('category', true)

const store = defineStore('category', {
  state: () => ({
    selected: null as Category | null,
  }),
  actions: {
    select(category: Category) {
      this.selected = category
    },
  },
})

export const useCategoryStore = useStore<Category>(
  'category',
  store,
  true,
)
