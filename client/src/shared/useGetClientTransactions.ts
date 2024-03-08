import { API_BASEURL, API_ROUTES } from '@/constants/AppConstants'
import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
import { ref } from 'vue'

const isError = ref<boolean>(false)
const isLoading = ref<boolean>(false)
const is404 = ref<null | boolean>(null)
const transactions = ref<ClientTransactionDTO[]>()

export function useGetClientTransactions(clientID: number) {
  window.addEventListener('focus', () => {
    fetchClientTransactions()
  })
  fetchClientTransactions()
  async function fetchClientTransactions() {
    try {
      isLoading.value = true
      const res = await fetch(`${API_BASEURL}/${API_ROUTES.transactions}/client/${clientID}`)

      if (res.ok) {
        //request successful
        const fetchedData: ClientTransactionDTO[] = await res.json()
        transactions.value = fetchedData
        is404.value = false
        isLoading.value = false
      }

      if (res.status === 404) {
        //client not found
        is404.value = true
        throw new Error(`Client with ID ${clientID} not found`)
      }

      if (!res.ok) {
        //server error?
        is404.value = null
        throw new Error('Unable to fetch client')
      }
    } catch (error) {
      console.error('Error Fetching Client transactions')
      isError.value = true
    } finally {
      isLoading.value = false
    }
  }

  return { transactions, isError, isLoading, is404, fetchClientTransactions }
}
