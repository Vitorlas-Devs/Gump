export const useRetryStore = defineStore('retry', {
  state: () => ({
    function: undefined as (() => Promise<any>) | undefined,
    last: {
      function: undefined as (() => Promise<any>) | undefined,
      times: 0,
    },
  }),
})
