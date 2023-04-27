import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

export interface IAdvert {
  id: number
  partner: number
  title: string
  image: number
}

export const useAdvertStore = defineStore(
  'advert',
  () => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const state = reactive({
      adverts: [] as IAdvert[]
    })

    const fetchAllAdverts = async () => {
      const response = await fetch(`${backendUrl}/advert`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        }
      })
      const data = await response.json()

      data.sort((a: IAdvert, b: IAdvert) => {
        if ((<string>a.title).toLowerCase() < (<string>b.title).toLowerCase()) {
          return -1
        }
        if ((<string>a.title).toLowerCase() > (<string>b.title).toLowerCase()) {
          return 1
        }
        return 0
      })

      state.adverts = data
    }

    const insertAdvert = async (advert: IAdvert): Promise<IAdvert> => {
      const response = await fetch(`${backendUrl}/advert/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        },
        body: JSON.stringify(advert)
      })
      const data = parseInt(await response.text(), 10)
      advert.id = data
      state.adverts.unshift(advert)
      return advert
    }

    const updateAdvert = async (advert: IAdvert) => {
      await fetch(`${backendUrl}/advert/update`, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        },
        body: JSON.stringify(advert)
      })
    }

    const deleteAdvert = async (id: number) => {
      await fetch(`${backendUrl}/advert/delete/${id}`, {
        method: 'DELETE',
        headers: {
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        }
      })
    }

    return {
      ...toRefs(state),
      fetchAllAdverts,
      insertAdvert,
      updateAdvert,
      deleteAdvert
    }
  },
  {
    persist: true
  }
)
