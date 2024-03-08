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
      <CreateClientTransactionForm
        @post-success="
          () => {
            refetchClient()
            refetchTransactions()
          }
        "
        :client-id="Number($route.params.id)"
      />
      <ClientTransactions
        v-if="!isTransactionsError && !isTransactionsLoading && !transactions404 && transactions"
        :transactions="transactions"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
  import ClientData from '@/components/ClientData.vue'
  import ClientTransactions from '@/components/ClientTransactions.vue'
  import CreateClientTransactionForm from '@/components/CreateClientTransactionForm.vue'
  import type { CreateClientTransactionDTO } from '@/models/CreateClientTransactionDTO'
  import { useGetClient } from '@/shared/useGetClient'
  import { useGetClientTransactions } from '@/shared/useGetClientTransactions'
  import { useRoute } from 'vue-router'

  const route = useRoute()
  const {
    client,
    foundClient,
    isError: isClientError,
    isLoading: isClientLoading,
    fetchClient: refetchClient
  } = useGetClient(Number(route.params.id))

  const {
    transactions,
    is404: transactions404,
    isLoading: isTransactionsLoading,
    isError: isTransactionsError,
    fetchClientTransactions: refetchTransactions
  } = useGetClientTransactions(Number(route.params.id))
</script>

<style scoped></style>
