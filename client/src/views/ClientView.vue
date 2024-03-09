<template>
  <AppPageWrapper>
    <div class="container">
      <h1 class="title">Client Transactions</h1>

      <div class="container" v-if="foundClient && !isClientError">
        <!-- Client Info -->
        <ClientInfoCard :client="client" :is-loading="isClientLoading"/>
        
        <!-- Create Transaction Form -->
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

        <div class="container">
          <ClientTransactions
            v-if="!isTransactionsError && !isTransactionsLoading && !transactions404 && transactions"
            :transactions="transactions"
          />
        </div>

      </div>

    </div>
  </AppPageWrapper>
</template>

<script setup lang="ts">
  import AppPageWrapper from '@/components/global/AppPageWrapper.vue'
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
  } = useGetClientTransactions(Number(route.params.id))
</script>

<style lang="scss" scoped>
  .container{
    height: 100%;
    display: flex;
    flex-direction: column;
    border: 2px solid aqua;
    overflow-y: hidden;
  }
</style>
