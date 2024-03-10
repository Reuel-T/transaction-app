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

      <div
        class="card container"
        v-if="!isLoading && !isError"
      >
        <ClientList :clients="data" />
      </div>
    </div>
  </AppPageWrapper>
</template>

<script setup lang="ts">
  import AppPageWrapper from '@/components/global/AppPageWrapper.vue'
  import ClientList from '@/components/page/client-list/ClientList.vue'
  import { useClients } from '@/shared/useGetClients'

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
