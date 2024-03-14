import { API_BASEURL, API_ROUTES } from '@/constants/AppConstants'
import type { ClientDTO } from '@/models/ClientDTO'
import { ref } from 'vue'

const data = ref<ClientDTO[]>([])
const isLoading = ref<boolean>(false)
const isError = ref<boolean>(false)

export function useClients() {
  //refecth clients when window gains focus (helps to keep data current)
  window.addEventListener('focus', () => {
    fetchClients()
  })
  fetchClients()
  //fetch function
  async function fetchClients() {
    try {
      isLoading.value = true

      const res = await fetch(`${API_BASEURL}/${API_ROUTES.clients}`)
      console.log(API_ROUTES.clients)

      if (!res.ok) {
        throw new Error('Failed to Fetch Data')
      }

      const fetchedData: ClientDTO[] = await res.json()
      data.value = fetchedData
      console.log(fetchedData)
    } catch (error) {
      isError.value = true
    } finally {
      isLoading.value = false
    }
  }
  return { data, isLoading, isError, fetchClients }
}
