import { defineStore } from 'pinia'
import { reactive, toRefs } from 'vue'

export interface IPartner {
  id: number
  name: string
  contactUrl: string
  apiUrl: string
  ads: number[]
}

export const usePartnerStore = defineStore(
  'partner',
  () => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL

    const state = reactive({
      partners: [] as IPartner[]
    })

    const fetchAllPartners = async () => {
      const response = await fetch(`${backendUrl}/partner`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        }
      })
      const data = await response.json()

      data.sort((a: IPartner, b: IPartner) => {
        if (a.name < b.name) {
          return -1
        }
        if (a.name > b.name) {
          return 1
        }
        return 0
      })

      state.partners = data
    }

    const insertPartner = async (partner: IPartner): Promise<IPartner> => {
      const response = await fetch(`${backendUrl}/partner/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        },
        body: JSON.stringify(partner)
      })
      const data = parseInt(await response.text(), 10)
      partner.id = data
      partner.ads = []
      state.partners.unshift(partner)
      return partner
    }

    const updatePartner = async (partner: IPartner) => {
      await fetch(`${backendUrl}/partner/update`, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
          authorization: `Bearer ${
            JSON.parse(localStorage.getItem('gumpUser') ?? '')?.sessionToken
          }`
        },
        body: JSON.stringify(partner)
      })
    }

    const deletePartner = async (id: number) => {
      await fetch(`${backendUrl}/partner/delete/${id}`, {
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
      fetchAllPartners,
      insertPartner,
      updatePartner,
      deletePartner
    }
  },
  {
    persist: true
  }
)
