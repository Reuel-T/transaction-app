<template>
  <div>
    <h1>Client View</h1>
    <h3>{{ $route.params.id }}</h3>

    <div v-if="isClientLoading">
      <p>Loading...</p>
    </div>

    <div v-if="foundClient === false">
      <h3>Client Not Found {{ $route.params.id }}</h3>
    </div>
    <div v-else-if="!isClientError && !isClientError && foundClient && client">
      <ClientData :client="client" />
    </div>
  </div>
</template>

<script setup lang="ts">
  import ClientData from '@/components/ClientData.vue'
  import { useGetClient } from '@/shared/useGetClient'
  import { useRoute } from 'vue-router'

  const route = useRoute()
  const {
    client,
    foundClient,
    isError: isClientError,
    isLoading: isClientLoading
  } = useGetClient(Number(route.params.id))
</script>

<style scoped></style>
