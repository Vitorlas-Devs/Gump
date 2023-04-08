import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

interface RequestErrorState {
  createBranchError: {
    error: boolean
    status: number | null
  }
  createOrUpdateFilesError: {
    error: boolean
    status: number | null
  }
  createPullRequestError: {
    error: boolean
    status: number | null
  }
}

export const useRequestErrorStore = defineStore('requestError', () => {
  const state = reactive<RequestErrorState>({
    createBranchError: {
      error: false,
      status: null
    },
    createOrUpdateFilesError: {
      error: false,
      status: null
    },
    createPullRequestError: {
      error: false,
      status: null
    }
  })

  const resetErrors = () =>
    Object.keys(state).forEach((key) => {
      state[key as keyof RequestErrorState].error = false
      state[key as keyof RequestErrorState].status = null
    })

  return {
    ...toRefs(state),
    resetErrors
  }
})
