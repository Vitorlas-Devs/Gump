import { createFetch } from '@vueuse/core'
import { useCookies } from '@vueuse/integrations/useCookies'

export const gumpFetch = createFetch({
  baseUrl: 'https://api.gump.live/', // https://api.gump.live/ // http://localhost:5135/api
  options: {
    beforeFetch({ options }) {
      const cookies = useCookies(['user'])

      const token = ref('')
      if (cookies.get('user'))
        token.value = cookies.get('user').current.token

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
