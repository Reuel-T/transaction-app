import { API_BASEURL, API_ROUTES } from '@/constants/AppConstants'
import type { Client } from '@/models/Client'
import { ref } from 'vue'

const data = ref<Client[]>([])
const isLoading = ref<boolean>(false)
const isError = ref<boolean>(false)

export function useClients() {
  window.addEventListener('focus', () => {
    fetchClients()
  })
  fetchClients()
  async function fetchClients() {
    try {
      isLoading.value = true

      const res = await fetch(`${API_BASEURL}/${API_ROUTES.clients}`)
      console.log(API_ROUTES.clients)

      if (!res.ok) {
        throw new Error('Failed to Fetch Data')
      }

      const fetchedData: Client[] = await res.json()
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
