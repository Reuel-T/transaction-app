import { API_BASEURL, API_ROUTES } from '@/constants/AppConstants'
import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
import type { CreateClientTransactionDTO } from '@/models/CreateClientTransactionDTO'
import { ref } from 'vue'

interface UsePostClientTransactionProps {
  onSuccess?: (data: ClientTransactionDTO) => void
  onError?: () => void
}

const isPosting = ref<boolean>(false)

export function usePostClientTransaction({
  onSuccess = () => {},
  onError = () => {}
}: UsePostClientTransactionProps) {
  function postData(requestBody: CreateClientTransactionDTO) {
    postClient(requestBody)
  }

  async function postClient(requestBody: CreateClientTransactionDTO) {
    try {
      const requestOptions = {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      }

      isPosting.value = true

      const res = await fetch(`${API_BASEURL}/${API_ROUTES.transactions}`, requestOptions)

      if (res.status === 201) {
        const data: ClientTransactionDTO = await res.json()
        onSuccess(data)
      } else {
        onError()
        throw new Error('Error on making new transaction')
      }
    } catch (error) {
      console.log('Error Posting Transaction')
    } finally {
      isPosting.value = false
    }
  }

  return { isPosting, postData }
}
