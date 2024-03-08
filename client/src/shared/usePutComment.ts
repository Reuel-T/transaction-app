import { API_BASEURL, API_ROUTES } from '@/constants/AppConstants'
import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
import { ref } from 'vue'

interface UsePutClientTransactionProps {
  onSuccess?: (transaction: ClientTransactionDTO) => void
  onError?: () => void
}

const isUpdating = ref<boolean>(false)

export function usePutTransactionComment({
  onSuccess = () => {},
  onError = () => {}
}: UsePutClientTransactionProps) {
  function putData(requestBody: { transactionID: number; comment: string }) {
    putComment(requestBody)
  }

  async function putComment(requestBody: { transactionID: number; comment: string }) {
    try {
      const requestOptions = {
        method: 'PUT', // Change the HTTP method to PUT
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      }

      isUpdating.value = true

      const res = await fetch(
        `${API_BASEURL}/${API_ROUTES.transactions}/comment/${requestBody.transactionID}`,
        requestOptions
      )

      const data: ClientTransactionDTO = await res.json()

      if (res.status === 201) {
        onSuccess(data)
      } else {
        onError()
        throw new Error('Error on updating transaction')
      }
    } catch (error) {
      console.error('Error Updating Transaction Comment')
    } finally {
      isUpdating.value = false
    }
  }

  return { isUpdating, putData }
}
