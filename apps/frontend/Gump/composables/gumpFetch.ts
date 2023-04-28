import { createFetch, useLocalStorage } from '@vueuse/core'

const token = useLocalStorage('token', '')

export const gumpFetch = createFetch({
  baseUrl: 'https://api.gump.live/',
  fetchOptions: {
    headers: {
      'Authorization': `Bearer ${token.value}`,
      'Content-Type': 'application/json',
    },
    mode: 'cors',
  },
})
