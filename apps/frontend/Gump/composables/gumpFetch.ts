import { createFetch, useLocalStorage } from '@vueuse/core'

const user = useCookie('user')
const token = useLocalStorage('token', '')
console.log('user', user.value)
console.log('token', token.value)

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
