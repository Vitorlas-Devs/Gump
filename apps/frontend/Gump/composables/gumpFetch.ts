import { createFetch, useLocalStorage } from '@vueuse/core'

export const gumpFetch = createFetch({
  baseUrl: 'https://api.gump.live/', // https://api.gump.live/ // http://localhost:5135/api
  options: {
    beforeFetch({ options }) {
      const userStorage = useLocalStorage('user', '')
      const token = ref('')
      token.value = JSON.parse(userStorage.value).current.token

      // @ts-expect-error - ;-;
      options.headers.Authorization = `Bearer ${token.value}`
    },
    async onFetchError(ctx) {
      const user = useUserStore()
      const retry = useRetryStore()
      const localPath = useLocalePath()
      if (ctx.response?.status === 401 && user.current.token !== 'offline' && user.current.username !== '' && user.current.password !== '') {
        await user.login({
          username: user.current.username,
          password: user.current.password,
        })

        if (retry.function) {
          if (retry.function !== retry.last.function) {
            await retry.function()
            retry.last.function = retry.function
            retry.last.times = 0
          } else {
            if (retry.last.times < 10) {
              await retry.function()
              retry.last.times += 1
            } else {
              retry.last.times = 0
              await navigateTo(localPath('/login'))
            }
          }
          retry.function = undefined
        }
      } else if (user.current.token !== 'offline' && (user.current.username === '' || user.current.password === '')) {
        await navigateTo(localPath('/login'))
      }
      return ctx
    },
  },
  fetchOptions: {
    headers: {
      'Content-Type': 'application/json',
    },
    mode: 'cors',
  },
})
