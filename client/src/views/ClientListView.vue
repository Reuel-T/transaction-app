<!-- Client List Page -->
<template>
  <AppPageWrapper>
    <div class="container">
      <h1 class="title">Clients</h1>
      <div v-if="isLoading">
        <p>Loading...</p>
      </div>
      <div v-else-if="isError">
        <p>There was an Error</p>
      </div>

      <!-- Form to create a client -->
      <div v-if="!isError && !isLoading">
        <CreateClientForm
          style="margin-bottom: 0.5rem"
          @client-posted="
            (newClient: ClientDTO) => {
              data.push(newClient)
            }
          "
        />
      </div>

      <!-- List of clients -->
      <div
        class="card container"
        style="margin-bottom: 0.5rem"
        v-if="!isLoading && !isError"
      >
        <ClientList :clients="data" />
      </div>
    </div>
  </AppPageWrapper>
</template>

<script setup lang="ts">
  import CreateClientForm from '@/components/page/client-list/CreateClientForm.vue'
  import AppPageWrapper from '@/components/global/AppPageWrapper.vue'
  import ClientList from '@/components/page/client-list/ClientList.vue'
  import { useClients } from '@/shared/useGetClients'
  import type { ClientDTO } from '@/models/ClientDTO'

  const { data, isError, isLoading } = useClients()
</script>

<style scoped lang="scss">
  .container {
    height: 100%;
    display: flex;
    flex-direction: column;
    overflow-y: hidden;
  }
</style>
