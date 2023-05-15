import { createFetch, useLocalStorage } from '@vueuse/core'

export const gumpFetch = createFetch({
  baseUrl: 'https://api.gump.live/', // https://api.gump.live/ // http://localhost:5135/api
  options: {
    beforeFetch({ options }) {
      const user = useLocalStorage('user', '')
      const token = ref('')
      token.value = JSON.parse(user.value).current.token

      // @ts-expect-error - ;-;
      options.headers.Authorization = `Bearer ${token.value}`
    },
  },
  fetchOptions: {
    headers: {
      'Content-Type': 'application/json',
    },
    mode: 'cors',
  },
})
