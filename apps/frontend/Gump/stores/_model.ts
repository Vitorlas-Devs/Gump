import type { UnwrapRef } from 'nuxt/dist/app/compat/capi'

// define a store factory that takes a generic type and returns a store
export function useStore<T extends { 'id': string }>(name: string) {
  // name the store using the generic type
  return defineStore(`${name}Store`, () => {
    const state = reactive({
      name: 'Alice',
      age: 25,
      data: null as T | null,
    })
    const getters = {
      greeting: () => `Hello, ${state.name}!`,
    }
    const actions = {
      birthday: () => {
        state.age++
      },
      setData: (data: T) => {
        state.data = data as UnwrapRef<T> | null
      },
    }
    return {
      state,
      getters,
      actions,
    }
  })
}
