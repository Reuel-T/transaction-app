import { API_BASEURL, API_ROUTES } from "@/constants/AppConstants";
import type { Client } from "@/models/Client";
import { ref } from "vue";

const isError = ref<boolean>(false);
const isLoading = ref<boolean>(false);
const foundClient = ref<null | boolean>(null);
const client = ref<Client>()

export function useGetClient(clientID: number) {
  window.addEventListener("focus", () => { });
  fetchClient();
  async function fetchClient() {
    try {
      isLoading.value = true;
      const res = await fetch(`${API_BASEURL}/${API_ROUTES.clients}/${clientID}`);

      if (res.ok) {
        const fetchedData : Client = await res.json();
        client.value = fetchedData;
        foundClient.value = true;
        isLoading.value = false;
      }

      if (res.status === 404) {
        //client not found
        foundClient.value = false;
        throw new Error(`Client with ID ${clientID} not found`);
      }

      if (!res.ok) {
        //server error?
        foundClient.value = null;
        throw new Error('Unable to fetch client');
      }


    } catch (error) {
      console.error('Error Fetching Client')
      isError.value = true;
    } finally {
      isLoading.value = false;
    }
  }

  return {client, isError, isLoading, foundClient, fetchClient}
}