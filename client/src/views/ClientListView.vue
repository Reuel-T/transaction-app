<template>
  <AppPageWrapper>
    <h1 class="title">Clients</h1>
    <div v-if="isLoading">
      <p>Loading...</p>
    </div>
    <div v-else-if="isError">
      <p>There was an Error</p>
    </div>
    <div
      v-if="!isLoading && !isError"
      style="margin-top: 1rem; margin-bottom: 0.5rem"
    >
      <ClientList :clients="data" />
    </div>
  </AppPageWrapper>
</template>

<script setup lang="ts">
  import AppPageWrapper from '@/components/global/AppPageWrapper.vue'
  import ClientList from '@/components/ClientList.vue'
  import { useClients } from '@/shared/useGetClients'

  const { data, isError, isLoading } = useClients()
</script>

<style scoped lang="scss"></style>
