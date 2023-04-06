import { Octokit } from '@octokit/rest'
import type { components } from '@octokit/openapi-types'
import type { Endpoints } from '@octokit/types'
import { Base64 } from 'js-base64'
import { useUserStore } from './stores/userStore'
import { storeToRefs } from 'pinia'

const OWNER = import.meta.env.VITE_OWNER
const REPO = import.meta.env.VITE_REPO

const octokit = new Octokit()

// GET TYPED YOU UNGRATEFUL, UNGAINLY, UNWASHED, UNWIELDY OCTOKIT RESPONSES
type CreateBranchResponse = Endpoints['POST /repos/{owner}/{repo}/git/refs']['response']
type GetCommitResponse = Endpoints['GET /repos/{owner}/{repo}/commits/{ref}']['response']['data']
type GetPullRequestResponse = Endpoints['GET /repos/{owner}/{repo}/pulls']['response']['data'][0]
type GetRepoContentResponseDataFile = components['schemas']['content-file']
// getContent type is still broken: https://github.com/octokit/rest.js/issues/32

export const getAuthenticatedUser = async () => {
  const { token } = storeToRefs(useUserStore())
  const { data } = await octokit.request('GET /user', {
    headers: {
      Authorization: `token ${token.value}`
    }
  })

  return { name: data.login, avatar: data.avatar_url }
}

/**
 * Gets the latest commit for a given branch. Useful to get the sha of the latest commit.
 * @async
 * @param { string } branchName - The name of the branch to get the latest commit for. This is the username.
 * @returns { Promise<{ response: GetCommitResponse | undefined; status: number; error?: any }> } The latest commit for the given branch.
 */
export const getLatestCommit = async (
  branchName: string
): Promise<{ response: GetCommitResponse | undefined; status: number; error?: any }> => {
  try {
    const { data, status } = await octokit.request('GET /repos/{owner}/{repo}/commits/{ref}', {
      owner: OWNER,
      repo: REPO,
      ref: branchName
    })
    console.log('GET latest commit:', status)

    return { response: data, status }
  } catch (error: any) {
    console.log('GET latest commit error:', error.status)
    console.log('GET latest commit error:', error)

    return { response: undefined, status: error.status, error }
  }
}

/**
 * Gets the pull request for a given branch.
 * @async
 * @param { string } branchName - The name of the branch to get the pull request for. This is the username.
 * @returns { Promise<{ GetPullRequestResponse: any | undefined; status: number; error?: any }> } The pull request for the given branch.
 * @throws { Error } If the pull request does not exist.
 */
export const getPullRequest = async (
  branchName: string
): Promise<{ response: GetPullRequestResponse | undefined; status: number; error?: any }> => {
  try {
    const response = await octokit.request('GET /repos/{owner}/{repo}/pulls', {
      owner: OWNER,
      repo: REPO,
      head: `${OWNER}:${branchName}`
    })
    if (response.data.length !== 0) {
      console.log('GET pull request:', response.status)
      console.log('GET pull request number:', response.data[0].number)
      console.log('GET pull request title:', response.data[0].title)
      console.log('GET pull request html_url:', response.data[0].html_url)
    }

    return { response: response.data[0], status: response.status }
  } catch (error: any) {
    console.log('GET pull request error:', error.status)
    console.log('GET pull request error:', error)

    return { response: undefined, status: error.status, error }
  }
}

/**
 * Gets the content for a given file.
 * @async
 * @param { string } branchName - The name of the branch to get the content for. This is the username.
 * @param { string } fileName - The name of the file to get the content for. This is the locale.
 * @returns { Promise<{ response: GetRepoContentResponseDataFile | undefined; status: number; error?: any }> } The content for the given file or undefined if the file or branch does not exist.
 */
export const getContent = async (
  branchName: string,
  fileName: string
): Promise<{
  response: GetRepoContentResponseDataFile | undefined
  status: number
  error?: any
}> => {
  try {
    const { data, status } = await octokit.request('GET /repos/{owner}/{repo}/contents/{path}', {
      owner: OWNER,
      repo: REPO,
      path: `locales/${fileName}.json`,
      ref: branchName
    })
    console.log('GET content:', status)

    return { response: data as GetRepoContentResponseDataFile, status: status }
  } catch (error: any) {
    console.log('GET content error:', error.status)
    console.log('GET content error:', error)

    return { response: undefined, status: error.status, error }
  }
}

/**
 * Gets the sha for a given branch.
 * @async
 * @param { string } [ branchName=main ] - The name of the branch to get the sha for. This is the username. Defaults to main.
 * @returns { Promise<{ sha: string | undefined; status: number; error?: any }> } The response from the get branch request and the sha of the branch.
 */
export const getBranch = async (
  branchName?: string
): Promise<{ sha: string; status: number; error?: any }> => {
  try {
    if (!branchName) {
      branchName = 'main'
    }
    const response = await octokit.request('GET /repos/{owner}/{repo}/git/ref/heads/{ref}', {
      owner: OWNER,
      repo: REPO,
      ref: branchName
    })
    console.log(`GET ${branchName} branch:`, response.status)

    return { sha: response.data.object.sha, status: response.status }
  } catch (error: any) {
    console.log(`GET ${branchName} branch error:`, error.status)
    console.log(`GET ${branchName} branch error:`, error)

    return { sha: '', status: error.status, error }
  }
}

/**
 * Tries to get a branch, if it exists it returns the branch, if it does not exist it creates the branch.
 * @async
 * @param { string } branchName - The name of the branch to create. This is the username.
 * @param { string } sha - The sha of the commit to create the branch from.
 * @returns { Promise<{ response: CreateBranchResponse | undefined; status: number; error?: any }> } The response from the get branch request or the create branch request or undefined.
 */
export const CreateBranch = async (
  branchName: string,
  sha: string
): Promise<{ response: CreateBranchResponse | undefined; status: number; error?: any }> => {
  const { status, error } = await getBranch(branchName)
  if (status === 200) {
    return { response: undefined, status, error }
  } else {
    try {
      const { token } = storeToRefs(useUserStore())
      const response = await octokit.request('POST /repos/{owner}/{repo}/git/refs', {
        headers: {
          authorization: `token ${token.value}`
        },
        owner: OWNER,
        repo: REPO,
        ref: `refs/heads/${branchName}`,
        sha: sha
      })
      console.log('CREATE branch:', response.status)

      return { response, status: response.status }
    } catch (error: any) {
      console.log('CREATE branch error:', error.status)
      console.log('CREATE branch error:', error)

      return { response: undefined, status: error.status, error }
    }
  }
}

/**
 * Creates or updates a file.
 * @async
 * @param { string } branchName - The name of the branch to create the file on. This is the username.
 * @param { string[] } fileNames - The name of the file to create. This is the locale.
 * @param { string[] } contents - The content of the file to create.
 * @returns { Promise<{ status: number; error?: any }> } The response from the create or update file request.
 */
export const createOrUpdateFiles = async (
  branchName: string,
  fileNames: string[],
  contents: string[]
): Promise<{ status: number; error?: any }> => {
  try {
    const { token } = storeToRefs(useUserStore())
    const files = fileNames.map((fileName, index) => ({
      name: fileName,
      content: contents[index]
    }))
    const encodedFiles = files.map((file) => ({
      ...file,
      content: Base64.encode(file.content)
    }))

    const latestCommitResponse = await getLatestCommit(branchName)

    const blobPromises = encodedFiles.map((file) =>
      octokit.request('POST /repos/{owner}/{repo}/git/blobs', {
        headers: {
          authorization: `token ${token.value}`
        },
        owner: OWNER,
        repo: REPO,
        content: file.content,
        encoding: 'base64'
      })
    )
    const blobs = await Promise.all(blobPromises)

    const tree = blobs.map((blob, index) => ({
      path: `locales/${encodedFiles[index].name}.json`,
      mode: '100644' as const,
      type: 'blob' as const,
      sha: blob.data.sha
    }))
    const { data: treeData } = await octokit.request('POST /repos/{owner}/{repo}/git/trees', {
      headers: {
        authorization: `token ${token.value}`
      },
      owner: OWNER,
      repo: REPO,
      base_tree: latestCommitResponse.response?.commit.tree.sha,
      tree
    })

    let commitMsg = '' // list the files that were changed
    for (const file of files) {
      commitMsg += `${file.name}, `
    }
    commitMsg = commitMsg.slice(0, -2) // remove the last comma and space

    const { data: newCommit } = await octokit.request('POST /repos/{owner}/{repo}/git/commits', {
      headers: {
        authorization: `token ${token.value}`
      },
      owner: OWNER,
      repo: REPO,
      message: `${branchName} changed ${commitMsg}`,
      tree: treeData.sha,
      parents: [latestCommitResponse.response?.sha!]
    })

    const response = await octokit.request('PATCH /repos/{owner}/{repo}/git/refs/heads/{ref}', {
      headers: {
        authorization: `token ${token.value}`
      },
      owner: OWNER,
      repo: REPO,
      ref: branchName,
      sha: newCommit.sha
    })
    console.log('CREATE/UPDATE file:', response.status)

    return { status: response.status }
  } catch (error: any) {
    console.log('CREATE/UPDATE file error:', error.status)
    console.log('CREATE/UPDATE file error:', error)

    return { status: error.status, error }
  }
}

/**
 * Creates a pull request.
 * @async
 * @param { string } branchName - The name of the branch to create the pull request from. This is the username.
 * @returns { Promise<{ prNumber?: number; prUrl?: string; status: number; error?: any }> } The response from the create pull request request or undefined.
 */
export const createPullRequest = async (
  branchName: string
): Promise<{
  prNumber?: number
  prUrl?: string
  status: number
  error?: any
}> => {
  const { response: getResponse, status } = await getPullRequest(branchName)
  if (getResponse) {
    return { prNumber: getResponse?.number, prUrl: getResponse?.html_url, status }
  } else {
    try {
      const { token } = storeToRefs(useUserStore())
      const response = await octokit.request('POST /repos/{owner}/{repo}/pulls', {
        headers: {
          authorization: `token ${token.value}`
        },
        owner: OWNER,
        repo: REPO,
        title: `[Translate] ${branchName}`,
        head: branchName,
        base: 'main'
      })
      console.log('CREATE pull request:', response.status)

      return {
        prNumber: response.data.number,
        prUrl: response.data.html_url,
        status: response.status
      }
    } catch (error: any) {
      console.log('CREATE pull request error:', error.status)
      console.log('CREATE pull request error:', error.message)

      return { status: error.status, error }
    }
  }
}
