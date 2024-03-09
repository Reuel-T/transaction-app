<template>
  <div class="page-card">
    <h1 class="title">Client Transactions</h1>
    
    <div v-if="foundClient === false">
      <h3>Client Not Found {{ $route.params.id }}</h3>
    </div>
    
    <div v-else-if="!isClientError">
      <ClientInfoCard :client="client" :is-loading="isClientLoading"/>
      <CreateClientTransactionForm
        @post-success="
          (data : ClientTransactionDTO) => {
            refetchClient()
            console.log(data);
            if (transactions) {
              transactions.push(data);  
            }
          }
        "
        :client-id="Number($route.params.id)"
      />
      <div class="transaction-list-wrapper">
        <ClientTransactions
          v-if="!isTransactionsError && !isTransactionsLoading && !transactions404 && transactions"
          :transactions="transactions"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import ClientInfoCard from '@/components/page/client-transaction/ClientInfoCard.vue'
  import ClientTransactions from '@/components/ClientTransactions.vue'
  import CreateClientTransactionForm from '@/components/CreateClientTransactionForm.vue'
  import { useGetClient } from '@/shared/useGetClient'
  import { useGetClientTransactions } from '@/shared/useGetClientTransactions'
  import { useRoute } from 'vue-router'
import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'

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

<style scoped>
  .transaction-list-wrapper{
    max-height: 100%;
    border: 2px solid aqua;
    overflow-y: auto;
  }
</style>
