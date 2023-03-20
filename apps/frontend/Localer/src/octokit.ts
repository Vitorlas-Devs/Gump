import { Octokit } from '@octokit/core'

const octokit = new Octokit({
  auth: import.meta.env.VITE_GITHUB_TOKEN
})

export const getMainBranchSha = async (): Promise<{ getMainBranchStatus: number; sha: string }> => {
  const response = await octokit.request('GET /repos/{owner}/{repo}/git/ref/heads/{ref}', {
    owner: '14A-A-Lyedlik-Devs',
    repo: 'Gump',
    ref: 'main'
  })
  console.log('GET main branch:', response.status)
  return { getMainBranchStatus: response.status, sha: response.data.object.sha }
}

export const getOrCreateBranch = async (branchName: string, sha: string): Promise<number> => {
  try {
    const response = await octokit.request('GET /repos/{owner}/{repo}/git/ref/heads/{ref}', {
      owner: '14A-A-Lyedlik-Devs',
      repo: 'Gump',
      ref: branchName
    })
    // branch exists, return status
    console.log('CREATE branch (exists):', response.status)
    return response.status
  } catch (error) {
    // branch does not exist, create it
    const response = await octokit.request('POST /repos/{owner}/{repo}/git/refs', {
      owner: '14A-A-Lyedlik-Devs',
      repo: 'Gump',
      ref: `refs/heads/${branchName}`,
      sha: sha
    })
    console.log('CREATE branch:', response.status)
    return response.status
  }
}

export const getFile = async (fileName: string): Promise<{ status: number; response: any }> => {
  const response = await octokit.request('GET /repos/{owner}/{repo}/contents/{path}', {
    owner: '14A-A-Lyedlik-Devs',
    repo: 'Gump',
    path: `locales/${fileName}.json`
  })
  console.log('GET file:', response.status)
  console.log('data:', response.data)
  const sha = getSha(response)
  console.log('sha:', sha)
  return { response: response, status: response.status }
}

// this is the most utterly fucked up function I have ever written
const getSha = (response: any): string => {
  const jsonString = JSON.stringify(response.data)
  const shaIndex = jsonString.indexOf('"sha":"') + 7
  const sha = jsonString.substring(shaIndex, jsonString.indexOf('"', shaIndex))
  return sha
}

// first try to get the file sha, if it exists update it
// if it does not exist, create it
export const createFileAndCommit = async (
  branchName: string,
  fileName: string,
  content: string
): Promise<{ status: number; response: any }> => {
  try {
    const response = await octokit.request('GET /repos/{owner}/{repo}/contents/{path}', {
      owner: '14A-A-Lyedlik-Devs',
      repo: 'Gump',
      path: `locales/${fileName}.json`
    })
    console.log('CREATE file and commit (get):', response.status)
    console.log(response.data)
    console.log(getSha(response))
    // file exists, update it
    const updateResponse = await octokit.request('PUT /repos/{owner}/{repo}/contents/{path}', {
      owner: '14A-A-Lyedlik-Devs',
      repo: 'Gump',
      path: `locales/${fileName}.json`,
      message: `${branchName} changed ${fileName}.json`,
      content: btoa(content),
      branch: branchName,
      sha: getSha(response)
    })
    console.log('CREATE file and commit (update):', updateResponse.status)
    return { response: response, status: updateResponse.status }
  } catch (error) {
    // file does not exist, create it
    const createResponse = await octokit.request('PUT /repos/{owner}/{repo}/contents/{path}', {
      owner: '14A-A-Lyedlik-Devs',
      repo: 'Gump',
      path: `locales/${fileName}.json`,
      message: `${branchName} created ${fileName}.json`,
      content: btoa(content),
      branch: branchName
    })
    console.log('CREATE file and commit (create):', createResponse.status)
    return { response: createResponse, status: createResponse.status }
  }
}

export const createPullRequest = async (branchName: string): Promise<number> => {
  const response = await octokit.request('POST /repos/{owner}/{repo}/pulls', {
    owner: '14A-A-Lyedlik-Devs',
    repo: 'Gump',
    title: `${branchName} committed new translations`,
    head: branchName,
    base: 'main'
  })
  console.log('CREATE pull request:', response.status)
  return response.status
}

export const createPullRequestFromContent = async (
  branchName: string,
  fileName: string,
  content: string
): Promise<{
  getMainBranchStatus: number
  createBranchStatus: number
  status: number
  createPullRequestStatus: number
}> => {
  const { getMainBranchStatus, sha } = await getMainBranchSha()

  const createBranchStatus = await getOrCreateBranch(branchName, sha)

  await getFile(fileName)

  const createFileAndCommitStatus = await createFileAndCommit(branchName, fileName, content)
  const status = createFileAndCommitStatus.status

  const createPullRequestStatus = await createPullRequest(branchName)
  return {
    getMainBranchStatus,
    createBranchStatus,
    status,
    createPullRequestStatus
  }
}
