import { createFetch, useLocalStorage } from '@vueuse/core'

const token = useLocalStorage('token', '')

export const gumpFetch = createFetch({
  baseUrl: 'http://localhost:5135/api/',
  fetchOptions: {
    headers: {
      'Authorization': `Bearer ${token.value}`,
      'Content-Type': 'application/json',
    },
    mode: 'cors',
  },
})
