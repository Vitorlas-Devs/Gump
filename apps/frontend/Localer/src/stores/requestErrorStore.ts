import { defineStore } from 'pinia'

export const useRequestErrorStore = defineStore({
  id: 'requestError',
  state: () => ({
    getContentError: false,
    createBranchError: false,
    createOrUpdateFileError: false,
    createFilesAndCommitError: false,
    createPullRequestError: false
  }),
  actions: {
    resetErrors() {
      this.getContentError = false
      this.createBranchError = false
      this.createOrUpdateFileError = false
      this.createFilesAndCommitError = false
      this.createPullRequestError = false
    }
  }
})
