import { Octokit } from '@octokit/core'
import { Base64 } from 'js-base64'

const OWNER = import.meta.env.VITE_OWNER
const REPO = import.meta.env.VITE_REPO

const octokit = new Octokit({
  auth: import.meta.env.VITE_GITHUB_TOKEN
})

export const getMainBranchSha = async (): Promise<string> => {
  const response = await octokit.request('GET /repos/{owner}/{repo}/git/ref/heads/{ref}', {
    owner: OWNER,
    repo: REPO,
    ref: 'main'
  })
  console.log('GET main branch:', response.status)

  return response.data.object.sha
}

export const getOrCreateBranch = async (branchName: string, sha: string) => {
  try {
    const response = await octokit.request('GET /repos/{owner}/{repo}/git/ref/heads/{ref}', {
      owner: OWNER,
      repo: REPO,
      ref: branchName
    })
    // branch exists, return status
    console.log(`GET ${branchName} branch:`, response.status)
  } catch (error) {
    // branch does not exist, create it
    const response = await octokit.request('POST /repos/{owner}/{repo}/git/refs', {
      owner: OWNER,
      repo: REPO,
      ref: `refs/heads/${branchName}`,
      sha: sha
    })
    console.log('CREATE branch:', response.status)
  }
}

// this is the most utterly fucked up function I have ever written
const getSha = (response: any): string => {
  const jsonString = JSON.stringify(response.data)
  const shaIndex = jsonString.indexOf('"sha":"') + 7
  const sha = jsonString.substring(shaIndex, jsonString.indexOf('"', shaIndex))
  return sha
}

// first try to get the file sha, if it exists update it else create it
export const createFilesAndCommit = async (
  branchName: string,
  fileNames: string[],
  contents: string[]
) => {
  try {
    for (let i = 0; i < fileNames.length; i++) {
      const fileName = fileNames[i]
      const content = contents[i]
      const response = await octokit.request('GET /repos/{owner}/{repo}/contents/{path}', {
        owner: OWNER,
        repo: REPO,
        path: `locales/${fileName}.json`,
        ref: branchName
      })
      console.log('GET commit', response.status)
      console.log('GET commit sha', getSha(response))

      try {
        // file exists, but we need to get the latest commit sha
        const getLatestCommitResponse = await octokit.request(
          'GET /repos/{owner}/{repo}/commits/{ref}',
          {
            owner: OWNER,
            repo: REPO,
            ref: branchName
          }
        )
        console.log('GET latest commit:', getLatestCommitResponse.status)
        console.log('GET latest commit sha', getSha(getLatestCommitResponse))

        // file exists, commit exists, update file
        const updateResponse = await octokit.request('PUT /repos/{owner}/{repo}/contents/{path}', {
          owner: OWNER,
          repo: REPO,
          path: `locales/${fileName}.json`,
          message: `${branchName} changed ${fileName}.json`,
          content: Base64.encode(content),
          branch: branchName,
          sha: getSha(getLatestCommitResponse)
        })
        console.log('UPDATE latest commit:', updateResponse.status)
      } catch (error) {
        // file exists no commit, update file
        const updateResponse = await octokit.request('PUT /repos/{owner}/{repo}/contents/{path}', {
          owner: OWNER,
          repo: REPO,
          path: `locales/${fileName}.json`,
          message: `${branchName} changed ${fileName}.json`,
          content: Base64.encode(content),
          branch: branchName,
          sha: getSha(response)
        })
        console.log('UPDATE commit:', updateResponse.status)
      }
    }
  } catch (error) {
    // file does not exist, create it
    for (let i = 0; i < fileNames.length; i++) {
      const fileName = fileNames[i]
      const content = contents[i]
      const createResponse = await octokit.request('PUT /repos/{owner}/{repo}/contents/{path}', {
        owner: OWNER,
        repo: REPO,
        path: `locales/${fileName}.json`,
        message: `${branchName} created ${fileName}.json`,
        content: Base64.encode(content),
        branch: branchName
      })
      console.log('CREATE commit:', createResponse.status)
    }
  }
}

export const createPullRequest = async (branchName: string) => {
  // get pull request or create it
  const response = await octokit.request('GET /repos/{owner}/{repo}/pulls', {
    owner: OWNER,
    repo: REPO,
    head: `${OWNER}:${branchName}`
  })
  console.log('GET pull request:', response.status)
  if (response.data.length === 0) {
    const response = await octokit.request('POST /repos/{owner}/{repo}/pulls', {
      owner: OWNER,
      repo: REPO,
      title: `[Translate] ${branchName}`,
      head: branchName,
      base: 'main'
    })
    console.log('CREATE pull request:', response.status)
  }
}

export const createPullRequestFromContent = async (
  branchName: string,
  fileName: string[],
  content: string[]
) => {
  const sha = await getMainBranchSha()

  await getOrCreateBranch(branchName, sha)

  await createFilesAndCommit(branchName, fileName, content)

  await createPullRequest(branchName)
}
