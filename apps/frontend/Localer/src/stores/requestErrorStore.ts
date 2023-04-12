import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

export interface IRequestError {
  id: string
  error: boolean
  status: number | null
}

interface IRequestErrorState {
  createBranchError: IRequestError
  createOrUpdateFilesError: IRequestError
  createPullRequestError: IRequestError
}

export const useRequestErrorStore = defineStore('requestError', () => {
  const state = reactive<IRequestErrorState>({
    createBranchError: {
      id: 'createBranchError',
      error: false,
      status: null
    },
    createOrUpdateFilesError: {
      id: 'createOrUpdateFilesError',
      error: false,
      status: null
    },
    createPullRequestError: {
      id: 'createPullRequestError',
      error: false,
      status: null
    }
  })

  const resetErrors = () =>
    Object.keys(state).forEach((key) => {
      state[key as keyof IRequestErrorState].error = false
      state[key as keyof IRequestErrorState].status = null
    })

  return {
    ...toRefs(state),
    resetErrors
  }
})
