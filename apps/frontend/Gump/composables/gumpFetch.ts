import { createFetch } from '@vueuse/core'
import { useCookies } from '@vueuse/integrations/useCookies'

const cookies = useCookies(['user'])

const token = ref('')
if (cookies.get('user'))
  token.value = cookies.get('user').current.token

console.log(token.value)

export const gumpFetch = createFetch({
  baseUrl: 'http://localhost:5135/api', // https://api.gump.live/ // http://localhost:5135/api
  fetchOptions: {
    headers: {
      'Authorization': `Bearer ${token.value}`,
      'Content-Type': 'application/json',
    },
    mode: 'cors',
  },
})
