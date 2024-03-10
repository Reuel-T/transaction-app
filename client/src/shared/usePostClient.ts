import { API_BASEURL, API_ROUTES } from '@/constants/AppConstants'
import type { ClientDTO } from '@/models/ClientDTO'
import type { CreateClientDTO } from '@/models/CreateClientDTO'
import { ref } from 'vue'

interface UsePostClientProps {
  onSuccess?: (data: ClientDTO) => void
  onError?: () => void
}

const isPosting = ref<boolean>(false)

export function usePostClient({ onSuccess = () => {}, onError = () => {} }: UsePostClientProps) {
  function postData(requestBody: CreateClientDTO) {
    postClient(requestBody)
  }

  async function postClient(requestBody: CreateClientDTO) {
    try {
      const requestOptions = {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      }

      isPosting.value = true

      const res = await fetch(`${API_BASEURL}/${API_ROUTES.clients}`, requestOptions)

      if (res.status === 201) {
        const data: ClientDTO = await res.json()
        onSuccess(data)
      } else {
        onError()
        throw new Error('Error on making new client')
      }
    } catch (error) {
      console.log('Error Posting new Client')
    } finally {
      isPosting.value = false
    }
  }

  return { isPosting, postData }
}
